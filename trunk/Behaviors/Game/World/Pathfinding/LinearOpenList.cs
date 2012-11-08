#region License GNU GPL
// LinearOpenList.cs
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
using BiM.Core.Collections;

namespace BiM.Behaviors.Game.World.Pathfinding
{
    public class LinearOpenList : IOpenList
    {
        private readonly IComparer<Cell> m_comparer;
        List<Cell> m_cells = new List<Cell>(); 

        public LinearOpenList(IComparer<Cell> comparer)
        {
            m_comparer = comparer;
        }

        public void Push(Cell cell)
        {
            m_cells.Add(cell);
        }

        public Cell Pop()
        {
            if (m_cells.Count == 0)
                throw new InvalidOperationException("LinearOpenList is empty");

            Cell bestCell = m_cells[0];
            for (int i = 1; i < m_cells.Count; i++)
            {
                // bestCell has a greater cost than the other cell
                if (m_comparer.Compare(bestCell, m_cells[i]) >= 0)
                    bestCell = m_cells[i];
            }

            m_cells.Remove(bestCell);

            return bestCell;
        }

        public int Count
        {
            get { return m_cells.Count; }
        }
    }
}