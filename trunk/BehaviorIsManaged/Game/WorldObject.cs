using System;
using System.Collections.Specialized;
using System.ComponentModel;
using BiM.Game.World;

namespace BiM.Game
{
    public abstract class WorldObject : INotifyPropertyChanged, IDisposable
    {
        public abstract int Id
        {
            get;
            protected set;
        }

        public virtual ObjectPosition Position
        {
            get;
            protected set;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void Dispose()
        {
            PropertyChanged = null;
        }

    }
}