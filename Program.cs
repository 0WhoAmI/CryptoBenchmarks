using CryptoBenchmarks.Algorithms;
using CryptoBenchmarks.Algorithms.Interfaces;
using CryptoBenchmarks.Benchmarks;
using CryptoBenchmarks.Models;
using System.Text.Json;

namespace CryptoBenchmarks
{
    public class Program
    {
        static void Main(string[] args)
        {

            /*
             * TODO:
             * - zrobić punkt 1 (porównanie czasu generowania kluczy)
             * 
             */

            var algorithms = new List<IEncryptionAlgorithm>
            {
                new AesGcmAlgorithm(128),
                new AesGcmAlgorithm(256),
                new TripleDesCbcAlgorithm(),
                new RsaOaepAlgorithm(2048),
                new RsaOaepAlgorithm(3072)
            };

            var allResults = new List<BenchmarkResult>();

            foreach (var algorithm in algorithms)
            {
                Console.WriteLine($"Benchmarking {algorithm.Name} ({algorithm.KeySize})...");


                var results = EncryptionBenchmark.Run(algorithm);
                allResults.AddRange(results);
            }

            File.WriteAllText("encryption_results.json", JsonSerializer.Serialize(allResults, new JsonSerializerOptions { WriteIndented = true }));

            Console.WriteLine("Zapisano do encryption_results.json");
        }
    }
}
