using System;
using System.Globalization;
using System.Windows.Data;

namespace BiM.Host.UI.Converters
{
    public class ConstantAdderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var num = System.Convert.ToDecimal(value);
            var num2 = System.Convert.ToDecimal(parameter);

            return num + num2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}