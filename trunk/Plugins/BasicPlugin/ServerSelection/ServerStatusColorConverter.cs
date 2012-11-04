using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using BiM.Protocol.Enums;

namespace BasicPlugin.ServerSelection
{
    public class ServerStatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (ServerStatusEnum)value;
            switch (status)
            {
                case ServerStatusEnum.ONLINE:
                    return new SolidColorBrush(Colors.Green);
                case ServerStatusEnum.NOJOIN:
                case ServerStatusEnum.OFFLINE:
                    return new SolidColorBrush(Colors.Red);
                case ServerStatusEnum.FULL:
                    return new SolidColorBrush(Colors.OrangeRed);
                case ServerStatusEnum.STARTING:
                    return new SolidColorBrush(Colors.LightGreen);
                case ServerStatusEnum.STOPING:
                    return new SolidColorBrush(Colors.LightSalmon);
                case ServerStatusEnum.SAVING:
                    return new SolidColorBrush(Colors.CornflowerBlue);
                default:
                    return new SolidColorBrush(Colors.DarkGray);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}