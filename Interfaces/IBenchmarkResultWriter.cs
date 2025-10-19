namespace CryptoBenchmarks.Interfaces
{
    public interface IBenchmarkResultWriter<T>
    {
        Task WriteAsync(string path, IEnumerable<T> results);
    }
}
