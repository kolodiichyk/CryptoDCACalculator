using CommunityToolkit.Maui;
using CryptoDCACalculator.Controls;
using CryptoDCACalculator.EF;
using CryptoDCACalculator.Services;
using CryptoDCACalculator.ViewModels;
using CryptoDCACalculator.Views;
using Microcharts.Maui;

namespace CryptoDCACalculator;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseCustomControls()
            .UseMicrocharts()
            .UseEF("CryptoDCACalculator.db")
            .RegisterServices()
            .RegisterViewModels()
            .RegisterViews()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Montserrat-Regular.ttf", "MontserratRegular");
                fonts.AddFont("Montserrat-SemiBold.ttf", "MontserratSemibold");
                fonts.AddFont("AlegreyaSans-Regular.ttf", "AlegreyaSansRegular");
            });

        return builder.Build();
    }
}
