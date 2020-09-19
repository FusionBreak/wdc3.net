using System;
using System.Collections.Generic;
using System.Linq;
using wdc3.net.Enums;
using wdc3.net.Factorys;
using wdc3.net.File;

namespace wdc3.net.Reader
{
    internal class VersionDefinitionParser
    {
        public static VersionDefinition ParseLayout(DataChunk chunk)
        {
            var output = new VersionDefinition();
            var builds = new List<BuildInfo>();
            var buildRanges = new List<BuildRange>();
            var definitions = new List<DefinitionInfo>();

            if(chunk.Content == null)
                throw new Exception();

            foreach(var row in chunk.Content)
            {
                if(row.StartsWith(DataChunkNames.BUILD) && row.Contains('-'))
                    buildRanges.Add(parseBuildRange(row));
                else if(row.StartsWith(DataChunkNames.BUILD))
                    builds.AddRange(parseBuilds(row));
                else
                    definitions.Add(parseDefinitionInfo(row));
            }

            output.Builds = builds;
            output.BuildRanges = buildRanges;
            output.Definitions = definitions;
            output.LayoutHashes = chunk.Parameters;

            return output;
        }

        public static VersionDefinition ParseBuild(DataChunk chunk)
        {
            var output = new VersionDefinition();
            var builds = new List<BuildInfo>();
            var buildRanges = new List<BuildRange>();
            var definitions = new List<DefinitionInfo>();

            if(chunk.Content == null || chunk.Parameters == null)
                throw new Exception();

            var firstRow = chunk.Name + " ";

            foreach(var para in chunk.Parameters)
                firstRow += para + ", ";

            firstRow = firstRow.TrimEnd(' ').TrimEnd(',');

            var rows = new List<string>
            {
                firstRow
            };
            rows.AddRange(chunk.Content);

            foreach(var row in rows)
            {
                if(row.StartsWith(DataChunkNames.BUILD) && row.Contains('-'))
                    buildRanges.Add(parseBuildRange(row));
                else if(row.StartsWith(DataChunkNames.BUILD))
                    builds.AddRange(parseBuilds(row));
                else
                    definitions.Add(parseDefinitionInfo(row));
            }

            output.Builds = builds;
            output.BuildRanges = buildRanges;
            output.Definitions = definitions;

            return output;
        }

        //BUILD 8.0.1.25902, 8.0.1.25976, 8.0.1.26010, 8.0.1.26032, 8.0.1.26095, 8.0.1.26131, 8.0.1.26141, 8.0.1.26175, 8.0.1.26231
        private static IEnumerable<BuildInfo> parseBuilds(string row)
        {
            var versionStrings = row.Split(' ')[1..].Select(a => a.Replace(",", ""));
            var buildfac = new BuildInfoFactory();

            foreach(var versionString in versionStrings)
            {
                yield return BuildInfoFactory.CreateFromBuildString(versionString);
            }
        }

        //BUILD 7.3.5.25600-7.3.5.25928
        private static BuildRange parseBuildRange(string row)
        {
            var versionsStrings = row.Split(' ')[1].Split('-');

            return new BuildRange()
            {
                MinBuild = BuildInfoFactory.CreateFromBuildString(versionsStrings[0]),
                MaxBuild = BuildInfoFactory.CreateFromBuildString(versionsStrings[1])
            };
        }

        /*
            $noninline,id$ID<32>
            Directory
            Flags<32>[2]
            Corpse[2]
            AreaTableID<u16>
            $noninline,relation$MapID<32>
        */

        private static DefinitionInfo parseDefinitionInfo(string row)
        {
            var output = new DefinitionInfo();

            if(row.StartsWith('$'))
            {
                var parts = row.TrimStart('$').Split('$');
                var parameters = parts[0].Split(',');

                output.IsNonInline = parameters.Contains("noninline");
                output.IsId = parameters.Contains("id");
                output.IsRelation = parameters.Contains("relation");

                row = parts[1];
            }

            if(row.Contains('<'))
            {
                var a = row.Split('<');
                var b = a[1].Split('>');

                output.IsSigned = !b[0].Contains('u');
                output.Size = int.Parse(b[0].Replace("u", ""));

                row = a[0] + b[1];
            }

            if(row.Contains('['))
            {
                var a = row.Split('[');
                var b = a[1].Split(']');

                output.ArrayLength = int.Parse(b[0]);

                row = a[0] + b[1];
            }

            if(row.Contains("//"))
            {
                var a = row.Split("//").Select(text => text.Trim()).ToArray();

                output.Comment = a[1];

                row = a[0];
            }

            output.Name = row;

            return output;
        }
    }
}