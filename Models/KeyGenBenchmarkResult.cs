namespace CryptoBenchmarks.Models
{
    public class KeyGenBenchmarkResult
    {
        public string Algorithm { get; set; } = string.Empty;
        public int KeySize { get; set; }
        public int Count { get; set; }
        public double MeanMs { get; set; }
        public double MedianMs { get; set; }
        public double P95Ms { get; set; }
        public double TotalMs { get; set; }
    }
}
