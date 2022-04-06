using System;
using System.Collections.Generic;
using System.Linq;
using wdc3.net.File;

namespace wdc3.net.Writer
{
    internal class SectionSerializer
    {
        public byte[] Serialze(IEnumerable<ISection> sections)
        {
            var output = new List<byte>();

            foreach(var section in sections)
            {
                switch(section)
                {
                    case Section sectionDefault:
                        if(sectionDefault.Records is not null)
                        {
                            foreach(var record in sectionDefault.Records)
                            {
                                if(record is not null && record.Data is not null)
                                    output.AddRange(record.Data.ToArray());
                            }

                            if(sectionDefault.StringData is not null)
                                output.AddRange(sectionDefault.StringData.ToArray());
                        }
                        break;

                    case SectionWithFlag sectionWithFlag:
                        if(sectionWithFlag.VariableRecordData is not null)
                            output.AddRange(sectionWithFlag.VariableRecordData.ToArray());
                        break;
                }

                if(section.IdList is not null)
                {
                    foreach(var id in section.IdList)
                        output.AddRange(BitConverter.GetBytes(id));
                }

                if(section.CopyTable is not null)
                {
                    foreach(var entry in section.CopyTable)
                    {
                        if(entry is not null)
                        {
                            output.AddRange(BitConverter.GetBytes(entry.IdOfNewRow));
                            output.AddRange(BitConverter.GetBytes(entry.IdOfCopiedRow));
                        }
                    }
                }

                if(section.OffsetMap is not null)
                {
                    foreach(var entry in section.OffsetMap)
                    {
                        if(entry is not null)
                        {
                            output.AddRange(BitConverter.GetBytes(entry.Offset));
                            output.AddRange(BitConverter.GetBytes(entry.Size));
                        }
                    }
                }

                if(section.RelationshipMap is not null)
                {
                    output.AddRange(BitConverter.GetBytes(section.RelationshipMap.NumEntries));
                    output.AddRange(BitConverter.GetBytes(section.RelationshipMap.MinId));
                    output.AddRange(BitConverter.GetBytes(section.RelationshipMap.MaxId));
                }

                if(section.OffsetMapIdList is not null)
                {
                    foreach(var id in section.OffsetMapIdList)
                        output.AddRange(BitConverter.GetBytes(id));
                }
            }

            return output.ToArray();
        }
    }
}