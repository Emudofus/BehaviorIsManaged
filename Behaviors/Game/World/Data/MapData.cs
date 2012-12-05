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
using BiM.Protocol.Data;
using BiM.Protocol.Tools.Dlm;
using ProtoBuf;

namespace BiM.Behaviors.Game.World.Data
{
    [ProtoContract]
    public class MapData : IMap
    {
        public MapData()
        {
            
        }

        public MapData(DlmMap map, MapPosition position = null)
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
            Cells = new CellDataList(map.Cells.Select(x => new CellData(x)).ToArray());
            if (position != null)
            {
                X = position.posX;
                Y = position.posY;
                WorldMap = position.worldMap;
                Outdoor = position.outdoor;
            }
            else
            {
                X = ( Id & 0x3FE00 ) >> 9; // 9 higher bits
                Y = Id & 0x01FF; // 9 lower bits
                WorldMap = Id & 0x3FFC0000 >> 18;

                if ((X & 0x100) == 0x100) // 9th bit is the sign. 1 means it's minus
                {
                    X = -( X & 0xFF ); // just take the 8 first bits and take the opposite number
                }
                if (( Y & 0x100 ) == 0x100) // 9th bit is the sign. 1 means it's minus
                {
                    Y = -( Y & 0xFF ); // just take the 8 first bits and take the opposite number
                }
            }
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
        public CellDataList Cells
        {
            get;
            set;
        }

        ICellList<ICell> IMap.Cells
        {
            get { return Cells; }
        }

        [ProtoMember(15)]
        public int X
        {
            get;
            set;
        }

        [ProtoMember(16)]
        public int Y
        {
            get;
            set;
        }

        [ProtoMember(17)]
        public int WorldMap
        {
            get;
            set;
        }

        [ProtoMember(18)]
        public bool Outdoor
        {
            get;
            set;
        }
    }
}