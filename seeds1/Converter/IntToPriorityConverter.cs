using System.Globalization;

namespace seeds1.Converter;

public class IntToPriorityConverter : IValueConverter, IMarkupExtension
{
    public object MediumPriority { get; set; }
    public object MaximumPriority { get; set; }
    public object MinimumPriority { get; set; }
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value != null && value is int catPreference)
        {
            if (catPreference == -1) return MinimumPriority;
            else if (catPreference == 1) return MaximumPriority;
            else return MediumPriority;
        }
        else
        {
            return MediumPriority;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}
