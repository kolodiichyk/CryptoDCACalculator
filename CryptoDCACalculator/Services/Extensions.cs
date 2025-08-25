using CommunityToolkit.Mvvm.Messaging;

namespace CryptoDCACalculator.Services;

public static class Extensions
{
    public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<ICoinApiService, CoinApiService>();
        builder.Services.AddTransient<IDataUpdateService, DataUpdateService>();
        builder.Services.AddTransient<ICalculatorService, CalculatorService>();
        builder.Services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

        return builder;
    }
}
