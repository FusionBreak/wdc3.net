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
        private IEnumerable<byte> __recordDataCombined => _recordData.Concat(_recordStringData);
        //private int _recordDataPosition = 0;

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
                    var fieldOffset = fieldStorageInfo.FieldOffsetBits / 8;
                    var size = fieldStorageInfo.FieldSizeBits / 8;
                    var value = readInt(fieldOffset, size);
                    //_recordDataPosition += fieldStorageInfo.FieldSizeBits / 8;
                    return type == typeof(string) ? readString(value+fieldStructure.Position) : value;

                //case FieldCompressions.Bitpacked:
                //    return null;
                //case FieldCompressions.CommonData:
                //    return null;
                case FieldCompressions.BitpackedIndexed:
                    var index = _recordData.Skip(fieldStorageInfo.FieldOffsetBits / 8).First();
                    return _palletValues.Skip(index).First();
                //case FieldCompressions.BitpackedIndexedArray:
                //    return null;
                //case FieldCompressions.BitpackedSigned:
                //    return null;
                default:
                    return fieldStorageInfo.StorageType;
            }
        }

        private int readInt(int offset, int size)
        {
            var value = BitConverter.ToInt32(_recordData.Skip(offset).Take(size).ToArray());
            return value;
        }

        private string readString(int offset)
        {
            var chars = __recordDataCombined
                        .Skip(offset)
                        .TakeWhile(x => x != 0)
                        .Select(x => Convert.ToChar(x))
                        .ToArray();

            var x = offset - _recordData.Count();

            return new string(chars);
        }
    }
}
