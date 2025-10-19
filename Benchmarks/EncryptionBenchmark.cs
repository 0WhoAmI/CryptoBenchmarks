using CryptoBenchmarks.Interfaces;
using CryptoBenchmarks.Models;
using CryptoBenchmarks.Utils;
using System.Diagnostics;
using System.Security.Cryptography;

namespace CryptoBenchmarks.Benchmarks
{
    public static class EncryptionBenchmark
    {
        private static readonly int[] MessageSizes = { 128, 512, 2048, 8192, 32768, 1_048_576, 4_194_304, 16_777_216 };

        public static async Task RunAsync(List<IEncryptionAlgorithm> algorithms)
        {
            var results = new List<EncryptionBenchmarkResult>();
            var writer = new CsvResultWriter<EncryptionBenchmarkResult>(
                "Algorithm,KeySize,MessageSizeBytes,MeanEnc,MedianEnc,P95Enc,MeanDec,MedianDec,P95Dec",
                r => $"{r.Algorithm},{r.KeySize},{r.MessageSizeBytes}," +
                     $"{r.MeanEncryptMs:F6},{r.MedianEncryptMs:F6},{r.P95EncryptMs:F6}," + // :F6 - 6 miejsc po przecinku
                     $"{r.MeanDecryptMs:F6},{r.MedianDecryptMs:F6},{r.P95DecryptMs:F6}"
            );

            Console.WriteLine("\n=== Benchmark: Szyfrowanie / Deszyfrowanie ===");

            foreach (IEncryptionAlgorithm algo in algorithms)
            {
                Console.WriteLine($"\n--- {algo.Name} ({algo.KeySize} bitów) ---");
                algo.GenerateKey();

                foreach (int size in MessageSizes)
                {
                    if (algo.Name.StartsWith("RSA") && size > 300)
                    {
                        // RSA nie jest przeznaczone do szyfrowania dużych bloków - zwykle szyfrujemy tylko mały klucz symetryczny i potem używamy AES (hybrydowo
                        continue;
                    }

                    byte[] data = GenerateRandomBytes(size);
                    EncryptionBenchmarkResult result = MeasureEncryptDecrypt(algo, data);
                    results.Add(result);
                    Print(result);
                }
            }

            await writer.WriteAsync("encryption_results.csv", results);
        }

        private static EncryptionBenchmarkResult MeasureEncryptDecrypt(IEncryptionAlgorithm algo, byte[] data)
        {
            const int Warmup = 2; // Pomijamy pierwsze wywołania, żeby JIT (Just-In-Time compiler - kompilator, który konwertuje kod C# (MSIL) na kod maszynowy podczas uruchamiania programu) i inicjalizacja nie psuły wyników
            const int Runs = 30;

            double[] encTimes = new double[Runs];
            double[] decTimes = new double[Runs];

            for (int i = 0; i < Runs + Warmup; i++)
            {
                Stopwatch sw = Stopwatch.StartNew();
                byte[] ciphertext = algo.Encrypt(data);
                sw.Stop();
                if (i >= Warmup)
                {
                    encTimes[i - Warmup] = sw.Elapsed.TotalMilliseconds;
                }

                sw.Restart();
                byte[] plaintext = algo.Decrypt(ciphertext);
                sw.Stop();
                if (i >= Warmup)
                {
                    decTimes[i - Warmup] = sw.Elapsed.TotalMilliseconds;
                }
            }

            return new EncryptionBenchmarkResult
            {
                Algorithm = algo.Name,
                KeySize = algo.KeySize,
                MessageSizeBytes = data.Length,
                MeanEncryptMs = StatisticsHelper.Mean(encTimes),
                MedianEncryptMs = StatisticsHelper.Median(encTimes),
                P95EncryptMs = StatisticsHelper.Percentile(encTimes, 95),
                MeanDecryptMs = StatisticsHelper.Mean(decTimes),
                MedianDecryptMs = StatisticsHelper.Median(decTimes),
                P95DecryptMs = StatisticsHelper.Percentile(decTimes, 95)
            };
        }

        private static byte[] GenerateRandomBytes(int size)
        {
            byte[] buffer = new byte[size];
            RandomNumberGenerator.Fill(buffer);
            return buffer;
        }

        private static void Print(EncryptionBenchmarkResult r)
        {
            Console.WriteLine($"{r.Algorithm} ({r.KeySize} bitów, {r.MessageSizeBytes} B) → " +
                              $"Enc={r.MeanEncryptMs:F3} ms, Dec={r.MeanDecryptMs:F3} ms"); // :F3 - 3 miejsca po przecink
        }
    }
}
