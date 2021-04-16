using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using wdc3.net.Enums;
using wdc3.net.File;

namespace wdc3.net.Table
{
    internal class Db2ValueExtractorWithOffsetFlag : IDb2ValueExtractor
    {
        private readonly byte[] _recordData;

        private int _stringCorrection = 0;

        public Db2ValueExtractorWithOffsetFlag(IEnumerable<byte> recordData) => _recordData = recordData.ToArray() ?? throw new ArgumentNullException(nameof(recordData));

        public object ExtractValue(FieldStructure fieldStructure, IFieldStorageInfo fieldStorageInfo, ColumnInfo columnInfo, RowInfo rowInfo)
        {
            var columnOffset = (int)fieldStorageInfo.FieldOffsetBits;

            var valueOffset = (int)((rowInfo.Offset is not null ? rowInfo.Offset * 8 : 0) + columnOffset - _stringCorrection);

            try
            {
                var output = columnInfo.ArrayLength > 0
                ? JsonSerializer.Serialize(ExtractMany(columnInfo.Type, columnInfo.Size, columnInfo.IsSigned, valueOffset, columnInfo.ArrayLength).ToArray())
                : ExtractSingle(columnInfo.Type, columnInfo.Size, columnInfo.IsSigned, valueOffset);

                if(output is string text && columnInfo.Type == Db2ValueTypes.Text)
                    _stringCorrection += fieldStorageInfo.FieldSizeBits - ((text.Length + 1) * 8);

                return output;
            }
            catch
            {
                return "ERROR";
            }
        }

        public void NextRow(RowInfo rowInfo) => _stringCorrection = 0;

        private IEnumerable<object> ExtractMany(Db2ValueTypes type, int size, bool isSigned, int valueOffset, int count)
        {
            for(int i = 0; i < count; i++)
            {
                var output = ExtractSingle(type, size, isSigned, valueOffset + (size * i));

                if(output is float dOut)
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
            (Db2ValueTypes.Text, _, _) => ReadString(valueOffset / 8),
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

        private string ReadString(int offset) => new string(_recordData
                                                    .Skip(offset)
                                                    .TakeWhile(x => x != 0)
                                                    .Select(x => Convert.ToChar(x))
                                                    .ToArray());
    }
}