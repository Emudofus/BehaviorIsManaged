#region License GNU GPL
// Spell.cs
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

// Author : FastFrench - antispam@laposte.net
#endregion

using System;
using System.Diagnostics;

namespace BiM.Behaviors.Game.World.Pathfinding.FFPathFinding
{
    public class CellInfo
    {
        public const int MAP_SIZE = 14;
        public const int NB_CELL = 560;
        public const short CELL_ERROR = -1;
        public const int DEFAULT_DISTANCE = 999;
        //public static readonly Color DefaultColor = Color.Green;
        //public static readonly Color PathColor = Color.LightGreen;
        //public static readonly Color ForestColor = Color.Brown;
        public int Weight { get; set; }
        public const int MaxWeight = 50;
        public const int DefaultWeight = 5;
        private int _distanceSteps = DEFAULT_DISTANCE;
        public int DistanceSteps { get { return _distanceSteps; } set { Debug.Assert(value >= 0); _distanceSteps = value; } }
        public byte SubMapId { get; set; } // 0 = undefined
        public Cell Cell { get; set; }
        public bool IsDiagonal { get; set; } // Used to put an higher price on diagonals in PathFinding algorithm 

        public CellInfo()
        {
            X = -1;
            Y = -1;
            CellId = -1;
            //color = Color.Empty;
            IsInPath = false;
            //isInPath2 = false;
            IsWalkable = false;
            IsCombatWalkable = false;
            AllowLOS = false;
            Speed = 1;
            DistanceSteps = DEFAULT_DISTANCE;
            SubMapId = 0;
        }

        public CellInfo(Cell _cell)
        {
            Cell = _cell;
            X = _cell.X;
            Y = _cell.Y;
            _cellid = _cell.Id;
            //color = DefaultColor;
            IsInPath = false;
            //isInPath2 = false;
            IsWalkable = _cell.Walkable;
            IsCombatWalkable = !_cell.NonWalkableDuringFight && IsWalkable;
            AllowLOS = _cell.LineOfSight;
            Speed = _cell.Speed;
            DistanceSteps = DEFAULT_DISTANCE;
            SubMapId = 0;
            MapLink = _cell.MapChangeData;
        }
        //public CellInfo(short cellId, bool isWalkable = true, bool isCombatWalkable = true, bool allowLOS = true, bool drawable = true, int speed = 1)
        //{
        //  this.X = X;
        //  this.Y = Y;
        //  this.CellId = cellId;
        //  //this.color = DefaultColor;
        //  this.isInPath1 = false;
        //  //this.isInPath2 = false;
        //  this.isWalkable = isWalkable;
        //  this.isCombatWalkable = isCombatWalkable;
        //  this.allowLOS = allowLOS;
        //  this.speed = speed;
        //}


        static public short CellIdFromPos(int x, int y)
        {
            int LowPart = (y + (x - y) / 2);
            int HighPart = x - y;
            if (LowPart < 0 || LowPart >= MAP_SIZE) return CELL_ERROR;
            if (HighPart < 0 || HighPart > 39) return CELL_ERROR;
            int result = HighPart * MAP_SIZE + LowPart;
            if (result >= NB_CELL || result < 0) return CELL_ERROR;
            return (short)((x - y) * MAP_SIZE + y + (x - y) / 2);
        }

        public short GetNeighbourCell(int dx, int dy)
        {
            return CellIdFromPos(X + dx, Y + dy);
        }

        public int X { get; set; }
        public int Y { get; set; }

        private short _cellid;
        public short CellId
        {
            get
            {
                return _cellid;
            }
            set
            {
                _cellid = value;
                //(cellId Mod 14) + cellId / 28
                if (value < 0) return;

                //x = (value % 14) + (int)((double)value  / 14.0); // Faux
                Y = (value % 14) - (value - value % 28) / 28; // OK
                X = (short)(0.5 + value / 14.5 + Y * 13.5 / 14.5); // OK.... even if I don't know why

                //Debug.Assert(_cellid == CellIdFromPos(x, y));
                //if (_cellid != CellIdFromPos(x, y))
                //  Debug.Print("CellId:{0} => [{1},{2}] => {3}", _cellid, x, y, CellIdFromPos(x, y));
            }
        }
        //public string label { get; set; }
        public bool IsInPath { get; set; }
        //public bool isInPath2 { get; set; }
        public bool IsCloseToEnemy { get; set; }
        public bool IsWalkable { get; set; }
        public bool IsCombatWalkable { get; set; }
        public bool AllowLOS { get; set; }
        //public Color color { get; set; }
        public byte MapLink { get; set; }
        //public int gfxCount { get; set; }
        //public uint firstGfx { get; set; }
        private int _speed;

        public int Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
                if (value == 0)
                {
                    Weight = DefaultWeight;
                    //color = DefaultColor;
                }
                else
                {
                    if (Speed < 0)
                    {
                        Weight = DefaultWeight * (1 - Speed);
                        //color = ForestColor;
                        if (Weight > MaxWeight)
                            Weight = MaxWeight;
                    }
                    else
                        if (Speed > 0)
                        {
                            //color = PathColor;
                            Weight = DefaultWeight / (1 + Speed);
                            if (Weight < 1) Weight = 1;
                        }
                }
            }
        }


        public enum FieldNames { Nothing, Label, isInPath, isWalkable, isCombatWalkable, allowLOS, /*color, */speed, coordinates, PathFindingInformations, mapLink, cellID/*, gfxCount, firstGfx */};

        static public Array FillCombo()
        {
            return Enum.GetValues(typeof(FieldNames));
        }

        public string getValue(FieldNames whichElement)
        {
            switch (whichElement)
            {
                case FieldNames.Nothing:
                    return "";
                //case FieldNames.Label:
                //  return label;
                case FieldNames.isInPath:
                    return IsInPath.ToString();
                case FieldNames.isWalkable:
                    return IsWalkable.ToString();
                case FieldNames.isCombatWalkable:
                    return IsCombatWalkable.ToString();
                case FieldNames.allowLOS:
                    return AllowLOS.ToString();
                //case FieldNames.color:
                //  return color.ToString();
                case FieldNames.speed:
                    return Speed.ToString();
                case FieldNames.coordinates:
                    return String.Format("{0},{1}", X, Y);
                case FieldNames.PathFindingInformations:
                    return DistanceSteps.ToString();
                case FieldNames.mapLink:
                    return MapLink.ToString();
                case FieldNames.cellID:
                    return this.CellId.ToString();
                //case FieldNames.firstGfx:
                //  return this.firstGfx.ToString();
                //case FieldNames.gfxCount:
                //  return this.gfxCount.ToString();
                default:
                    return "???";
            }

        }
        public override string ToString()
        {
            if (Cell != null) return Cell.ToString();
            return "<null>";
        }
    }
}
