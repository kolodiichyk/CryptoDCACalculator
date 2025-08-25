namespace CryptoDCACalculator.Models;

public class CalculationResult
{
    public DateTime Date { get; set; }
    public decimal PriceAtDate { get; set; }
    public decimal InvestedAmount { get; set; }
    public decimal CoinAmount { get; set; }
    public decimal ValueToday { get; set; }
    public decimal ROI { get; set; }
    public decimal TotalInvested { get; set; }
    public decimal TotalCoinAmount { get; set; }
    public decimal TotalValueToday { get; set; }
    public string CoinSymbol { get; set; } = string.Empty;
}
