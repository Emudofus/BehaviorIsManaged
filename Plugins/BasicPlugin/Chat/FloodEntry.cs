#region License GNU GPL
// FloodEntry.cs
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using BiM.Protocol.Enums;

namespace BasicPlugin.Chat
{
    [Serializable]
    public class FloodEntry : INotifyPropertyChanged
    {
        public FloodEntry()
        {
            Timer = 10;
            Channels = new FloodedChannel[]
            {
                new FloodedChannel() { Channel = ChatActivableChannelsEnum.CHANNEL_GLOBAL },
                new FloodedChannel() { Channel = ChatActivableChannelsEnum.CHANNEL_TEAM },
                new FloodedChannel() { Channel = ChatActivableChannelsEnum.CHANNEL_GUILD },
                new FloodedChannel() { Channel = ChatActivableChannelsEnum.CHANNEL_ALIGN },
                new FloodedChannel() { Channel = ChatActivableChannelsEnum.CHANNEL_PARTY },
                new FloodedChannel() { Channel = ChatActivableChannelsEnum.CHANNEL_SALES },
                new FloodedChannel() { Channel = ChatActivableChannelsEnum.CHANNEL_SEEK },
                new FloodedChannel() { Channel = ChatActivableChannelsEnum.CHANNEL_NOOB },
                new FloodedChannel() { Channel = ChatActivableChannelsEnum.CHANNEL_ARENA },
                new FloodedChannel() { Channel = ChatActivableChannelsEnum.PSEUDO_CHANNEL_PRIVATE },
            };
        }

        public bool IsEnabled
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public int Timer
        {
            get;
            set;
        }

        public bool UseTimer
        {
            get;
            set;
        }

        public bool OnCharacterEnterMap
        {
            get;
            set;
        }

        public FloodedChannel[] Channels
        {
            get;
            set;
        }

        public DateTime LastSend
        {
            get;
            set;
        }

        public TimeSpan TimeUntilNextSend
        {
            get
            {
                return UseTimer ? DateTime.Now - LastSend - TimeSpan.FromSeconds(Timer) : TimeSpan.Zero;
            }
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
          if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        
    }

    [Serializable]
    public class FloodedChannel : INotifyPropertyChanged
    {
        public ChatActivableChannelsEnum Channel
        {
            get;
            set;
        }

        public bool IsEnabled
        {
            get;
            set;
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
          if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}