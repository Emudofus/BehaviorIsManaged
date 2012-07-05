using System;
using System.ComponentModel;

namespace BiM.Behaviors.Game.Guilds
{
    public class GuildEmblem : INotifyPropertyChanged
    {
        public GuildEmblem()
        {
            
        }

        public GuildEmblem(Protocol.Types.GuildEmblem emblem)
        {
            if (emblem == null) throw new ArgumentNullException("emblem");

            SymbolShape = emblem.symbolShape;
            SymbolColor = emblem.symbolColor;
            BackgroundShape = emblem.backgroundShape;
            BackgroundColor = emblem.backgroundColor;
        }

        public short SymbolShape
        {
            get;
            private set;
        }

        public int SymbolColor
        {
            get;
            private set;
        }

        public short BackgroundShape
        {
            get;
            private set;
        }

        public int BackgroundColor
        {
            get;
            private set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}