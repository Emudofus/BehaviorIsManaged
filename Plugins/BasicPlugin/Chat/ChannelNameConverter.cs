#region License GNU GPL
// ChannelNameConverter.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using BiM.Behaviors.Data;
using BiM.Behaviors.Data.D2O;
using BiM.Behaviors.Data.I18N;
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
                var data = ObjectDataManager.Instance.Get<ChatChannel>((int)channel);
                m_channelsNames.Add(channel, I18NDataManager.Instance.ReadText(data.nameId));
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