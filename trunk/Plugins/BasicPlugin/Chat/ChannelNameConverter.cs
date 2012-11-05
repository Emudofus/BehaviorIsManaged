using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using BiM.Behaviors.Data;
using BiM.Protocol.Data;
using BiM.Protocol.Enums;

namespace BasicPlugin.Chat
{
    public class ChannelNameConverter : IValueConverter
    {
        private Dictionary<ChatActivableChannelsEnum, string> m_channelsNames = new Dictionary<ChatActivableChannelsEnum, string>();

        public ChannelNameConverter()
        {
            var channels = Enum.GetValues(typeof(ChatActivableChannelsEnum));
            foreach (ChatActivableChannelsEnum channel in channels)
            {
                var data = DataProvider.Instance.Get<ChatChannel>((int)channel);
                m_channelsNames.Add(channel, DataProvider.Instance.Get<string>(data.nameId));
            }
        }

        public string GetChannelName(ChatActivableChannelsEnum channel)
        {
            if (!m_channelsNames.ContainsKey(channel))
                return "?";

            return m_channelsNames[channel];
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var channel = (ChatActivableChannelsEnum)value;

            return GetChannelName(channel);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }
}