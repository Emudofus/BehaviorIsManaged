#region License GNU GPL
// DlmCellData.cs
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
using BiM.Core.IO;

namespace BiM.Protocol.Tools.Dlm
{
    public struct DlmCellData
    {
        public DlmCellData(short id)
        {
            Id = id;
            LosMov = 3;
            m_rawFloor = 0;
            m_floor = 0;
            Speed = 0;
            MapChangeData = 0;
            MoveZone = 0;
            _arrow = 0;
        }

        public short Id;

        public short Floor
        {
            get { return m_floor ?? (m_floor = (short) (m_rawFloor*10)).Value; }
        }

        private sbyte m_rawFloor;
        private short? m_floor;
        private byte? _arrow;


        public byte LosMov;

        public byte Speed;

        public byte MapChangeData;
        public byte MoveZone;
        public bool Los
        {
            get { return (LosMov & 2) >> 1 == 1; }
        }

        public bool Mov
        {
            get { return (LosMov & 1) == 1 && !NonWalkableDuringFight && !FarmCell; }
        }

        public bool NonWalkableDuringFight
        {
            get { return (LosMov & 4) >> 2 == 1; }
        }

        public bool Red
        {
            get { return (LosMov & 8) >> 3 == 1; }
        }

        public bool Blue
        {
            get { return (LosMov & 16) >> 4 == 1; }
        }

        public bool FarmCell
        {
            get { return (LosMov & 32) >> 5 == 1; }
        }

        public bool Visible
        {
            get { return (LosMov & 64) >> 6 == 1; }
        }

        public bool useTopArrow
        {
            get { return (this._arrow & 1) != 0; }
        }// end function

        public bool useBottomArrow
        {
            get { return (this._arrow & 2) != 0; }
        }// end function

        public bool useRightArrow
        {
            get { return (this._arrow & 4) != 0; }
        }// end function

        public bool useLeftArrow
        {
            get { return (this._arrow & 8) != 0; }
        }// end function



        public static DlmCellData ReadFromStream(short id, byte version, IDataReader reader)
        {
            var cell = new DlmCellData(id);

            cell.m_rawFloor = reader.ReadSByte();

            if (cell.m_rawFloor == -128)
            {
                return cell;
            }


            cell.LosMov = reader.ReadByte();
            cell.Speed = reader.ReadByte();
            cell.MapChangeData = reader.ReadByte();

            if (version > 5)
            {
                cell.MoveZone = reader.ReadByte();
            }

            if (version > 7)
            {
                byte tmpBits = reader.ReadByte();
                cell._arrow = (byte)( 15 & tmpBits);
            }

            return cell;
        }
    }
}