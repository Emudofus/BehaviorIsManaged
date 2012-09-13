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