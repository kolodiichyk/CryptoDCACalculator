using Microsoft.Maui.Handlers;

namespace CryptoDCACalculator.Controls.ExtendedEntry;

public partial class ExtendedEntryHandler : EntryHandler
{
    private static readonly PropertyMapper<ExtendedEntry, ExtendedEntryHandler> ExtendedEntryMapper =
        new (Mapper)
        {
            [nameof(ExtendedEntry.HasBorder)] = MapHasBorder,
            [nameof(ExtendedEntry.Padding)] = MapPadding,
            [nameof(ExtendedEntry.CornerRadius)] = MapCornerRadius,
            [nameof(ExtendedEntry.BorderWidth)] = MapBorderWidth,
            [nameof(ExtendedEntry.BorderColor)] = MapBorderColor,
            [nameof(ExtendedEntry.BackgroundColor)] = MapHasBorder,
            [nameof(ExtendedEntry.Height)] = MapHasBorder,
            [nameof(ExtendedEntry.Width)] = MapHasBorder,
            [nameof(ExtendedEntry.TextColor)] = MapHasBorder,
            [nameof(ExtendedEntry.Parent)] = MapHasBorder,
        };

    public ExtendedEntryHandler()
        : base(ExtendedEntryMapper)
    {
    }

    public ExtendedEntryHandler(PropertyMapper mapper = null)
        : base(mapper ?? ExtendedEntryMapper)
    {
    }

    private static void MapBorderColor(ExtendedEntryHandler handler, ExtendedEntry view)
    {
        handler?.SetBorderColor(view);
    }

    private static void MapBorderWidth(ExtendedEntryHandler handler, ExtendedEntry view)
    {
        handler?.SetBorderWidth(view);
    }

    private static void MapCornerRadius(ExtendedEntryHandler handler, ExtendedEntry view)
    {
        handler?.SetCornerRadius(view);
    }

    private static void MapPadding(ExtendedEntryHandler handler, ExtendedEntry view)
    {
        handler?.SetPadding(view);
    }

    private static void MapHasBorder(ExtendedEntryHandler handler, ExtendedEntry view)
    {
        handler?.SetHasBorder(view);
    }
}
