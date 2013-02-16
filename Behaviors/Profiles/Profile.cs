#region License GNU GPL
// Waypoint.cs
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
    public class Profile
    {
        public Profile()
        {
            
        }

        public ProfileAction[] Actions
        {
            get;
            private set;
        }

        public ProfileAction CurrentAction
        {
            get;
            private set;
        }

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

            CurrentAction.Finished += OnCurrentActionFinished;

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

            OnStop();
        }

        public virtual void OnStart()
        {
        }

        public virtual void OnStop()
        {
        }

        public virtual void OnPause()
        {
        }

        public virtual void OnResume()
        {
        }

        private void OnCurrentActionFinished(ProfileAction action, bool success)
        {
            throw new NotImplementedException();
        }
    }
}