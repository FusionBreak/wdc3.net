using System;
using System.Collections.Generic;
using System.Linq;
using wdc3.net.File;

namespace wdc3.net.Table
{
    internal class RowInfoReader
    {
        //public static IEnumerable<RowInfo> ReadRowInfos(IEnumerable<ISection> sections) => sections switch
        //{
        //    IEnumerable<Section> normals => ReadRowInfos(normals),
        //    IEnumerable<SectionWithFlag> flaggeds => ReadRowInfos(flaggeds),
        //    _ => throw new Exception(),
        //};

        public static IEnumerable<RowInfo> ReadRowInfos(IEnumerable<ISection> sections, bool withOffsets)
                => withOffsets
                ? ReadRowInfos(sections.Select(section => (Section)section))
                : ReadRowInfos(sections.Select(section => (SectionWithFlag)section));

        public static IEnumerable<RowInfo> ReadRowInfos(IEnumerable<SectionWithFlag> sections)
        {
            var start = sections.First().OffsetMap?.First().Offset;
            uint sectionSizeCorrection = 0;

            foreach(var section in sections)
            {
                if(section.OffsetMap is null)
                    throw new Exception();

                if(section.IdList is null)
                    throw new Exception();

                var offsetMaps = section.OffsetMap.ToArray();
                var ids = section.IdList.ToArray();
                var mapIds = section.OffsetMapIdList?.ToArray();

                for(var offsetMapIndex = 0; offsetMapIndex < offsetMaps.Length; offsetMapIndex++)
                {
                    yield return new RowInfo()
                    {
                        Offset = (offsetMaps[offsetMapIndex].Offset - start - sectionSizeCorrection) ?? 0,
                        Id = ids[offsetMapIndex],
                        Size = offsetMaps[offsetMapIndex].Size,
                        OffsetMapId = mapIds?[offsetMapIndex]
                    };
                }

                sectionSizeCorrection += (uint)section.SizeOfWithoutVariableRecordData;
            }
        }

        public static IEnumerable<RowInfo> ReadRowInfos(IEnumerable<Section> sections)
        {
            foreach(var section in sections)
            {
                for(var index = 0; index < section.IdList?.Count(); index++)
                {
                    var id = section.IdList.Skip(index).First();
                    var offsetMap = section.OffsetMap?.Skip(index).First();
                    var offsetMapId = section.OffsetMapIdList?.Skip(index).First();

                    yield return new RowInfo()
                    {
                        Id = id,
                        Offset = offsetMap?.Offset,
                        Size = offsetMap?.Size,
                        OffsetMapId = offsetMapId
                    };
                }
            }
        }
    }
}