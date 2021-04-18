namespace wdc3.net.File
{
    public class BuildInfoFactory
    {
        public static BuildInfo CreateFromBuildString(string buildString)
        {
            var parts = buildString.Split('.');
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