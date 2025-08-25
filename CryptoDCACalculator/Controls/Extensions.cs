using CryptoDCACalculator.Controls.ExtendedEntry;

namespace CryptoDCACalculator.Controls;

public static class Extensions
{
    public static MauiAppBuilder UseCustomControls(this MauiAppBuilder builder)
    {
        builder.ConfigureMauiHandlers(handlers =>
        {
            handlers.AddHandler<ExtendedEntry.ExtendedEntry, ExtendedEntryHandler>();
        });

        return builder;
    }
}
