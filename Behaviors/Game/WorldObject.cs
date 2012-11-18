#region License GNU GPL
// WorldObject.cs
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
        protected void OnPropertyChanged(string propertyName)
        {
          if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


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