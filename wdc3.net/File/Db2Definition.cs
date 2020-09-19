using System;
using System.Collections.Generic;
using System.Linq;
using wdc3.net.Factorys;

namespace wdc3.net.File
{
    public class Db2Definition
    {
        public IEnumerable<ColumnDefinition>? ColumnDefinitions { get; set; }
        public IEnumerable<VersionDefinition>? VersionDefinitions { get; set; }
        public string? Comment { get; set; }

        public VersionDefinition? GetVersionDefinition(string buildString)
            => VersionDefinitions
                ?.Where(version => version.Builds != null && version.Builds.Contains(BuildInfoFactory.CreateFromBuildString(buildString)))
                .First();

        public VersionDefinition? GetVersionDefinition(uint hexLayoutHash)
            => VersionDefinitions
                ?.Where(version => version.LayoutHashes != null && version.LayoutHashes.Contains(hexLayoutHash.ToString("X4")))
                .First();
    }
}