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

        private Dictionary<SectionHeader, Section> _sections = new();
        private List<byte> _currentSectionStringData;
        private List<uint> _currentIdList;

        private List<BitArray> _currentSectionData = new();

        public void ProcessRow(Db2Row row)
        {
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

                    case FieldCompressions.CommonData:
                        break;

                    case FieldCompressions.BitpackedIndexed:
                        break;

                    case FieldCompressions.BitpackedIndexedArray:
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
    }
}