namespace wdc3.net.File
{
    public class BuildInfo
    {
        public int Expansion { get; set; }
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Build { get; set; }

        public override bool Equals(object? obj)
            => obj is BuildInfo build
                && build != null
                && build.Expansion == Expansion
                && build.Major == Major
                && build.Minor == Minor
                && build.Build == Build;
    }
}