using System.ComponentModel.DataAnnotations;

namespace CryptoDCACalculator.EF.Entities;

public class InvestmentCalculation
{
    [Key]
    public int Id { get; set; }
    public int InvestmentId { get; set; }
    public Investment Investment { get; set; }
    public DateTime Date { get; set; }
    public decimal PriceAtDate { get; set; }
    public decimal InvestedAmount { get; set; }
    public decimal CoinAmount { get; set; }
    public decimal ROI { get; set; }
    public decimal TotalInvested { get; set; }
    public decimal TotalCoinAmount { get; set; }
}
