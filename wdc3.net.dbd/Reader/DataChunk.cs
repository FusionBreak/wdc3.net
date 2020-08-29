using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wdc3.net.dbd.Reader
{
    internal class DataChunk
    {
        public string? Name { get; set; }
        public IEnumerable<string>? Parameters { get; set; }
        public bool HasParameters => Parameters.Any();
        public IEnumerable<string>? Content { get; set; }
        public bool HasContent => Content != null && Content.Any();
    }
}
