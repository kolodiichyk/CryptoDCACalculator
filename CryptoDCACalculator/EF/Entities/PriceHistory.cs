using System.ComponentModel.DataAnnotations;

namespace CryptoDCACalculator.EF.Entities;

public class PriceHistory
{
    [Key]
    public int Id { get; set; }
    public int CryptoCurrencyId { get; set; }
    public CryptoCurrency CryptoCurrency{ get; set; }
    public DateTime Date { get; set; }
    public decimal Price { get; set; }
}
