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

            foreach(var section in sections)
            {
                foreach(var offsetMap in section.OffsetMap ?? throw new Exception())
                {
                    if(offsetMap is null)
                        throw new Exception();

                    var offset = offsetMap.Offset;
                }
            }

            yield return null;
        }
    }
}