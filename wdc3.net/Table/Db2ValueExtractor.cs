using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private int _recordDataPosition = 0;

        private IEnumerable<int> _palletValues
        {
            get
            {
                for(int palletIndex = 0; palletIndex < (_palletData.Count()/ PALLET_VALUE_SIZE); palletIndex++)
                {
                    yield return BitConverter.ToInt32(_palletData.Skip(palletIndex * PALLET_VALUE_SIZE).Take(PALLET_VALUE_SIZE).ToArray());
                }
            }
        }

        public Db2ValueExtractor(IEnumerable<byte> palletData, IEnumerable<byte> commonData, IEnumerable<byte> recordData, IEnumerable<byte> recordStringData)
        {
            _palletData = palletData ?? throw new ArgumentNullException(nameof(palletData));
            _commonData = commonData ?? throw new ArgumentNullException(nameof(commonData));
            _recordData = recordData ?? throw new ArgumentNullException(nameof(recordData));
            _recordStringData = recordStringData ?? throw new ArgumentNullException(nameof(recordStringData));
        }

        public object ExtractValue(FieldStructure fieldStructure, IFieldStorageInfo fieldStorageInfo, Type type)
        {
            _ = fieldStructure;
            switch(fieldStorageInfo.StorageType)
            {
                case FieldCompressions.None:
                    var size = fieldStorageInfo.FieldSizeBits / 8;
                    var offsetByteCount = fieldStorageInfo.FieldOffsetBits / 8;
                    var fieldOffset = _recordDataPosition;
                    var value = readInt(_recordDataPosition, size);
                    return type == typeof(string) ? readString(value + fieldStructure.Position) : value; //$"{value} | {fieldStructure.Position}"; //
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
                    //var a_size = (fieldStorageInfo.FieldSizeBits + (fieldStorageInfo.FieldOffsetBits & 7) + 7) / 8;
                    //var a_offset = fieldStorageInfo.FieldOffsetBits / 8;
                    //var a_value = readByte(_recordDataPosition);
                    return readByte(_recordDataPosition);
                //case FieldCompressions.CommonData:
                //    return null;
                case FieldCompressions.BitpackedIndexed:
                    var index = readByte(_recordDataPosition);
                    return _palletValues.Skip(index).First();
                //case FieldCompressions.BitpackedIndexedArray:
                //    return null;
                default:
                    return fieldStorageInfo.StorageType;
            }
        }

        private byte readByte(int offset)
        {
            var value = _recordData.Skip(_recordDataPosition).First();
            _recordDataPosition++;
            return value;
        }

        private int readInt(int offset, int size)
        {
            var value = BitConverter.ToInt32(_recordData.Skip(offset).Take(size).ToArray());
            _recordDataPosition += size;
            return value;
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
    }
}
