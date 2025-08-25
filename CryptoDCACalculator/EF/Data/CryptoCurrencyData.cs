using System.Collections.Generic;
using CryptoDCACalculator.EF.Entities;

namespace CryptoDCACalculator.EF.Data;

internal static class CryptoCurrencyData
{
    public static IEnumerable<CryptoCurrency> Seed()
    {
        return
        [
            new CryptoCurrency { Id = 1, Symbol = "BTC", Name = "Bitcoin", Image = "bitcoin"},
            new CryptoCurrency { Id = 2, Symbol = "ETH", Name = "Ethereum", Image = "ethereum" },
            new CryptoCurrency { Id = 3, Symbol = "SOL", Name = "Solana", Image = "solana" },
            new CryptoCurrency { Id = 4, Symbol = "XRP", Name = "Ripple", Image = "xrp" },
            new CryptoCurrency { Id = 5, Symbol = "BNB", Name = "BNB", Image = "bnb" },
            new CryptoCurrency { Id = 6, Symbol = "DOGE", Name = "Dogecoin", Image = "dogecoin" },
            new CryptoCurrency { Id = 7, Symbol = "TON", Name = "Toncoin", Image = "toncoin" },
            new CryptoCurrency { Id = 8, Symbol = "TRX", Name = "TRON", Image = "tron" }
        ];
    }
}
