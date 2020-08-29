namespace wdc3.net.Reader
{
    public interface IFileReader<T>
    {
        long Position { get; }

        T Read();
    }
}