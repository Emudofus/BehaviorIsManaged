#region License GNU GPL
// SpellModification.cs
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
using BiM.Protocol.Enums;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Stats
{
    public class SpellModification : INotifyPropertyChanged
    {
        public SpellModification(CharacterSpellModification modification)
        {
            if (modification == null) throw new ArgumentNullException("modification");
            SpellId = modification.spellId;
            ModificationType = (CharacterSpellModificationTypeEnum) modification.modificationType;
            Value = new StatsRow(modification.value);
        }

        public int SpellId
        {
            get;
            set;
        }

        public CharacterSpellModificationTypeEnum ModificationType
        {
            get;
            set;
        }

        public StatsRow Value
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
          if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}