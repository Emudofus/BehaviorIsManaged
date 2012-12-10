#region License GNU GPL

// Area.cs
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
using BiM.Behaviors.Data;
using BiM.Behaviors.Data.D2O;
using BiM.Behaviors.Data.I18N;
using BiM.Protocol.Data;

namespace BiM.Behaviors.Game.World.Areas
{
    public class Area
    {
        private readonly Protocol.Data.Area m_area;

        public Area(int id)
        {
            m_area = ObjectDataManager.Instance.Get<Protocol.Data.Area>(id);
            SuperArea = new SuperArea(SuperAreaId);
        }

        public int Id
        {
            get { return m_area.id; }
        }

        public uint NameId
        {
            get { return m_area.nameId; }
        }

        public string Name
        {
            get { return I18NDataManager.Instance.ReadText(NameId); }
        }

        public SuperArea SuperArea
        {
            get;
            private set;
        }

        public int SuperAreaId
        {
            get { return m_area.superAreaId; }
        }

        public bool ContainHouses
        {
            get { return m_area.containHouses; }
        }

        public Boolean ContainPaddocks
        {
            get { return m_area.containPaddocks; }
        }

        public Rectangle Bounds
        {
            get { return m_area.bounds; }
        }
    }
}