#region License GNU GPL
// ProgressionCounter.cs
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

using System.ComponentModel;
using System.Diagnostics;

namespace BiM.Core.UI
{
    public class ProgressionCounter : INotifyPropertyChanged
    {
        public delegate void ProgressionEndedHandler(ProgressionCounter counter);
        public event ProgressionEndedHandler Ended;

        public delegate void ProgressionUpdatedHandler(ProgressionCounter counter);
        public event ProgressionUpdatedHandler Updated;

        public ProgressionCounter(int total)
        {
            Total = total;   
        }

        public int Total
        {
            get;
            private set;
        }

        public int Value
        {
            get;
            private set;
        }

        public string Text
        {
            get;
            private set;
        }

        public bool IsEnded
        {
            get;
            private set;
        }

        public void UpdateValue(int value, string text = null)
        {
            Value = value;
            Text = text; 
            
            var handler = Updated;
            if (handler != null) handler(this);
        }

        public void NotifyEnded()
        {
            IsEnded = true;

            ProgressionEndedHandler handler = Ended;
            if (handler != null) handler(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}