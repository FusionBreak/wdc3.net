using wdc3.net.File;

namespace wdc3.net.Factorys
{
    public class BuildInfoFactory
    {
        public BuildInfo CreateFromBuildString(string buildString)
        {
            string[] parts = buildString.Split('.');
            return new BuildInfo()
            {
                Expansion = int.Parse(parts[0]),
                Major = int.Parse(parts[1]),
                Minor = int.Parse(parts[2]),
                Build = int.Parse(parts[3])
            };
        }
    }
}