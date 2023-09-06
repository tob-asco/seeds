using System.Globalization;

namespace seeds1.Converter;

public class TagNameOfFamilyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || !targetType.IsAssignableFrom(typeof(string)))
        {
            return "convert error";
        }

        // cut off the family descriptor ("lang: French" -> "French")
        var parts = value.ToString().Split(':');
        return parts.Count() > 1 ?
            string.Join("", parts.Skip(1)).Trim() : value.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
