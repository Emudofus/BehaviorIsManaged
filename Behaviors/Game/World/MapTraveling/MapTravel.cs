#region License GNU GPL
// MapTravel.cs
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
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Behaviors.Game.World.MapTraveling.Transitions;

namespace BiM.Behaviors.Game.World.MapTraveling
{
    public class MapTravel
    {        
        #region Delegates

        public delegate void TransitionEndedHandler(MapTravel travel, SubMapTransition transition, bool success);

        #endregion

        public event TransitionEndedHandler TransitionEnded;

        private void OnTransitionEnded(SubMapTransition transition, bool success)
        {
            TransitionEndedHandler handler = TransitionEnded;
            if (handler != null) handler(this, transition, success);
        }


        public MapTravel(SubMapTransition[] transitions, SubMapBinder[] subMaps)
        {
            Transitions = transitions;
            SubMaps = subMaps;

            if (Transitions.Length != SubMaps.Length)
                throw new ArgumentException("Transitions.Length != SubMaps.Length");
        }

        public SubMapBinder[] SubMaps
        {
            get;
            private set;
        }

        public SubMapTransition[] Transitions
        {
            get;
            private set;
        }

        public int Index
        {
            get;
            set;
        }

        public SubMapTransition CurrentTransition
        {
            get { return Index < 0 || Index >= Transitions.Length ? null : Transitions[Index]; }
        }

        public SubMapBinder CurrentSubMap
        {
            get
            {
                return Index < 0 || Index >= SubMaps.Length ? null : SubMaps[Index];
            }
        }

        public bool IsEnded()
        {
            return Index >= Transitions.Length - 1;
        }

        public bool HasFailed()
        {
            return Index == -1;
        }

        public bool BeginTransition(PlayedCharacter character)
        {
            if (IsEnded())
                return true;

            CurrentTransition.TransitionEnded += CurrentTransitionOnTransitionEnded;
            return CurrentTransition.BeginTransition(character.SubMap, SubMaps[Index + 1], character);
        }

        private void CurrentTransitionOnTransitionEnded(SubMapTransition transition, SubMap @from, SubMapBinder to, bool success)
        {
            CurrentTransition.TransitionEnded -= CurrentTransitionOnTransitionEnded;

            if (!success)
                Index = -1;
            else
            {
                Index++;
            }

            OnTransitionEnded(transition, success);
        }
    }
}