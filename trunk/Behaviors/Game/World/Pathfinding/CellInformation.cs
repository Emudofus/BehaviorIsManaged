#region License GNU GPL
// CellInformation.cs
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
namespace BiM.Behaviors.Game.World.Pathfinding
{
    public class CellInformation
    {
        public CellInformation(Cell cell, bool walkable)
        {
            Cell = cell;
            Walkable = walkable;
            Cost = 1;
        }

        public CellInformation(Cell cell, bool walkable, bool fighting)
        {
            Cell = cell;
            Walkable = walkable;
            Fighting = fighting;
            Cost = 1;
        }

        public CellInformation(Cell cell, bool walkable, bool fighting, int cost)
        {
            Cell = cell;
            Walkable = walkable;
            Fighting = fighting;
            Cost = cost;
        }

        public Cell Cell
        {
            get;
            set;
        }

        public bool Walkable
        {
            get;
            set;
        }

        public bool Fighting
        {
            get;
            set;
        }

        public int Cost
        {
            get;
            set;
        }
    }
}