namespace CryptoDCACalculator.Services;

public interface IDataUpdateService
{
    Task<bool> NeedsPriceHistoryUpdateAsync(string cryptoSymbol, DateTime startDate, DateTime endDate);
    Task UpdatePriceHistoryAsync(string cryptoSymbol, DateTime startDate, DateTime endDate);
}
