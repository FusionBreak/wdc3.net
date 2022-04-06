using System;
using System.Collections.Generic;
using wdc3.net.File;

namespace wdc3.net.Writer
{
    internal class FieldStorageInfoSerializer
    {
        public byte[] Serialze(IEnumerable<IFieldStorageInfo> fieldStorageInfos)
        {
            var output = new List<byte>();

            foreach(var fieldStorageInfo in fieldStorageInfos)
            {
                if(fieldStorageInfo is null)
                    throw new Exception();

                output.AddRange(BitConverter.GetBytes(fieldStorageInfo.FieldOffsetBits));
                output.AddRange(BitConverter.GetBytes(fieldStorageInfo.FieldSizeBits));
                output.AddRange(BitConverter.GetBytes(fieldStorageInfo.AdditionalDataSize));
                output.AddRange(BitConverter.GetBytes((uint)fieldStorageInfo.StorageType));

                switch(fieldStorageInfo)
                {
                    case FieldStorageInfoBitPacked bitPacked:
                        output.AddRange(BitConverter.GetBytes(bitPacked.BitpackingOffsetBits));
                        output.AddRange(BitConverter.GetBytes(bitPacked.BitpackingSizeBits));
                        output.AddRange(BitConverter.GetBytes(bitPacked.Flags));
                        break;

                    case FieldStorageInfoCommonData commonData:
                        output.AddRange(BitConverter.GetBytes(commonData.DefaultValue));
                        output.AddRange(BitConverter.GetBytes(commonData.UnkOrUnused2));
                        output.AddRange(BitConverter.GetBytes(commonData.UnkOrUnused3));
                        break;

                    case FieldStorageInfoBitPackedIndexed bitPackedIndexed:
                        output.AddRange(BitConverter.GetBytes(bitPackedIndexed.BitpackingOffsetBits));
                        output.AddRange(BitConverter.GetBytes(bitPackedIndexed.BitpackingSizeBits));
                        output.AddRange(BitConverter.GetBytes(bitPackedIndexed.UnkOrUnused3));
                        break;

                    case FieldStorageInfoBitPackedIndexedArray bitPackedIndexedArray:
                        output.AddRange(BitConverter.GetBytes(bitPackedIndexedArray.BitpackingOffsetBits));
                        output.AddRange(BitConverter.GetBytes(bitPackedIndexedArray.BitpackingSizeBits));
                        output.AddRange(BitConverter.GetBytes(bitPackedIndexedArray.ArrayCount));
                        break;

                    case FieldStorageInfo fieldStorageInfoDefault:
                        output.AddRange(BitConverter.GetBytes(fieldStorageInfoDefault.UnkOrUnused1));
                        output.AddRange(BitConverter.GetBytes(fieldStorageInfoDefault.UnkOrUnused2));
                        output.AddRange(BitConverter.GetBytes(fieldStorageInfoDefault.UnkOrUnused3));
                        break;
                }
            }

            return output.ToArray();
        }
    }
}