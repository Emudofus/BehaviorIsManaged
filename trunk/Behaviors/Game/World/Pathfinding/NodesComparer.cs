using System.Collections.Generic;

namespace BiM.Behaviors.Game.World.Pathfinding
{
    public class NodesComparer : IComparer<short>
    {
        private readonly PathNode[] m_matrix;
        private readonly bool m_orderByDescending;

        public NodesComparer(PathNode[] matrix, bool orderByDescending)
        {
            m_matrix = matrix;
            m_orderByDescending = orderByDescending;
        }

        #region IComparer<short> Members

        public int Compare(short a, short b)
        {
            if (m_matrix[a].F > m_matrix[b].F)
            {
                return m_orderByDescending ? -1 : 1;
            }

            if (m_matrix[a].F < m_matrix[b].F)
            {
                return m_orderByDescending ? 1 : - 1;
            }
            return 0;
        }

        #endregion
    }
}