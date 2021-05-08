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
        private readonly byte[] _palletData;
        private readonly byte[] _commonData;
        public IEnumerable<byte> RecordData => ConvertSectionData();
        public IEnumerable<byte> PalletData => PalletValues.Where(value => value != null).SelectMany(data => BitConverter.GetBytes(data ?? 0));

        private Dictionary<SectionHeader, Section> _sections = new();
        private List<byte> _currentSectionStringData;
        private List<uint> _currentIdList;

        private List<BitArray> _currentSectionData = new();

        public int?[] PalletValues = new int?[1000];

        public IEnumerable<int?> Foo => PalletValues.Where(value => value != null);

        //private uint _additionalDataOffset = 0;

        public void ProcessRow(Db2Row row)
        {
            uint _additionalDataOffset = 0;

            foreach(var cell in row.Cells)
            {
                switch(cell.FieldStorageInfo?.StorageType)
                {
                    case FieldCompressions.None:
                        if(cell.ColumnInfo?.ArrayLength > 0)
                        {
                        }
                        else
                        {
                            if(cell.ColumnInfo?.Type == Db2ValueTypes.Text)
                            {
                                //var stringOffset = value + fieldStructure.Position + (_recordSize * _currentRow);

                                //return ReadString(stringOffset);
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
        }

        private void WriteNumber(int value, int size)
        {
            var bits = new BitArray(BitConverter.GetBytes(value));
            var bools = new List<bool>();

            for(int bitIndex = 0; bitIndex < size; bitIndex++)
                bools.Add(bits[bitIndex]);

            _currentSectionData.Add(new BitArray(bools.ToArray()));
        }

        private byte[] ConvertSectionData()
        {
            var boolArrays = FillBoolArray(_currentSectionData.Select(bitArray => bitArray.Cast<bool>()).SelectMany(x => x));
            var bits = new BitArray(boolArrays);

            var output = new byte[bits.Length / 8];
            bits.CopyTo(output, 0);

            return output;
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
    }
}