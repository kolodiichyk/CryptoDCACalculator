using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CryptoDCACalculator.EF;
using CryptoDCACalculator.EF.Entities;
using CryptoDCACalculator.Helpers.Messages;
using CryptoDCACalculator.Models;
using CryptoDCACalculator.Services;
using CryptoDCACalculator.ViewModels.Bases;
using CryptoDCACalculator.Views;
using Microsoft.EntityFrameworkCore;

namespace CryptoDCACalculator.ViewModels;

public partial class MainViewModel(
    IDbContextFactory<AppDbContext> contextFactory,
    IDataUpdateService dataUpdateService,
    ICalculatorService calculatorService,
    IMessenger messenger) : BaseViewModel
{
    [ObservableProperty] private ObservableCollection<CryptoCurrencyModel> _cryptoCurrencies = new();

    [ObservableProperty] private ObservableCollection<InvestmentModel> _investments = new();

    [ObservableProperty] private InvestmentModel? _selectedInvestment;

    [ObservableProperty] private decimal _totalInvested;

    [ObservableProperty] private decimal _totalValueToday;

    [ObservableProperty] private decimal _totalROI;

    [ObservableProperty] private LoadingState  _loadingCryptoCurrenciesState;
    [ObservableProperty] private LoadingState  _loadingInvestmentsState;

    [RelayCommand]
    private async Task OnInvestmentSelectedAsync()
    {
        if (SelectedInvestment == null)
            return;

        await Shell.Current.GoToAsync($"{nameof(InvestmentDetailsPage)}",
            new Dictionary<string, object> { { nameof(InvestmentDetailsViewModel.Investment), SelectedInvestment } });
        ;
        SelectedInvestment = null;
    }

    [RelayCommand]
    private async Task OnNewInvestmentAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(InvestmentPage)}");
    }

    [RelayCommand]
    private async Task ShowPortfolioAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(PortfolioPage)}",
            new Dictionary<string, object>
            {
                { nameof(Investments), Investments },
                { nameof(TotalInvested), TotalInvested },
                { nameof(TotalValueToday), TotalValueToday },
                { nameof(TotalROI), TotalROI },
            });
    }

    public override void Initialize()
    {
        base.Initialize();
        messenger.Register<InvestmentUpdatedEvent>(this, OnInvestmentUpdated);
    }

    private void OnInvestmentUpdated(object recipient, InvestmentUpdatedEvent message)
    {
        Investments = new ObservableCollection<InvestmentModel>();
        Task.Run(LoadInvestments);
    }

    public override void OnAppearing()
    {
        base.OnAppearing();

        if (CryptoCurrencies.Any())
            return;

        Task.Run(() => TryInvokeAsync(LoadCryptoCurrencies,
            () => { LoadingCryptoCurrenciesState = LoadingState.Finished; }));
        Task.Run(() => TryInvokeAsync(LoadInvestments,
            () => { LoadingInvestmentsState = LoadingState.Finished; }));
    }

    private async Task LoadInvestments()
    {
        LoadingInvestmentsState = LoadingState.Loading;
        await using var appDbContext = await contextFactory.CreateDbContextAsync();
        var investments = await appDbContext.Investments
            .Include(t => t.CryptoCurrency)
            .ToListAsync();
        var investmentModels = new ObservableCollection<InvestmentModel>();

        foreach (var investment in investments)
        {
            var calculationResults = await calculatorService.CalculateDCAAsync(investment.ToModel());

            investment.InvestmentCalculations.Clear();
            investment.InvestmentCalculations.AddRange(
                calculationResults.Select(t => new InvestmentCalculation
                {
                    InvestmentId = investment.Id,
                    Date = t.Date,
                    TotalInvested = t.TotalInvested,
                    TotalCoinAmount = t.TotalCoinAmount,
                    ROI = t.ROI,
                    PriceAtDate = t.PriceAtDate,
                    CoinAmount = t.CoinAmount,
                    InvestedAmount = t.InvestedAmount
                }));

            investment.TotalInvested = calculationResults.Last().TotalInvested;
            investment.TotalCoinAmount = calculationResults.Last().TotalCoinAmount;

            var investmentModel = investment.ToModel();
            investmentModel.InvestmentCalculations = calculationResults;
            investmentModel.CryptoCurrency.CurrentPrice = calculationResults.Last().PriceAtDate;
            investmentModel.TotalValueToday = calculationResults.Last().TotalValueToday;

            TotalInvested += investment.TotalInvested;
            TotalValueToday += investmentModel.TotalValueToday;

            investmentModels.Add(investmentModel);
        }

        await appDbContext.SaveChangesAsync();

        MainThread.BeginInvokeOnMainThread(() =>
            {
                Investments = new ObservableCollection<InvestmentModel>(investmentModels);
                if (TotalInvested != 0)
                    TotalROI = Math.Round((TotalValueToday - TotalInvested) / TotalInvested, 2);

                OnPropertyChanged(nameof(Investments));
            }
        );
    }

    private async Task LoadCryptoCurrencies()
    {
        LoadingCryptoCurrenciesState = LoadingState.Loading;
        await using var appDbContext = await contextFactory.CreateDbContextAsync();
        var cryptoCurrencies = await appDbContext.CryptoCurrencies.ToListAsync();
        foreach (var cryptoCurrency in cryptoCurrencies)
        {
            if (await dataUpdateService.NeedsPriceHistoryUpdateAsync(cryptoCurrency.Symbol, DateTime.UtcNow,
                    DateTime.UtcNow))
                await dataUpdateService.UpdatePriceHistoryAsync(cryptoCurrency.Symbol, DateTime.UtcNow,
                    DateTime.UtcNow);

            var model = cryptoCurrency.ToModel();
            model.CurrentPrice = (await appDbContext.PriceHistory
                                     .FirstOrDefaultAsync(
                                         t => t.CryptoCurrency.Symbol == cryptoCurrency.Symbol &&
                                              t.Date == DateTime.UtcNow.Date))?.Price ??
                                 0;
            MainThread.BeginInvokeOnMainThread(() => CryptoCurrencies.Add(model));
        }

    }
}
