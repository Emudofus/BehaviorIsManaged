#region License GNU GPL
// WaypointAction.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
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

namespace BiM.Behaviors.Profiles
{
    public abstract class ProfileAction
    {
        public delegate void ActionFinishedDelegate(ProfileAction action, bool success);
        public event ActionFinishedDelegate Finished;

        public event Action<ProfileAction> Started;
        public event Action<ProfileAction> Paused;
        public event Action<ProfileAction> Resumed;


        public bool IsRunning
        {
            get;
            private set;
        }

        public bool IsPaused
        {
            get;
            private set;
        }

        /// <summary>
        /// True if successed, false if not. Null if not ended
        /// </summary>
        public bool? Successed
        {
            get;
            private set;
        }

        public PlayedCharacter Character
        {
            get;
            private set;
        }

        public void Start(PlayedCharacter character)
        {
            if (IsPaused)
            {
                Resume();
                return;
            }

            if (IsRunning)
                return;

            IsRunning = true;
            Character = character;

            OnStart();
        }

        public void Pause()
        {
            if (IsPaused || !IsRunning)
                return;

            IsRunning = false;
            IsPaused = true;

            OnPause();
        }

        public void Resume()
        {
            if (!IsPaused)
                return;

            IsPaused = false;
            IsRunning = true;

            OnResume();
        }

        public void Stop()
        {
            if (!IsRunning && !IsPaused)
                return;

            IsRunning = false;
            IsPaused = false;

            OnStop(true);
        }

        protected virtual void OnStart()
        {
            var evnt = Started;
            if (evnt != null)
                evnt(this);
        }

        protected virtual void OnStop(bool successed)
        {
            Successed = successed;
            var evnt = Finished;
            if (evnt != null)
                evnt(this, successed);
        }

        protected virtual void OnPause()
        {
            var evnt = Paused;
            if (evnt != null)
                evnt(this);
        }

        protected virtual void OnResume()
        {
            var evnt = Resumed;
            if (evnt != null)
                evnt(this);
        }

        public abstract bool CanRun(PlayedCharacter character);
    }
}