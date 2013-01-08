#region License GNU GPL

// SubMapTransition.cs
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

using System.IO;
using BiM.Behaviors.Game.Actors.RolePlay;

namespace BiM.Behaviors.Game.World.MapTraveling.Transitions
{
    public abstract class SubMapTransition
    {
        #region Delegates

        public delegate void TransitionEndedHandler(SubMapTransition transition, SubMap from, SubMapBinder to, bool success);

        #endregion

        public abstract bool BeginTransition(SubMap from, SubMapBinder to, PlayedCharacter character);

        public event TransitionEndedHandler TransitionEnded;

        public virtual void OnTransitionEnded(SubMap from, SubMapBinder to, bool success)
        {
            TransitionEndedHandler evnt = TransitionEnded;
            if (evnt != null)
                evnt(this, from, to, success);
        }

        public abstract void Serialize(BinaryWriter writer);
        public abstract void Deserialize(BinaryReader reader);
    }
}