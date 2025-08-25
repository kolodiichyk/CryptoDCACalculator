using System.Globalization;

namespace CryptoDCACalculator.Helpers.Converters;

public class RoiColorConverter : IValueConverter
{
    public Color PositiveColor { get; init; } = (Color)Application.Current.Resources["Green"];

    public Color NegativeColor { get; init; } = (Color)Application.Current.Resources["Red"];

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is decimal roi)
        {
            // Return red color resource for negative ROI, white for positive
            return roi < 0 ? NegativeColor : PositiveColor;
        }

        // Default to white if value is not a valid double
        return PositiveColor;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
