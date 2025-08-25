namespace CryptoDCACalculator.ViewModels;

public static class Extensions
{
    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<InvestmentDetailsViewModel>();
        builder.Services.AddTransient<InvestmentViewModel>();
        builder.Services.AddTransient<InvestmentDetailsChartViewModel>();
        builder.Services.AddTransient<PortfolioViewMode>();

        return builder;
    }
}
