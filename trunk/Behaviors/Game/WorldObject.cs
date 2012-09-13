using System;
using System.ComponentModel;
using BiM.Behaviors.Game.World;

namespace BiM.Behaviors.Game
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

        public virtual void Tick(int dt)
        {

        }

        public virtual void Dispose()
        {
            PropertyChanged = null;
        }

    }
}