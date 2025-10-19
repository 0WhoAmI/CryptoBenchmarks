using CryptoBenchmarks.Interfaces;
using CryptoBenchmarks.Models;
using CryptoBenchmarks.Utils;
using System.Diagnostics;

namespace CryptoBenchmarks.Benchmarks
{
    public static class KeyGenBenchmark
    {
        private static readonly int[] Series = { 1, 10, 100, 1000 };

        public static async Task RunAsync(List<IEncryptionAlgorithm> algorithms)
        {
            var results = new List<KeyGenBenchmarkResult>();
            var writer = new CsvResultWriter<KeyGenBenchmarkResult>(
                "Algorithm,KeySize,Count,MeanMs,MedianMs,P95Ms,TotalMs",
                r => $"{r.Algorithm},{r.KeySize},{r.Count},{r.MeanMs:F6},{r.MedianMs:F6},{r.P95Ms:F6},{r.TotalMs:F6}"
            );

            Console.WriteLine("=== Benchmark: Generowanie kluczy ===");

            foreach (IEncryptionAlgorithm algo in algorithms)
            {
                foreach (int count in Series)
                {
                    double[] times = MeasureKeyGenTimes(algo, count);
                    KeyGenBenchmarkResult result = ComputeStats(algo, count, times);
                    results.Add(result);
                    Print(result);
                }
            }

            await writer.WriteAsync("keygen_results.csv", results);
        }

        private static double[] MeasureKeyGenTimes(IEncryptionAlgorithm algorithm, int count)
        {
            int warmup = 2;
            double[] times = new double[count];
            for (int i = 0; i < count + warmup; i++)
            {
                Stopwatch sw = Stopwatch.StartNew();
                algorithm.GenerateKey();
                sw.Stop();

                if (i >= warmup)
                {
                    times[i - warmup] = sw.Elapsed.TotalMilliseconds;
                }
            }

            return times;
        }

        private static KeyGenBenchmarkResult ComputeStats(IEncryptionAlgorithm algo, int count, double[] times)
        {
            return new KeyGenBenchmarkResult
            {
                Algorithm = algo.Name,
                KeySize = algo.KeySize,
                Count = count,
                MeanMs = StatisticsHelper.Mean(times),
                MedianMs = StatisticsHelper.Median(times),
                P95Ms = StatisticsHelper.Percentile(times, 95),
                TotalMs = times.Sum()
            };
        }

        private static void Print(KeyGenBenchmarkResult r)
        {
            Console.WriteLine($"{r.Algorithm} ({r.KeySize} bitów, {r.Count} kluczy) → " +
                              $"Mean={r.MeanMs:F3} ms | Median={r.MedianMs:F3} ms | P95={r.P95Ms:F3} ms | Total={r.TotalMs:F3} ms");
        }
    }
}
