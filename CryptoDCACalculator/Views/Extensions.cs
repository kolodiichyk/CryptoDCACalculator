namespace CryptoDCACalculator.Views;

public static class Extensions
{
    public static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
        Routing.RegisterRoute(nameof(InvestmentDetailsPage), typeof(InvestmentDetailsPage));
        builder.Services.AddTransient<InvestmentDetailsPage>();

        Routing.RegisterRoute(nameof(InvestmentPage), typeof(InvestmentPage));
        builder.Services.AddTransient<InvestmentPage>();

        Routing.RegisterRoute(nameof(InvestmentDetailsChartPage), typeof(InvestmentDetailsChartPage));
        builder.Services.AddTransient<InvestmentDetailsChartPage>();

        Routing.RegisterRoute(nameof(PortfolioPage), typeof(PortfolioPage));
        builder.Services.AddTransient<PortfolioPage>();

        return builder;
    }
}
