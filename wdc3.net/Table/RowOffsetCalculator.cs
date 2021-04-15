using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wdc3.net.File;

namespace wdc3.net.Table
{
    internal class RowOffsetCalculator
    {
        public static IEnumerable<RowOffset> Calculate(IEnumerable<SectionWithFlag> sections)
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

                for(int offsetMapIndex = 0; offsetMapIndex < offsetMaps.Length; offsetMapIndex++)
                {
                    yield return new RowOffset()
                    {
                        Offset = (offsetMaps[offsetMapIndex].Offset - start - sectionSizeCorrection) ?? 0,
                        RowId = ids[offsetMapIndex]
                    };
                }

                sectionSizeCorrection += (uint)section.SizeOfWithoutVariableRecordData;
            }
        }
    }
}