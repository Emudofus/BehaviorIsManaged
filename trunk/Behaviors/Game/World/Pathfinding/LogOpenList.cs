using System.Collections.Generic;
using BiM.Core.Collections;

namespace BiM.Behaviors.Game.World.Pathfinding
{
    public class LogOpenList : PriorityQueueB<Cell>, IOpenList
    {
        public LogOpenList(IComparer<Cell> comparer)
            : base(comparer)
        {
            
        }

        void IOpenList.Push(Cell cell)
        {
            Push(cell);
        }
    }
}