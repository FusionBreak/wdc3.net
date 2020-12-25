using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using wdc3.net.File;

namespace wdc3.net.Table
{
    public class Db2ValueExtractor
    {
        private const int PALLET_VALUE_SIZE = sizeof(int);

        private IEnumerable<byte> _palletData;
        private IEnumerable<byte> _commonData;
        private IEnumerable<byte> _recordData;
        private IEnumerable<byte> _recordStringData;
        private IEnumerable<byte> _recordDataCombined => _recordData.Concat(_recordStringData);
        private BitArray _recordDataAsBits;

        //private int _recordDataPosition = 0;
        private int _palletDataPosition = 0;

        private int _recordDataBitPosition = 0;

        private int _currentRow = 0;
        private int _rowBitSize = 0;
        private int _currentRowBitOffset => _currentRow * _rowBitSize;

        private IEnumerable<int> _palletValues
        {
            get
            {
                for(int palletIndex = 0; palletIndex < (_palletData.Count() / PALLET_VALUE_SIZE); palletIndex++)
                {
                    yield return BitConverter.ToInt32(_palletData.Skip(palletIndex * PALLET_VALUE_SIZE).Take(PALLET_VALUE_SIZE).ToArray());
                }
            }
        }

        public Db2ValueExtractor(IEnumerable<byte> palletData, IEnumerable<byte> commonData, IEnumerable<byte> recordData, IEnumerable<byte> recordStringData, int rowBitSize)
        {
            _palletData = palletData ?? throw new ArgumentNullException(nameof(palletData));
            _commonData = commonData ?? throw new ArgumentNullException(nameof(commonData));
            _recordData = recordData ?? throw new ArgumentNullException(nameof(recordData));
            _recordStringData = recordStringData ?? throw new ArgumentNullException(nameof(recordStringData));

            _rowBitSize = rowBitSize;
            _recordDataAsBits = new BitArray(recordData.ToArray());
        }

        public object ExtractValue(FieldStructure fieldStructure, IFieldStorageInfo fieldStorageInfo, ColumnInfo columnInfo)
        {
            _ = fieldStructure;
            switch(fieldStorageInfo.StorageType)
            {
                case FieldCompressions.None:
                    if(columnInfo.ArrayLength > 0)
                    {
                        var size = fieldStorageInfo.FieldSizeBits / columnInfo.ArrayLength;
                        var output = new List<object>();

                        for(int i = 0; i < columnInfo.ArrayLength; i++)
                            output.Add(ReadInt(_recordDataBitPosition, size));

                        return JsonSerializer.Serialize(output);
                    }
                    else
                    {
                        var value = ReadInt(_currentRowBitOffset + fieldStorageInfo.FieldOffsetBits, fieldStorageInfo.FieldSizeBits);
                        return columnInfo.Type == typeof(string) ? readString(value + fieldStructure.Position) : value; //$"{value} | {fieldStructure.Position}";
                    }
                // Bitpacked -- the field is a bitpacked integer in the record data.  It
                // is field_size_bits long and starts at field_offset_bits.
                // A bitpacked value occupies
                //   (field_size_bits + (field_offset_bits & 7) + 7) / 8
                // bytes starting at byte
                //   field_offset_bits / 8
                // in the record data.  These bytes should be read as a little-endian value,
                // then the value is shifted to the right by (field_offset_bits & 7) and
                // masked with ((1ull << field_size_bits) - 1).
                case FieldCompressions.Bitpacked:
                case FieldCompressions.BitpackedSigned:
                    //float a_size = (fieldStorageInfo.FieldSizeBits + (fieldStorageInfo.FieldOffsetBits & 7) + 7) / 8;
                    //BitArray.Cast
                    //BitConverter.ToInt32(null,)
                    //var tet = _recordDataAsBits.

                    //var a_offset = fieldStorageInfo.FieldOffsetBits / 8;
                    //var a_value = readByte(_recordDataPosition);
                    //var index = _palletDataPosition;
                    //var offset = index + fieldStorageInfo.
                    //var test = _recordData.Skip(0).Take(0).ToArray();
                    return ReadInt(_currentRowBitOffset + fieldStorageInfo.FieldOffsetBits, fieldStorageInfo.FieldSizeBits);
                //return readByte(_recordDataPosition);
                //case FieldCompressions.CommonData:
                //    return null;
                case FieldCompressions.BitpackedIndexed:
                    var index = ReadInt(_currentRowBitOffset + fieldStorageInfo.FieldOffsetBits, fieldStorageInfo.FieldSizeBits);
                    return "ERROR: " + _palletValues.Skip(index).First();
                //case FieldCompressions.BitpackedIndexedArray:
                //    return null;
                default:
                    return fieldStorageInfo.StorageType;
            }
        }

        private int ReadInt(int offsetInBits, int sizeInBits)
        {
            int maxPossibleValue = (int) Math.Pow(2, sizeInBits) - 1;
            int output = 0;

            for(int bitIndex = 0; bitIndex < sizeInBits; bitIndex++)
            {
                var test = _recordDataAsBits.Get(offsetInBits + bitIndex) ? 1 : 0;
                output |= test << bitIndex;
            }

            return output == maxPossibleValue ? -1 : output;
        }

        private string readString(int offset)
        {
            if(offset - _recordData.Count() < 0)
                return $"ERROR:{offset - _recordData.Count()} ";

            var chars = _recordDataCombined
                        .Skip(offset)
                        .TakeWhile(x => x != 0)
                        .Select(x => Convert.ToChar(x))
                        .ToArray();

            return new string(chars);
        }

        public void NextRow()
        {
            _currentRow++;
        }
    }
}