using System.Collections.ObjectModel;
using CryptoDCACalculator.EF;
using CryptoDCACalculator.Models;
using CryptoDCACalculator.Services;
using Microcharts;
using Microsoft.EntityFrameworkCore;

namespace CryptoDCACalculator;

public partial class MainPage2 : ContentPage
{
    public ObservableCollection<CalculationGroupedResult> GroupedResults { get; set; }

    public MainPage2()
    {
        InitializeComponent();
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        List<CalculationResult> results = new List<CalculationResult>();

        var dbContext = App.Current.Handler.MauiContext.Services.GetService<AppDbContext>();
        var calculatorService = App.Current.Handler.MauiContext.Services.GetService<ICalculatorService>();

        var investments = await dbContext.Investments
            .Include(t=>t.CryptoCurrency).ToListAsync();

        foreach (var investment in investments.ToModel())
        {
            var result = await calculatorService.CalculateDCAAsync(investment);
            results.AddRange(result);
        }

        Init(results);
        ResultCollectionView.ItemsSource = GroupedResults;

        var chart = new LineChart();
        var chartEntry = new List<ChartEntry>();
        var coins = results.Select(r => r.CoinSymbol).Distinct().Skip(2).Take(1).ToList();

        foreach (var coin in coins)
        {
            var cryptoCurrency = await dbContext.CryptoCurrencies.FirstAsync(t=>t.Symbol== coin);
            var i = 0;
            foreach (var priceHistory in cryptoCurrency.PriceHistory.ToList())
            {
                if (i ++ % 7 != 0 && i != 1)
                    continue;

                chartEntry.Add(new ChartEntry((float)priceHistory.Price) { Label = priceHistory.Date.ToString("MM/dd/yyyy"), Color = SkiaSharp.SKColor.Parse("#FF1943") });
            }
        }

        chart.Entries = chartEntry;
        ChartView.Chart = chart;
    }

    public void Init(IEnumerable<CalculationResult> results)
    {
        var coins = results.Select(r => r.CoinSymbol).Distinct().ToList();

        var grouped = results
            .GroupBy(r => r.Date.Date)
            .OrderBy(g => g.Key)
            .Select(g =>
            {
                // усі записи, які реально були
                var dayResults = g.ToList();

                // додаємо нульові для відсутніх монет
                var withZeros = coins
                    .Select(symbol =>
                    {
                        var existing = dayResults.FirstOrDefault(x => x.CoinSymbol == symbol);
                        if (existing != null)
                            return existing;

                        return new CalculationResult
                        {
                            Date = g.Key,
                            CoinSymbol = symbol,
                            InvestedAmount = 0,
                            CoinAmount = 0,
                            ValueToday = 0,
                            ROI = 0,
                            TotalInvested = dayResults.Sum(x => x.TotalInvested), // можеш залишити 0 або підраховувати окремо
                            TotalCoinAmount = 0,
                            TotalValueToday = dayResults.Sum(x => x.TotalValueToday)
                        };
                    })
                    .ToList();

                return new CalculationGroupedResult(g.Key, withZeros);
            })
            .ToList();

        GroupedResults = new ObservableCollection<CalculationGroupedResult>(grouped);
    }
}
