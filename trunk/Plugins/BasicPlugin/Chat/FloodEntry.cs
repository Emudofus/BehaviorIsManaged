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
    }
}