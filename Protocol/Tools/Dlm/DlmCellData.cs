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
    public class DlmCellData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public DlmCellData(DlmMap map, short id)
        {
            Map = map;
            Id = id;
            LosMov = 3;
        }

        public DlmMap Map
        {
            get;
            private set;
        }

        public short Id
        {
            get;
            private set;
        }

        public short Floor
        {
            get;
            set;
        }

        public byte LosMov
        {
            get;
            set;
        }

        public byte Speed
        {
            get;
            set;
        }

        public byte MapChangeData
        {
            get;
            set;
        }

        public byte MoveZone
        {
            get;
            set;
        }

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



        public static DlmCellData ReadFromStream(DlmMap map, short id, BigEndianReader reader)
        {
            var cell = new DlmCellData(map, id);

            cell.Floor = (short) (reader.ReadByte()*10);

            if (cell.Floor == -1280)
            {
                return cell;
            }

            cell.LosMov = reader.ReadByte();
            cell.Speed = reader.ReadByte();
            cell.MapChangeData = reader.ReadByte();

            if (map.Version > 5)
            {
                cell.MoveZone = reader.ReadByte();
            }

            return cell;
        }
    }
}