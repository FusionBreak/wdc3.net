﻿using System.Collections.Generic;

namespace wdc3.net.File
{
    public class VersionDefinition
    {
        public IEnumerable<BuildInfo>? Builds { get; set; }
        public IEnumerable<BuildRange>? BuildRanges { get; set; }
        public IEnumerable<string>? LayoutHashes { get; set; }
        public string? Comment { get; set; }
        public IEnumerable<DefinitionInfo>? Definitions { get; set; }
    }
}