namespace CryptoBenchmarks.Models
{
    public class BenchmarkResult
    {
        public string Algorithm { get; set; }
        public int KeySize { get; set; }
        public int DataSize { get; set; }
        public double EncryptMean { get; set; }
        public double EncryptMedian { get; set; }
        public double EncryptP95 { get; set; }
        public double DecryptMean { get; set; }
        public double DecryptMedian { get; set; }
        public double DecryptP95 { get; set; }
    }
}
