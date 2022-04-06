using System;
using wdc3.net.File;

namespace wdc3.net.Table
{
    public class Db2CreateInformationFactory
    {
        public static Db2CreateInformation CreateDb2CreateInformation(Db2 db2)
        {
            return new Db2CreateInformation()
            {
                Magic = db2.Header?.Magic ?? throw new NullReferenceException(nameof(Db2.Header)),
                TableHash = db2.Header?.TableHash ?? throw new NullReferenceException(nameof(Db2.Header)),
                LayoutHash = db2.Header?.LayoutHash ?? throw new NullReferenceException(nameof(Db2.Header)),
                Locale = db2.Header?.Locale ?? throw new NullReferenceException(nameof(Db2.Header)),
                Flags = db2.Header?.Flags ?? throw new NullReferenceException(nameof(Db2.Header)),
                FieldStorageInfos = db2.FieldStorageInfos,
                FieldStorageInfoSize = db2.Header?.FieldStorageInfoSize ?? throw new NullReferenceException(nameof(Db2.Header)),
                BitpackedDataOffset = db2.Header?.BitpackedDataOffset ?? throw new NullReferenceException(nameof(Db2.Header)),
                LookUpColumnCount = db2.Header?.LookUpColumnCount ?? throw new NullReferenceException(nameof(Db2.Header)),
                FieldStructures = db2.FieldStructures
            };
        }
    }
}