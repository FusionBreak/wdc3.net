using System.Collections.Generic;
using System.IO;
using wdc3.net.File;

namespace wdc3.net.Reader
{
    public class FieldStorageInfoReader : IFileReader<IEnumerable<IFieldStorageInfo>>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles")]
        private const int SIZE_OF_FIELD_STORAGE_INFO = 24;

        private readonly BinaryReader _reader;
        private readonly int _totalFieldStorageInfoSize;
        private int FieldStorageInfoCount => _totalFieldStorageInfoSize / SIZE_OF_FIELD_STORAGE_INFO;
        public long Position { get; private set; }

        public FieldStorageInfoReader(BinaryReader reader, int totalFieldStorageInfoSize)
        {
            _reader = reader;
            _totalFieldStorageInfoSize = totalFieldStorageInfoSize;
        }

        public IEnumerable<IFieldStorageInfo> Read()
        {
            var output = new List<IFieldStorageInfo>();

            for(var currentFieldStorageCount = 0; currentFieldStorageCount < FieldStorageInfoCount; currentFieldStorageCount++)
            {
                var fieldOffsetBits = _reader.ReadUInt16();
                var fieldSizeBits = _reader.ReadUInt16();
                var additionalDataSize = _reader.ReadUInt32();
                var storageType = (FieldCompressions)_reader.ReadInt32();
                var value1 = _reader.ReadUInt32();
                var value2 = _reader.ReadUInt32();
                var value3 = _reader.ReadUInt32();

                IFieldStorageInfo fieldStorageInfo = storageType switch
                {
                    FieldCompressions.Bitpacked => new FieldStorageInfoBitPacked()
                    {
                        FieldOffsetBits = fieldOffsetBits,
                        FieldSizeBits = fieldSizeBits,
                        AdditionalDataSize = additionalDataSize,
                        StorageType = storageType,
                        BitpackingOffsetBits = value1,
                        BitpackingSizeBits = value2,
                        Flags = value3
                    },
                    FieldCompressions.BitpackedSigned => new FieldStorageInfoBitPacked()
                    {
                        FieldOffsetBits = fieldOffsetBits,
                        FieldSizeBits = fieldSizeBits,
                        AdditionalDataSize = additionalDataSize,
                        StorageType = storageType,
                        BitpackingOffsetBits = value1,
                        BitpackingSizeBits = value2,
                        Flags = value3
                    },
                    FieldCompressions.CommonData => new FieldStorageInfoCommonData()
                    {
                        FieldOffsetBits = fieldOffsetBits,
                        FieldSizeBits = fieldSizeBits,
                        AdditionalDataSize = additionalDataSize,
                        StorageType = storageType,
                        DefaultValue = value1,
                        UnkOrUnused2 = value2,
                        UnkOrUnused3 = value3
                    },
                    FieldCompressions.BitpackedIndexed => new FieldStorageInfoBitPackedIndexed()
                    {
                        FieldOffsetBits = fieldOffsetBits,
                        FieldSizeBits = fieldSizeBits,
                        AdditionalDataSize = additionalDataSize,
                        StorageType = storageType,
                        BitpackingOffsetBits = value1,
                        BitpackingSizeBits = value2,
                        UnkOrUnused3 = value3
                    },
                    FieldCompressions.BitpackedIndexedArray => new FieldStorageInfoBitPackedIndexedArray()
                    {
                        FieldOffsetBits = fieldOffsetBits,
                        FieldSizeBits = fieldSizeBits,
                        AdditionalDataSize = additionalDataSize,
                        StorageType = storageType,
                        BitpackingOffsetBits = value1,
                        BitpackingSizeBits = value2,
                        ArrayCount = value3
                    },
                    _ => new FieldStorageInfo()
                    {
                        FieldOffsetBits = fieldOffsetBits,
                        FieldSizeBits = fieldSizeBits,
                        AdditionalDataSize = additionalDataSize,
                        StorageType = storageType,
                        UnkOrUnused1 = value1,
                        UnkOrUnused2 = value2,
                        UnkOrUnused3 = value3
                    },
                };

                output.Add(fieldStorageInfo);
            }

            Position = _reader.BaseStream.Position;

            return output;
        }
    }
}