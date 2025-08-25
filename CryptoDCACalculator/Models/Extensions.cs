using CryptoDCACalculator.EF.Entities;

namespace CryptoDCACalculator.Models;

public static class Extensions
{
    public static PriceHistoryModel ToModel(this PriceHistory entity)
    {
        return new PriceHistoryModel
        {
            CryptoCurrencySymbol = entity.CryptoCurrency.Symbol,
            Date = entity.Date,
            Price = entity.Price
        };
    }

    public static IEnumerable<PriceHistoryModel> ToModel(this IEnumerable<PriceHistory> entities)
    {
        return entities.Select(t => t.ToModel());
    }

    public static CryptoCurrencyModel ToModel(this CryptoCurrency entity)
    {
        return new CryptoCurrencyModel
        {
            Id = entity.Id,
            Symbol = entity.Symbol,
            Name = entity.Name,
            Image = entity.Image,
            PriceHistory = entity.PriceHistory.ToModel()
        };
    }

    public static IEnumerable<CryptoCurrencyModel> ToModel(this IEnumerable<CryptoCurrency> entities)
    {
        return entities.Select(t => t.ToModel());
    }

    public static InvestmentModel ToModel(this Investment entity)
    {
        return new InvestmentModel
        {
            Id = entity.Id,
            MonthlyAmount = entity.MonthlyAmount,
            InvestmentDay = entity.InvestmentDay,
            StartDate = entity.StartDate,
            IsActive = entity.IsActive,
            TotalInvested = entity.TotalInvested,
            TotalCoinAmount = entity.TotalCoinAmount,
            CryptoCurrency = entity.CryptoCurrency.ToModel()
        };
    }

    public static IEnumerable<InvestmentModel> ToModel(this IEnumerable<Investment> entities)
    {
        return entities.Select(t => t.ToModel());
    }
}
