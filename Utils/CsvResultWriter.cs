using CryptoBenchmarks.Interfaces;
using System.Text;

namespace CryptoBenchmarks.Utils
{
    public class CsvResultWriter<T> : IBenchmarkResultWriter<T>
    {
        private readonly string _header;
        private readonly Func<T, string> _lineFormatter;

        public CsvResultWriter(string header, Func<T, string> lineFormatter)
        {
            _header = header;
            _lineFormatter = lineFormatter;
        }

        public async Task WriteAsync(string path, IEnumerable<T> results)
        {
            var sb = new StringBuilder();
            sb.AppendLine(_header);

            foreach (var r in results)
                sb.AppendLine(_lineFormatter(r));

            await File.WriteAllTextAsync(path, sb.ToString());
            Console.WriteLine($"\nWyniki zapisano do pliku: {path}");
        }
    }
}
