using System.Collections.Generic;
using System.IO;
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
            var output = new List<ISection>();

            foreach(var sectionHeader in _sectionHeaders)
            {
                ISection section;
                if((_headerFlag & 1) == 0)
                    section = readSection(sectionHeader.RecordCount, sectionHeader.StringTableSize);
                else
                    section = readSectionWithFlag(sectionHeader.OffsetRecordsEnd - sectionHeader.FileOffset);

                var idList = new List<uint>();
                for(int i = 0; i < sectionHeader.IdListSize / 4; i++)
                    idList.Add(_reader.ReadUInt32());
                section.IdList = idList;

                if(sectionHeader.CopyTableCount > 0)
                {
                    var copyTable = new CopyTableEntry();
                    copyTable.IdOfNewRow = _reader.ReadUInt32();
                    copyTable.IdOfCopiedRow = _reader.ReadUInt32();
                    section.CopyTable = copyTable;
                }

                if(sectionHeader.OffsetMapIdCount > 0)
                {
                    var offsetMap = new OffsetMapEntry();
                    offsetMap.Offset = _reader.ReadUInt32();
                    offsetMap.Size = _reader.ReadUInt16();
                    section.OffsetMap = offsetMap;
                }

                if(sectionHeader.RelationsshipDataSize > 0)
                {
                    var relationshipMap = new RelationshipMapping();
                    relationshipMap.NumEntries = _reader.ReadUInt32();
                    relationshipMap.MinId = _reader.ReadUInt32();
                    relationshipMap.MaxId = _reader.ReadUInt32();
                    section.RelationshipMap = relationshipMap;
                }

                if(sectionHeader.OffsetMapIdCount > 0)
                {
                    var offsetMapIdList = new List<uint>();
                    for(int i = 0; i < sectionHeader.OffsetMapIdCount; i++)
                        offsetMapIdList.Add(_reader.ReadUInt32());
                    section.OffsetMapIdList = offsetMapIdList;
                }

                output.Add(section);
            }

            Position = _reader.BaseStream.Position;
            return output;
        }

        private SectionWithFlag readSectionWithFlag(uint variableRecordDataLength)
        {
            var output = new SectionWithFlag();

            var variableRecordData = new List<byte>();
            for(int currentByte = 0; currentByte < variableRecordDataLength; currentByte++)
                variableRecordData.Add(_reader.ReadByte());

            output.VariableRecordData = variableRecordData;

            return output;
        }

        private Section readSection(uint recordCount, uint stringTableSize)
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
