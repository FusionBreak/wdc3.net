using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wdc3.net.Enums;
using wdc3.net.File;

namespace wdc3.net.Table
{
    public class Db2ValueInserterNoOffsetFlag
    {
        private decimal _recordDataSize;
        private decimal _recordSize;

        private readonly byte[] _palletData;
        private readonly byte[] _commonData;
        public IEnumerable<byte> PalletData => PalletValues.Where(value => value != null).SelectMany(data => BitConverter.GetBytes(data ?? 0));
        public IEnumerable<byte> CommonData => new List<byte>();

        public IEnumerable<Section> Sections => _sections.Values;

        private Section CurrentSection => _sections[_tactKeyHash];
        private ulong _tactKeyHash;
        private Dictionary<ulong, Section> _sections = new();

        private List<uint> _currentIdList = new();
        private List<BitArray> _currentSectionData = new();
        private List<string> _currentSectionStrings = new() { " " };

        public int?[] PalletValues = new int?[1000];

        public Db2ValueInserterNoOffsetFlag(decimal recordDataSize, decimal recordSize)
        {
            _recordDataSize = recordDataSize;
            _recordSize = recordSize;
        }

        public IEnumerable<int?> Foo => PalletValues.Where(value => value != null);

        public void ProcessRow(Db2Row row)
        {
            UpdateSection(row.DependentSectionHeader);

            uint _additionalDataOffset = 0;

            foreach(var cell in row.Cells)
            {
                switch(cell.FieldStorageInfo?.StorageType)
                {
                    case FieldCompressions.None:
                        if(cell.ColumnInfo?.ArrayLength > 0)
                        {
                            var size = cell.FieldStorageInfo.FieldSizeBits / cell.ColumnInfo.ArrayLength;
                            var arrayValues = ((List<object>)(cell?.Value ?? throw new NullReferenceException(nameof(Db2Cell.Value)))).Select(value => (int)value).ToArray();

                            for(int i = 0; i < arrayValues.Length; i++)
                            {
                                WriteNumber(arrayValues[i], size);
                            }
                        }
                        else
                        {
                            if(cell.ColumnInfo?.Type == Db2ValueTypes.Text)
                            {
                                var stringOffset = AddString((string)(cell.Value ?? throw new NullReferenceException(nameof(Db2Cell.Value)))) - cell.FieldStructure?.Position ?? throw new NullReferenceException(nameof(Db2Cell.FieldStructure));
                                WriteNumber(stringOffset, cell.FieldStorageInfo.FieldSizeBits);
                            }
                            else
                            {
                                WriteNumber((int)(cell.Value ?? throw new NullReferenceException(nameof(Db2Cell.Value))), cell.FieldStorageInfo.FieldSizeBits);
                            }
                        }
                        break;

                    case FieldCompressions.Bitpacked:
                    case FieldCompressions.BitpackedSigned:
                        WriteNumber((int)(cell.Value ?? throw new NullReferenceException(nameof(Db2Cell.Value))), cell.FieldStorageInfo.FieldSizeBits);
                        break;

                    case FieldCompressions.BitpackedIndexed:
                        var offset = _additionalDataOffset / 4;
                        var palletIndex = AddPalletValue((int)(cell.Value ?? throw new NullReferenceException(nameof(Db2Cell.Value))), offset);
                        _additionalDataOffset += cell.FieldStorageInfo.AdditionalDataSize;
                        WriteNumber(palletIndex, cell.FieldStorageInfo.FieldSizeBits);
                        break;

                    case FieldCompressions.BitpackedIndexedArray:
                        var offset_array = _additionalDataOffset / 4;
                        var values = (cell.Value as List<object> ?? throw new NullReferenceException(nameof(Db2Cell.Value))).Select(value => (int)value);
                        var palletIndex_array = AddPalletArray(values.ToArray(), offset_array);
                        _additionalDataOffset += cell.FieldStorageInfo.AdditionalDataSize;
                        WriteNumber(palletIndex_array, cell.FieldStorageInfo.FieldSizeBits);
                        break;
                }
            }

            _currentIdList.Add(row.Id);
        }

        private void UpdateSection(SectionHeader sectionHeader)
        {
            if(!_sections.ContainsKey(sectionHeader.TactKeyHash))
                _sections.Add(sectionHeader.TactKeyHash, new Section()
                {
                    Records = new List<RecordData>(),
                    StringData = new List<byte>(),
                    IdList = new List<uint>(),
                    CopyTable = new List<CopyTableEntry>(),
                    OffsetMap = new List<OffsetMapEntry>(),
                    RelationshipMap = null,
                    OffsetMapIdList = new List<uint>()
                });

            if(_tactKeyHash != sectionHeader.TactKeyHash)
            {
                InsertDataToSection();
                _tactKeyHash = sectionHeader.TactKeyHash;
            }
        }

        private void InsertDataToSection()
        {
            CurrentSection.Records = ConvertSectionData().ToArray();
        }

        private void WriteNumber(int value, int size)
        {
            var bits = new BitArray(BitConverter.GetBytes(value));
            var bools = new List<bool>();

            for(int bitIndex = 0; bitIndex < size; bitIndex++)
                bools.Add(bits[bitIndex]);

            _currentSectionData.Add(new BitArray(bools.ToArray()));
        }

        private IEnumerable<RecordData> ConvertSectionData()
        {
            var boolArrays = FillBoolArray(_currentSectionData.Select(bitArray => bitArray.Cast<bool>()).SelectMany(x => x));
            var bits = new BitArray(boolArrays);

            var output = new byte[bits.Length / 8];
            bits.CopyTo(output, 0);

            for(int i = 0; i < output.Length; i += (int)_recordSize)
            {
                var data = output.Skip(i).Take((int)_recordSize).ToArray();

                //if(data.Length < _recordSize)
                //    throw new Exception($"RecordData is too small. Size is {data.Length} but it must have the size {_recordSize}");

                yield return new RecordData() { Data = data };
            }
        }

        private bool[] FillBoolArray(IEnumerable<bool> bools)
        {
            var output = bools.ToList();

            while(output.Count % 8 != 0)
                output.Add(false);

            return output.ToArray();
        }

        private int AddPalletValue(int value, uint index)
        {
            var output = (int)index;
            if(PalletValues[index] != null && PalletValues[index] != value)
            {
                output = AddPalletValue(value, index + 1);
            }
            else
            {
                PalletValues[index] = value;
            }

            return output;
        }

        private int AddPalletArray(int[] values, uint index)
        {
            var output = (int)index;
            if(CheckAllValuesFromArray(values, index).Contains(false))
            {
                output = AddPalletArray(values, index + 1);
            }
            else
            {
                for(int i = 0; i < values.Length; i++)
                {
                    PalletValues[index + i] = values[i];
                }
            }

            return output;

            IEnumerable<bool> CheckAllValuesFromArray(int[] values, uint index)
            {
                for(int i = 0; i < values.Length; i++)
                {
                    yield return PalletValues[index + i] == null || PalletValues[index + i] == values[i];
                }
            }
        }

        private int AddString(string value)
        {
            value = value == "" ? " " : value;

            if(!_currentSectionStrings.Contains(value))
                _currentSectionStrings.Add(value);

            var offset = _currentSectionStrings.Take(_currentSectionStrings.IndexOf(value)).Select(s => s.Length + 1).Sum();

            return (int)_recordDataSize + offset;
        }
    }
}