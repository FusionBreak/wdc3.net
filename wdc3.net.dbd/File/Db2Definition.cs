using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wdc3.net.dbd.Factorys;

namespace wdc3.net.dbd.File
{
    public class Db2Definition
    {
        public IEnumerable<ColumnDefinition>? ColumnDefinitions { get; set; }
        public IEnumerable<VersionDefinition>? VersionDefinitions { get; set; }
        public string? Comment { get; set; }

        public VersionDefinition GetVersionDefinition(string buildString)
            => VersionDefinitions
                .Where(version => version.Builds.Contains(new BuildInfoFactory().CreateFromBuildString(buildString)))
                .First();
    }
}
