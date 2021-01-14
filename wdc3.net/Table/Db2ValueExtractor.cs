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

        private int _recordSize = 0;
        private byte[] _palletData;
        private byte[] _commonData;
        private byte[] _recordData;
        private byte[] _recordStringData;
        private byte[] _recordDataCombined;
        private BitArray _recordDataAsBits;

        //private int _recordDataPosition = 0;
        private int _palletDataPosition = 0;

        private int _recordDataBitPosition = 0;

        private uint _additionalDataOffset = 0;

        private int _currentRow = 0;
        private int _rowBitSize = 0;
        private int _currentRowBitOffset => _currentRow * _rowBitSize;
        //private int _rowStringBitSize = 0;

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

        public Db2ValueExtractor(IEnumerable<byte> palletData, IEnumerable<byte> commonData, IEnumerable<byte> recordData, IEnumerable<byte> recordStringData, int rowBitSize, int recordSize)
        {
            _palletData = palletData.ToArray() ?? throw new ArgumentNullException(nameof(palletData));
            _commonData = commonData.ToArray() ?? throw new ArgumentNullException(nameof(commonData));
            _recordData = recordData.ToArray() ?? throw new ArgumentNullException(nameof(recordData));
            _recordStringData = recordStringData.ToArray() ?? throw new ArgumentNullException(nameof(recordStringData));
            _recordDataCombined = _recordData.Concat(_recordStringData).ToArray();

            _rowBitSize = FillBitSizeToByte(rowBitSize);
            _recordDataAsBits = new BitArray(recordData.ToArray());
            _recordSize = recordSize;
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
                        {
                            output.Add(ReadInt(_currentRowBitOffset + fieldStorageInfo.FieldOffsetBits + (size * i), size));
                        }

                        return JsonSerializer.Serialize(output);
                    }
                    else
                    {
                        var value = ReadInt(_currentRowBitOffset + fieldStorageInfo.FieldOffsetBits, fieldStorageInfo.FieldSizeBits);
                        return columnInfo.Type == Db2ValueTypes.Text ? readString(value + fieldStructure.Position + (_recordSize * _currentRow)) : value;
                    }

                case FieldCompressions.Bitpacked:
                case FieldCompressions.BitpackedSigned:
                    return ReadInt(_currentRowBitOffset + fieldStorageInfo.FieldOffsetBits, fieldStorageInfo.FieldSizeBits);

                // For compression types '3' and '4', you need to take the bitpacked value you obtained from the record
                // and use it as an index into 'pallet_data'. For compression type '3', you pull a 4-byte value from 'pallet_data'
                // using the formula 'additional_data_offset + (index * 4)', where 'additional_data_offset' is
                // the sum of 'additional_data_size' for every column before the current one.
                case FieldCompressions.BitpackedIndexed:
                    var index = ReadInt(_currentRowBitOffset + fieldStorageInfo.FieldOffsetBits, fieldStorageInfo.FieldSizeBits);
                    var offset = _additionalDataOffset / 4 + (index);
                    var value2 = _palletValues.Skip((int)offset).First();
                    _additionalDataOffset += fieldStorageInfo.AdditionalDataSize;
                    return value2;

                case FieldCompressions.BitpackedIndexedArray:
                    var index_array = ReadInt(_currentRowBitOffset + fieldStorageInfo.FieldOffsetBits, fieldStorageInfo.FieldSizeBits);
                    var offset_array = _additionalDataOffset / 4 + (index_array);

                    var output2 = new List<object>();
                    for(int i = 0; i < columnInfo.ArrayLength; i++)
                    {
                        output2.Add(_palletValues.Skip((int)offset_array + i).First());
                    }

                    _additionalDataOffset += fieldStorageInfo.AdditionalDataSize;

                    return JsonSerializer.Serialize(output2);

                default:
                    return fieldStorageInfo.StorageType;
            }
        }

        private int ReadInt(int offsetInBits, int sizeInBits)
        {
            //int maxPossibleValue = (int)Math.Pow(2, sizeInBits) - 1;
            int output = 0;

            for(int bitIndex = 0; bitIndex < sizeInBits; bitIndex++)
            {
                var bit = _recordDataAsBits.Get(offsetInBits + bitIndex) ? 1 : 0;
                output |= bit << bitIndex;
            }

            return output;
            //return output == maxPossibleValue ? -1 : output;
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

        private int FillBitSizeToByte(int offset)
        {
            while(offset % 8 != 0)
            {
                offset++;
            }

            return offset;
        }

        public void NextRow()
        {
            _currentRow++;
            _additionalDataOffset = 0;
        }
    }
}