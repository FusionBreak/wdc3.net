using System.Collections.Generic;
using System.Linq;

namespace wdc3.net.Reader
{
    internal class DataChunk
    {
        public string? Name { get; set; }
        public IEnumerable<string>? Parameters { get; set; }
        public bool HasParameters => Parameters != null && Parameters.Any();
        public IEnumerable<string>? Content { get; set; }
        public bool HasContent => Content != null && Content.Any();
    }
}