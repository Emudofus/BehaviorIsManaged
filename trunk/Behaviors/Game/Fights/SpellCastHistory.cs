#region License GNU GPL
// SpellCastHistory.cs
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BiM.Behaviors.Game.Actors.Fighters;
using BiM.Core.Collections;

namespace BiM.Behaviors.Game.Fights
{
    public class SpellCastHistory
    {
        private ObservableCollectionMT<SpellCast> m_casts = new ObservableCollectionMT<SpellCast>();
        private ReadOnlyObservableCollectionMT<SpellCast> m_readOnlyCasts;

        public SpellCastHistory(Fighter fighter)
        {
            Fighter = fighter;
            m_readOnlyCasts = new ReadOnlyObservableCollectionMT<SpellCast>(m_casts);
        }

        public Fighter Fighter
        {
            get;
            private set;
        }

        public ReadOnlyObservableCollectionMT<SpellCast> Casts
        {
            get
            {
                return m_readOnlyCasts;
            }
        }

        public void AddSpellCast(SpellCast cast)
        {
            m_casts.Add(cast);
        }
    }
}