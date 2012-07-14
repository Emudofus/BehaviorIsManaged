using System;
using System.ComponentModel;

namespace BiM.Behaviors.Game.Guilds
{
    public class GuildInformations : INotifyPropertyChanged
    {
        public GuildInformations()
        {
            
        }

        public GuildInformations(Protocol.Types.GuildInformations guild)
        {
            if (guild == null) throw new ArgumentNullException("guild");

            Id = guild.guildId;
            Name = guild.guildName;
            Emblem = new GuildEmblem(guild.guildEmblem);
        }

        public int Id
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public GuildEmblem Emblem
        {
            get;
            private set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}