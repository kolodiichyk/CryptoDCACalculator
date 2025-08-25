namespace CryptoDCACalculator.Models;

public class CryptoCurrencyModel
{
    public int Id { get; set; }
    public string Symbol { get; set; }
    public string Name { get; set; }

    public decimal CurrentPrice { get; set; }

    public string Image { get; set; }
    public IEnumerable<PriceHistoryModel> PriceHistory { get; set; }
}
