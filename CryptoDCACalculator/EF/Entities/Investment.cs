using System;
using System.ComponentModel.DataAnnotations;

namespace CryptoDCACalculator.EF.Entities;

public class Investment
{
    [Key]
    public int Id { get; set; }
    public int CryptoCurrencyId { get; set; }
    public CryptoCurrency CryptoCurrency { get; set; }
    public decimal MonthlyAmount { get; set; }
    public DateTime StartDate { get; set; }
    public int InvestmentDay { get; set; } // Day of month (15th, 20th, etc.)
    public decimal TotalInvested { get; set; }
    public decimal TotalCoinAmount { get; set; }
    public bool IsActive { get; set; } = true;

    public List<InvestmentCalculation> InvestmentCalculations { get; set; } = new();
}
