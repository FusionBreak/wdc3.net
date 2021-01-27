using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using wdc3.net.Enums;
using wdc3.net.File;

namespace wdc3.net.Table
{
    internal class Db2ValueExtractorWithOffsetFlag : IDb2ValueExtractor
    {
        private const int PALLET_VALUE_SIZE = sizeof(int);
        private byte[] _recordData;
        private int _recordDataOffset;

        public Db2ValueExtractorWithOffsetFlag(IEnumerable<byte> recordData, int recordDataOffset)
        {
            _recordData = recordData.ToArray() ?? throw new ArgumentNullException(nameof(recordData));
            _recordDataOffset = recordDataOffset;
        }

        public object ExtractValue(FieldStructure fieldStructure, IFieldStorageInfo fieldStorageInfo, ColumnInfo columnInfo, RowInfo rowInfo)
        {
            var valueOffset = (int)(((rowInfo?.Offset ?? throw new Exception()) - _recordDataOffset) + fieldStorageInfo.FieldOffsetBits);

            return columnInfo.ArrayLength > 0
                ? JsonSerializer.Serialize(ExtractMany(columnInfo.Type, columnInfo.Size, columnInfo.IsSigned, valueOffset, columnInfo.ArrayLength).ToArray())
                : ExtractSingle(columnInfo.Type, columnInfo.Size, columnInfo.IsSigned, valueOffset);
        }

        public void NextRow()
        {
        }

        private IEnumerable<object> ExtractMany(Db2ValueTypes type, int size, bool isSigned, int valueOffset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                var output = ExtractSingle(type, size, isSigned, valueOffset + (size * i));

                if (output is float dOut)
                    yield return float.IsNaN(dOut) ? dOut.ToString() : output;
                else
                    yield return output;
            }
        }

        private object ExtractSingle(Db2ValueTypes type, int size, bool isSigned, int valueOffset) => (type, size, isSigned) switch
        {
            (Db2ValueTypes.Integer, 0, true) => ReadInteger(valueOffset / 8),
            (Db2ValueTypes.Integer, 8, _) => ReadByte(valueOffset / 8),
            (Db2ValueTypes.Integer, 16, true) => ReadShort(valueOffset / 8),
            (Db2ValueTypes.Integer, 32, true) => ReadInteger(valueOffset / 8),
            (Db2ValueTypes.Integer, 64, true) => ReadLong(valueOffset / 8),
            (Db2ValueTypes.Integer, 0, false) => ReadUInteger(valueOffset / 8),
            (Db2ValueTypes.Integer, 16, false) => ReadUShort(valueOffset / 8),
            (Db2ValueTypes.Integer, 32, false) => ReadUInteger(valueOffset / 8),
            (Db2ValueTypes.Integer, 64, false) => ReadULong(valueOffset / 8),
            (Db2ValueTypes.Float, 0, _) => ReadFloat(valueOffset / 8),
            (Db2ValueTypes.Float, 16, _) => throw new NotImplementedException(),
            (Db2ValueTypes.Float, 32, _) => ReadFloat(valueOffset / 8),
            (Db2ValueTypes.Float, 64, _) => ReadDouble(valueOffset / 8),
            (Db2ValueTypes.Text, _, _) => "???",
            _ => throw new NotImplementedException(),
        };

        private float ReadFloat(int offset) => BitConverter.ToSingle(_recordData, offset);

        private double ReadDouble(int offset) => BitConverter.ToDouble(_recordData, offset);

        private byte ReadByte(int offset) => _recordData.Skip(offset).First();

        private short ReadShort(int offset) => BitConverter.ToInt16(_recordData, offset);

        private int ReadInteger(int offset) => BitConverter.ToInt32(_recordData, offset);

        private long ReadLong(int offset) => BitConverter.ToInt64(_recordData, offset);

        private short ReadUShort(int offset) => BitConverter.ToInt16(_recordData, offset);

        private uint ReadUInteger(int offset) => BitConverter.ToUInt32(_recordData, offset);

        private long ReadULong(int offset) => BitConverter.ToInt64(_recordData, offset);
    }
}