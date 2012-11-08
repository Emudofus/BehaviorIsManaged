#region License GNU GPL
// GuildEmblem.cs
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

namespace BiM.Behaviors.Game.Guilds
{
    public class GuildEmblem : INotifyPropertyChanged
    {
        public GuildEmblem()
        {
            
        }

        public GuildEmblem(Protocol.Types.GuildEmblem emblem)
        {
            if (emblem == null) throw new ArgumentNullException("emblem");

            SymbolShape = emblem.symbolShape;
            SymbolColor = emblem.symbolColor;
            BackgroundShape = emblem.backgroundShape;
            BackgroundColor = emblem.backgroundColor;
        }

        public short SymbolShape
        {
            get;
            private set;
        }

        public int SymbolColor
        {
            get;
            private set;
        }

        public short BackgroundShape
        {
            get;
            private set;
        }

        public int BackgroundColor
        {
            get;
            private set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}