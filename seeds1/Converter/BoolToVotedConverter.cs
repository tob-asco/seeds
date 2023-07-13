using System.Globalization;

namespace seeds1.Converter;

public class BoolToVotedConverter : IValueConverter, IMarkupExtension
{
    public object NotVoted { get; set; }
    public object Voted { get; set; }
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value != null && value is bool voted)
        {
            if (voted == true) return Voted;
            else return Voted;
        }
        else
        {
            return NotVoted;
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
