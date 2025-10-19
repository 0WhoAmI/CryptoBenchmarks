namespace CryptoBenchmarks.Models
{
    public class EncryptionBenchmarkResult
    {
        public string Algorithm { get; set; } = string.Empty;
        public int KeySize { get; set; }
        public int MessageSizeBytes { get; set; }
        public double MeanEncryptMs { get; set; }
        public double MedianEncryptMs { get; set; }
        public double P95EncryptMs { get; set; }
        public double MeanDecryptMs { get; set; }
        public double MedianDecryptMs { get; set; }
        public double P95DecryptMs { get; set; }
    }
}
