using System;
using System.ComponentModel;
using BiM.Behaviors.Game.World;
using BiM.Protocol.Enums;
using BiM.Protocol.Types;
using NLog;

namespace BiM.Behaviors.Game
{
    public abstract class WorldObject : INotifyPropertyChanged, IDisposable
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public WorldObject()
        {
        }

        public abstract int Id
        {
            get;
            protected set;
        }

        public virtual Cell Cell
        {
            get;
            protected set;
        }

        public virtual DirectionsEnum Direction
        {
            get;
            protected set;
        }

        public virtual Map Map
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


        public void Update(EntityDispositionInformations informations)
        {
            Direction = (DirectionsEnum)informations.direction;
            if (Map == null)
                logger.Error("Cannot define position of {0} with EntityDispositionInformations because Map is null", this);
            else
                Cell = Map.Cells[informations.cellId];
        }
    }
}