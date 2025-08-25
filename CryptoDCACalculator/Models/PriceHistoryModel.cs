using System;

namespace CryptoDCACalculator.Models;

public class PriceHistoryModel
{
    public string CryptoCurrencySymbol{ get; set; }
    public DateTime Date { get; set; }
    public decimal Price { get; set; }
}
