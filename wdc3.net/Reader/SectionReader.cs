using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using wdc3.net.File;

namespace wdc3.net.Reader
{
    public class SectionReader : IFileReader<IEnumerable<ISection>>
    {
        public long Position { get; private set; }
        BinaryReader _reader;
        IEnumerable<SectionHeader> _sectionHeaders;
        ushort _headerFlag;
        uint _recordSize;

        public SectionReader(BinaryReader reader, IEnumerable<SectionHeader> sectionHeaders, ushort headerFlag, uint recordSize)
        {
            _reader = reader;
            _sectionHeaders = sectionHeaders;
            _headerFlag = headerFlag;
            _recordSize = recordSize;
        }

        public IEnumerable<ISection> Read()
        {
            List<ISection> output = new List<ISection>();

            foreach(var sectionHeader in _sectionHeaders)
            {
                if((_headerFlag & 1) == 0)
                    output.Add(ReadSection(sectionHeader.RecordCount, sectionHeader.StringTableSize));
                else
                    output.Add(ReadSectionWithFlag(sectionHeader.OffsetRecordsEnd - sectionHeader.FileOffset));
            }

            Position = _reader.BaseStream.Position;
            return output;
        }

        public SectionWithFlag ReadSectionWithFlag(uint variableRecordDataLength)
        {
            var output = new SectionWithFlag();

            var variableRecordData = new List<byte>();
            for(int currentByte = 0; currentByte < variableRecordDataLength; currentByte++)
                variableRecordData.Add(_reader.ReadByte());

            output.VariableRecordData = variableRecordData;

            return output;
        }

        public Section ReadSection(uint recordCount, uint stringTableSize)
        {
            var output = new Section();

            var records = new List<RecordData>();
            for(int currentRecord = 0; currentRecord < recordCount; currentRecord++)
            {
                var data = new List<byte>();
                for(int currentData = 0; currentData < _recordSize; currentData++)
                    data.Add(_reader.ReadByte());

                records.Add(new RecordData() { Data = data });
            }
            output.Records = records;

            var stringData = new List<byte>();
            for(int i = 0; i < stringTableSize; i++)
                stringData.Add(_reader.ReadByte());
            output.StringData = stringData;

            return output;
        }
    }
}
