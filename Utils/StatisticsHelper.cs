namespace CryptoBenchmarks.Utils
{
    public static class StatisticsHelper
    {
        // Średnia arytmetyczna
        public static double Mean(double[] values) => values.Average();

        public static double Median(double[] values)
        {
            double[] sorted = values.Order().ToArray();
            int n = sorted.Length;

            return n % 2 == 0
                ? (sorted[(n / 2) - 1] + sorted[n / 2]) / 2.0
                : sorted[n / 2];
        }

        public static double Percentile(double[] values, double p)
        {
            double[] sorted = values.Order().ToArray();
            double rank = (p / 100.0) * (sorted.Length - 1);
            int lower = (int)Math.Floor(rank);
            int upper = (int)Math.Ceiling(rank);

            if (lower == upper)
                return sorted[lower];

            double weight = rank - lower;
            return sorted[lower] * (1 - weight) + sorted[upper] * weight;
        }
    }
}
