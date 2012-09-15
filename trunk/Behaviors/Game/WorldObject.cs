using System;
using System.ComponentModel;
using BiM.Behaviors.Game.World;
using BiM.Protocol.Enums;

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

        public Cell Cell
        {
            get { return Position != null ? Position.Cell : null; }
        }

        public DirectionsEnum Direction
        {
            get { return Position != null ? Position.Direction : 0; }
        }

        public Map Map
        {
            get { return Position != null ? Position.Map : null; }
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