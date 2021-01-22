using System.Collections.Generic;
using System.IO;
using wdc3.net.File;

namespace wdc3.net.Reader
{
    public class SectionReader : IFileReader<IEnumerable<ISection>>
    {
        public long Position { get; private set; }
        private readonly BinaryReader _reader;
        private readonly IEnumerable<SectionHeader> _sectionHeaders;
        private readonly ushort _headerFlag;
        private readonly uint _recordSize;

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
                var section = (_headerFlag & 1) == 0
                    ? readSection(sectionHeader.RecordCount, sectionHeader.StringTableSize)
                    : (ISection)readSectionWithFlag(sectionHeader.OffsetRecordsEnd - sectionHeader.FileOffset);

                var idList = new List<uint>();
                for(var i = 0; i < sectionHeader.IdListSize / 4; i++)
                    idList.Add(_reader.ReadUInt32());
                section.IdList = idList;

                if(sectionHeader.CopyTableCount > 0)
                {
                    var copyTableEntrys = new List<CopyTableEntry>();
                    for(int copyTableIndex = 0; copyTableIndex < sectionHeader.CopyTableCount; copyTableIndex++)
                    {
                        copyTableEntrys.Add(new CopyTableEntry
                        {
                            IdOfNewRow = _reader.ReadUInt32(),
                            IdOfCopiedRow = _reader.ReadUInt32()
                        });
                    }
                    section.CopyTable = copyTableEntrys;
                }

                if(sectionHeader.OffsetMapIdCount > 0)
                {
                    var offsetMapEntrys = new List<OffsetMapEntry>();
                    for(int offsetMapIndex = 0; offsetMapIndex < sectionHeader.OffsetMapIdCount; offsetMapIndex++)
                    {
                        offsetMapEntrys.Add(new OffsetMapEntry
                        {
                            Offset = _reader.ReadUInt32(),
                            Size = _reader.ReadUInt16()
                        });
                    }
                    section.OffsetMap = offsetMapEntrys;
                }

                if(sectionHeader.RelationsshipDataSize > 0)
                {
                    var relationshipMap = new RelationshipMapping
                    {
                        NumEntries = _reader.ReadUInt32(),
                        MinId = _reader.ReadUInt32(),
                        MaxId = _reader.ReadUInt32()
                    };
                    section.RelationshipMap = relationshipMap;
                }

                if(sectionHeader.OffsetMapIdCount > 0)
                {
                    var offsetMapIdList = new List<uint>();
                    for(var i = 0; i < sectionHeader.OffsetMapIdCount; i++)
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
            for(var currentByte = 0; currentByte < variableRecordDataLength; currentByte++)
                variableRecordData.Add(_reader.ReadByte());

            output.VariableRecordData = variableRecordData;

            return output;
        }

        private Section readSection(uint recordCount, uint stringTableSize)
        {
            var output = new Section();

            var records = new List<RecordData>();
            for(var currentRecord = 0; currentRecord < recordCount; currentRecord++)
            {
                var data = new List<byte>();
                for(var currentData = 0; currentData < _recordSize; currentData++)
                    data.Add(_reader.ReadByte());

                records.Add(new RecordData() { Data = data });
            }
            output.Records = records;

            var stringData = new List<byte>();
            for(var i = 0; i < stringTableSize; i++)
                stringData.Add(_reader.ReadByte());
            output.StringData = stringData;

            return output;
        }
    }
}