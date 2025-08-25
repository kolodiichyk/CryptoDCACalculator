namespace CryptoDCACalculator.Helpers.CryptoPriceDataGenerator;

internal class CryptoHistoricalData
{
    public decimal StartPrice { get; set; }
    public Dictionary<DateTime, decimal> KeyPrices { get; set; } = new();
    public double Volatility { get; set; } // Daily volatility as a percentage (0.05 = 5%)
}
