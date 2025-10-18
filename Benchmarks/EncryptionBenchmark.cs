using CryptoBenchmarks.Algorithms.Interfaces;
using CryptoBenchmarks.Models;
using CryptoBenchmarks.Utils;
using System.Diagnostics;
using System.Security.Cryptography;

namespace CryptoBenchmarks.Benchmarks
{
    public static class EncryptionBenchmark
    {
        private static readonly int[] DataSizes = { 128, 512, 2048, 8192, 32768, 1_048_576, 4_194_304, 16_777_216 };

        public static List<BenchmarkResult> Run(IEncryptionAlgorithm algo)
        {
            var results = new List<BenchmarkResult>();
            algo.GenerateKey();

            foreach (var size in DataSizes)
            {
                if (algo.Name.StartsWith("RSA") && size > 300) continue;

                var data = RandomNumberGenerator.GetBytes(size);
                var encryptTimes = new List<double>();
                var decryptTimes = new List<double>();

                for (int i = 0; i < 32; i++) // 30 + 2 warm-up
                {
                    if (i < 2) { algo.Encrypt(data); continue; }

                    var sw = Stopwatch.StartNew();
                    var enc = algo.Encrypt(data);
                    sw.Stop();
                    encryptTimes.Add(sw.Elapsed.TotalMilliseconds);

                    sw.Restart();
                    algo.Decrypt(enc);
                    sw.Stop();
                    decryptTimes.Add(sw.Elapsed.TotalMilliseconds);
                }

                results.Add(new BenchmarkResult
                {
                    Algorithm = algo.Name,
                    KeySize = algo.KeySize,
                    DataSize = size,
                    EncryptMean = encryptTimes.Average(),
                    EncryptMedian = Statistics.Median(encryptTimes),
                    EncryptP95 = Statistics.Percentile(encryptTimes, 95),
                    DecryptMean = decryptTimes.Average(),
                    DecryptMedian = Statistics.Median(decryptTimes),
                    DecryptP95 = Statistics.Percentile(decryptTimes, 95)
                });
            }
            return results;
        }
    }
}
