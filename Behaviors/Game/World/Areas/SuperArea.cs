#region License GNU GPL

// SuperArea.cs
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

using BiM.Behaviors.Data;
using BiM.Behaviors.Data.D2O;
using BiM.Behaviors.Data.I18N;

namespace BiM.Behaviors.Game.World.Areas
{
    public class SuperArea
    {
        private readonly Protocol.Data.SuperArea m_superArea;

        public SuperArea(int id)
        {
            m_superArea = ObjectDataManager.Instance.Get<Protocol.Data.SuperArea>(id);
        }

        public int Id
        {
            get { return m_superArea.id; }
        }

        public uint NameId
        {
            get { return m_superArea.nameId; }
        }

        public string Name
        {
            get { return I18NDataManager.Instance.ReadText(NameId); }
        }

        public uint WorldmapId
        {
            get { return m_superArea.worldmapId; }
        }
    }
}