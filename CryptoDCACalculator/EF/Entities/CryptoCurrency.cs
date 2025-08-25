using System.ComponentModel.DataAnnotations;

namespace CryptoDCACalculator.EF.Entities;

public class CryptoCurrency
{
    [Key]
    public int Id { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public string Image { get; set; }

    public List<PriceHistory> PriceHistory { get; set; } = new();
}
