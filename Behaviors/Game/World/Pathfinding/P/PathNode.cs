#region License GNU GPL
// PathNode.cs
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

using BiM.Behaviors.Game.World.Data;

namespace BiM.Behaviors.Game.World.Pathfinding.P
{
    public class PathNode
    {
        public const int DEFAULT_DISTANCE = 999;
        public const int MAX_WEIGHT = 50;
        public const int DEFAULT_WEIGHT = 5;

        public Cell Cell;
        public int Weight;
        public int DistanceSteps;
        private int m_speed;

        public PathNode(Cell cell)
        {
            Cell = cell;
            //color = DefaultColor;
            IsInPath = false;
            //isInPath2 = false;
            IsWalkable = cell.Walkable;
            IsCombatWalkable = !cell.NonWalkableDuringFight && IsWalkable;
            AllowLOS = cell.LineOfSight;
            Speed = cell.Speed;
            DistanceSteps = DEFAULT_DISTANCE;
            SubMapId = 0;
            MapLink = cell.MapChangeData;
        }

        public int Speed
        {
            get { return m_speed; }
            set
            {
                m_speed = value;
                if (value == 0)
                {
                    Weight = DEFAULT_WEIGHT;
                }
                else
                {
                    if (Speed < 0)
                    {
                        Weight = DEFAULT_WEIGHT*(1 - Speed);
                        if (Weight > MAX_WEIGHT)
                            Weight = MAX_WEIGHT;
                    }
                    else if (Speed > 0)
                    {
                        Weight = DEFAULT_WEIGHT/(1 + Speed);
                        if (Weight < 1) Weight = 1;
                    }
                }
            }
        }

        public int SubMapId;

        public bool IsDiagonal;
        public bool IsInPath;
        public bool IsCloseToEnemy;
        public bool IsWalkable;
        public bool IsCombatWalkable;
        public bool AllowLOS;
        public byte MapLink;

        public int X
        {
            get { return Cell.X; }
        }

        public int Y 
        {
            get { return Cell.Y; }
        }

        public override string ToString()
        {
            if (Cell != null) return Cell.ToString();
            return "<null>";
        }
    }
}