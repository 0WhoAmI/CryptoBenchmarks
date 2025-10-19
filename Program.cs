using CryptoBenchmarks.Algorithms;
using CryptoBenchmarks.Benchmarks;
using CryptoBenchmarks.Interfaces;

namespace CryptoBenchmarks
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // aby poprawnie wyświetlać polskie litery

            var algorithms = new List<IEncryptionAlgorithm>
            {
                new RsaOaepAlgorithm(2048),
                new RsaOaepAlgorithm(3072),
                new AesGcmAlgorithm(128),
                new AesGcmAlgorithm(256),
                new TripleDesCbcAlgorithm() // W domyśle .NET  keySize = 168 bit
            };

            await KeyGenBenchmark.RunAsync(algorithms);
            await EncryptionBenchmark.RunAsync(algorithms);
        }
    }
}
