#region License GNU GPL
// CellData.cs
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

using BiM.Protocol.Tools.Dlm;
using ProtoBuf;

namespace BiM.Behaviors.Game.World.Data
{
    [ProtoContract]
    public class CellData
    {
        public CellData()
        {
            
        }

        public CellData(DlmCellData cell)
        {
            Id = cell.Id;
            Floor = cell.Floor;
            LosMov = cell.LosMov;
            Speed = cell.Speed;
            MapChangeData = cell.MapChangeData;
            MoveZone = cell.MoveZone;
        }

        [ProtoMember(1)]
        public short Id
        {
            get;
            set;
        }

        [ProtoMember(2)]
        public short Floor
        {
            get;
            set;
        }
        [ProtoMember(3)]
        public byte LosMov
        {
            get;
            set;
        }

        [ProtoMember(4)]
        public byte Speed
        {
            get;
            set;
        }

        [ProtoMember(5)]
        public byte MapChangeData
        {
            get;
            set;
        }
        [ProtoMember(6)]
        public byte MoveZone
        {
            get;
            set;
        }
    }
}