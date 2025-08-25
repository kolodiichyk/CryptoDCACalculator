using CommunityToolkit.Mvvm.ComponentModel;
using CryptoDCACalculator.Helpers;
using CryptoDCACalculator.Models;
using CryptoDCACalculator.ViewModels.Bases;
using Microcharts;

namespace CryptoDCACalculator.ViewModels;

public partial class InvestmentDetailsChartViewModel : BaseViewModel, IQueryAttributable
{
    [ObservableProperty]
    private InvestmentModel _investment;

    [ObservableProperty]
    private LineChart _chart;

    public void ApplyQueryAttributes(IDictionary<string, object> parameters)
    {
        if (parameters == null)
            return;

        if (parameters.TryGetValue(nameof(Investment), out InvestmentModel investment) && investment != null)
        {
            Investment = investment;
            TryInvoke(CreateLineChart);
        }
    }

    private void CreateLineChart()
    {
        Chart = new LineChart();
        var chartEntry = new List<ChartEntry>();
        decimal prevPrice = 0;

        foreach (var calculation in Investment.InvestmentCalculations)
        {
            chartEntry.Add(new ChartEntry((float)calculation.PriceAtDate)
            {
                Color = prevPrice > calculation.PriceAtDate
                    ? SkiaSharp.SKColor.Parse("#FF1943")
                    : SkiaSharp.SKColor.Parse("#19FF43")
            });
            prevPrice = calculation.PriceAtDate;
        }

        Chart.LineSize = 2;
        Chart.Entries = chartEntry;
        Chart.LegendOption = SeriesLegendOption.None;
        Chart.EnableYFadeOutGradient = false;
        OnPropertyChanged(nameof(Chart));
    }
}
