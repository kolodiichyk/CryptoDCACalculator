using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CryptoDCACalculator.Models;
using CryptoDCACalculator.ViewModels.Bases;
using CryptoDCACalculator.Helpers;
using Microcharts;
using SkiaSharp;

namespace CryptoDCACalculator.ViewModels;

public partial class PortfolioViewMode : BaseViewModel, IQueryAttributable
{
    [ObservableProperty]
    private ObservableCollection<InvestmentModel> _investments = new();

    [ObservableProperty]
    private decimal _totalInvested;

    [ObservableProperty]
    private decimal _totalValueToday;

    [ObservableProperty]
    private decimal _totalROI;

    [ObservableProperty]
    private DonutChart _totalInvestmentChart;

    [ObservableProperty]
    private DonutChart _totalValueChart;

    [ObservableProperty]
    private BarChart _profitBarChart;

    private SKColor[] Colors =>
    [
        SKColor.Parse("#ffb3ba"),
        SKColor.Parse("#ffdfba"),
        SKColor.Parse("#ffffba"),
        SKColor.Parse("#baffc9"),
        SKColor.Parse("#bae1ff"),
        SKColor.Parse("#dbdcff"),
        SKColor.Parse("#1b85b8"),
        SKColor.Parse("#c3cb71")
    ];

    public void ApplyQueryAttributes(IDictionary<string, object> parameters)
    {
        if (parameters == null)
            return;

        if (parameters.TryGetValue(nameof(Investments), out ObservableCollection<InvestmentModel> investments) && investments != null)
            Investments = investments;

        if (parameters.TryGetValue(nameof(TotalInvested), out decimal totalInvested))
            TotalInvested = totalInvested;

        if (parameters.TryGetValue(nameof(TotalValueToday), out decimal totalValueToday))
            TotalValueToday = totalValueToday;

        if (parameters.TryGetValue(nameof(TotalROI), out decimal totalROI))
            TotalROI = totalROI;
    }

    public override void OnAppearing()
    {
        base.OnAppearing();
        TryInvoke(CreateTotalInvestmentChart);
    }

    private void CreateTotalInvestmentChart()
    {
        TotalInvestmentChart = new DonutChart();
        TotalValueChart = new DonutChart();
        ProfitBarChart = new BarChart();
        var totalInvestmentChartEntry = new List<ChartEntry>();
        var totalValueChartEntry = new List<ChartEntry>();
        var profitBarChartEntry = new List<ChartEntry>();
        int i = 0;

        var totalsByCryptoName = Investments
            .GroupBy(i => i.CryptoCurrency.Name)
            .Select(g => new
            {
                CryptoCurrencyName = g.Key,
                TotalInvested = g.Sum(i => i.TotalInvested),
                TotalValueToday = g.Sum(i => i.TotalValueToday)
            })
            .ToList();
        foreach (var investment in totalsByCryptoName)
        {
            totalInvestmentChartEntry.Add(new ChartEntry((float)investment.TotalInvested)
            {
                Label = investment.CryptoCurrencyName,
                Color = Colors[i],
                ValueLabel = investment.TotalInvested.ToString("C")
            });
            totalValueChartEntry.Add(new ChartEntry((float)investment.TotalValueToday)
            {
                Label = investment.CryptoCurrencyName,
                Color = Colors[i],
                ValueLabel = investment.TotalValueToday.ToString("C")
            });
            profitBarChartEntry.Add(new ChartEntry((float)investment.TotalValueToday - (float)investment.TotalInvested)
            {
                Label = investment.CryptoCurrencyName,
                Color = Colors[i],
                ValueLabel = investment.TotalValueToday.ToString("C")
            });
            i++;
        }

        TotalInvestmentChart.Entries = totalInvestmentChartEntry;
        TotalValueChart.Entries = totalValueChartEntry;
        ProfitBarChart.Entries = profitBarChartEntry;
        ProfitBarChart.LabelTextSize =
        TotalValueChart.LabelTextSize =
            TotalInvestmentChart.LabelTextSize = 26;
        OnPropertyChanged(nameof(TotalInvestmentChart));
    }
}
