namespace CryptoDCACalculator.Models;

public class InvestmentModel
{
    public int Id { get; set; }
    public CryptoCurrencyModel CryptoCurrency { get; set; }
    public decimal MonthlyAmount { get; set; }
    public DateTime StartDate { get; set; }
    public int InvestmentDay { get; set; }
    public decimal TotalInvested { get; set; }
    public decimal TotalCoinAmount { get; set; }
    public decimal Profit => TotalCoinAmount * CryptoCurrency.CurrentPrice - TotalInvested;
    public decimal TotalValueToday { get; set; }
    public decimal ROI => Math.Round((TotalCoinAmount * CryptoCurrency.CurrentPrice - TotalInvested)/ TotalInvested * 100, 2);
    public bool IsActive { get; set; }

    public List<CalculationResult> InvestmentCalculations { get; set; } = new();
}
