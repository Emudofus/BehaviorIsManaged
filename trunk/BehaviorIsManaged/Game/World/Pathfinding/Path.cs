using System;

namespace BiM.Game.World.Pathfinding
{
    public class Path
    {
        public ObjectPosition Start
        {
            get;
            private set;
        }

        public ObjectPosition End
        {
            get;
            private set;
        }

        public Cell[] GetPathCells()
        {
            throw new NotImplementedException();
        }
    }
}