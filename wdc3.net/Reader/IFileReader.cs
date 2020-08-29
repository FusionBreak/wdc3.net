namespace wdc3.net.Reader
{
    internal interface IFileReader<T>
    {
        long Position { get; }

        T Read();
    }
}