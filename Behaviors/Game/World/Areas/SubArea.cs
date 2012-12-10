#region License GNU GPL

// SubArea.cs
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
using BiM.Behaviors.Data;
using BiM.Behaviors.Data.D2O;
using BiM.Behaviors.Data.I18N;
using BiM.Protocol.Data;
using BiM.Protocol.Enums;

namespace BiM.Behaviors.Game.World.Areas
{
    public class SubArea
    {
        private readonly Protocol.Data.SubArea m_subArea;

        public SubArea(int id)
        {
            m_subArea = ObjectDataManager.Instance.Get<Protocol.Data.SubArea>(id);
            Area = new Area(AreaId);
        }

        public int Id
        {
            get { return m_subArea.id; }
        }

        public uint NameId
        {
            get { return m_subArea.nameId; }
        }

        public string Name
        {
            get { return I18NDataManager.Instance.ReadText(NameId); }
        }

        public int AreaId
        {
            get { return m_subArea.areaId; }
        }

        public Area Area
        {
            get;
            private set;
        }

        public AlignmentSideEnum AlignmentSide
        {
            get;
            set;
        }

        public List<AmbientSound> AmbientSounds
        {
            get { return m_subArea.ambientSounds; }
        }

        public List<uint> MapIds
        {
            get { return m_subArea.mapIds; }
        }

        public Rectangle Bounds
        {
            get { return m_subArea.bounds; }
        }

        public List<int> Shape
        {
            get { return m_subArea.shape; }
        }

        public List<uint> CustomWorldMap
        {
            get { return m_subArea.customWorldMap; }
        }

        public int PackId
        {
            get { return m_subArea.packId; }
        }
    }
}