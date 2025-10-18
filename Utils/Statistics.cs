namespace CryptoBenchmarks.Utils
{
    public static class Statistics
    {
        public static double Median(IEnumerable<double> data)
        {
            var sorted = data.OrderBy(x => x).ToArray();
            int n = sorted.Length;
            return n % 2 == 0 ? (sorted[n / 2 - 1] + sorted[n / 2]) / 2.0 : sorted[n / 2];
        }

        public static double Percentile(IEnumerable<double> data, double p)
        {
            var sorted = data.OrderBy(x => x).ToArray();
            double rank = (p / 100.0) * (sorted.Length - 1);
            int lower = (int)Math.Floor(rank);
            int upper = (int)Math.Ceiling(rank);
            if (lower == upper) return sorted[lower];
            double weight = rank - lower;
            return sorted[lower] * (1 - weight) + sorted[upper] * weight;
        }
    }
}
