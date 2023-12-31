﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seeds1.Converter;

public class CatBtnTextConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values == null || values.Length < 3 || !targetType.IsAssignableFrom(typeof(string)))
        {
            return "convert error";
            // Alternatively, return BindableProperty.UnsetValue to use the binding FallbackValue
        }

        return "" + values[0] + values[1] + values[2];
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        //need only OneWayBinding
        throw new NotImplementedException();
    }
}
