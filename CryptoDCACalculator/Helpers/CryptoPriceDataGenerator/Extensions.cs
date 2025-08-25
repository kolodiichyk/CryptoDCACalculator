namespace CryptoDCACalculator.Helpers.CryptoPriceDataGenerator;

public static class Extensions
{
    public static double NextGaussian(this Random random, double mean = 0, double stdDev = 1)
    {
        // Box-Muller transform
        var u1 = 1.0 - random.NextDouble(); // Uniform(0,1] random doubles
        var u2 = 1.0 - random.NextDouble();
        var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        return mean + stdDev * randStdNormal;
    }
}
