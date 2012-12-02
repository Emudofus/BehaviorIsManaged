#region License GNU GPL

// MapData.cs
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
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Tools.Dlm;
using ProtoBuf;

namespace BiM.Behaviors.Game.World.Data
{
    [ProtoContract]
    public class MapData
    {
        public MapData()
        {
            
        }

        public MapData(DlmMap map)
        {
            Id= map.Id;
            Version = map.Version;
            Encrypted = map.Encrypted;
            EncryptionVersion = map.EncryptionVersion;
            RelativeId = map.RelativeId;
            MapType = map.MapType;
            SubAreaId = map.SubAreaId;
            BottomNeighbourId = map.BottomNeighbourId;
            LeftNeighbourId = map.LeftNeighbourId;
            RightNeighbourId = map.RightNeighbourId;
            TopNeighbourId = map.TopNeighbourId;
            Cells = map.Cells.Select(x => new CellData(x)).ToArray();
        }

        [ProtoMember(1)]
        public int Id
        {
            get;
            set;
        }

        [ProtoMember(2)]
        public byte Version
        {
            get;
            set;
        }

        [ProtoMember(3)]
        public bool Encrypted
        {
            get;
            set;
        }

        [ProtoMember(4)]
        public byte EncryptionVersion
        {
            get;
            set;
        }

        [ProtoMember(5)]
        public uint RelativeId
        {
            get;
            set;
        }

        [ProtoMember(6)]
        public byte MapType
        {
            get;
            set;
        }

        [ProtoMember(7)]
        public int SubAreaId
        {
            get;
            set;
        }

        [ProtoMember(8)]
        public int BottomNeighbourId
        {
            get;
            set;
        }

        [ProtoMember(9)]
        public int TopNeighbourId
        {
            get;
            set;
        }

        [ProtoMember(10)]
        public int LeftNeighbourId
        {
            get;
            set;
        }

        [ProtoMember(11)]
        public int RightNeighbourId
        {
            get;
            set;
        }

        [ProtoMember(12)]
        public bool UsingNewMovementSystem
        {
            get;
            set;
        }

        [ProtoMember(14)]
        public CellData[] Cells
        {
            get;
            set;
        }
    }
}