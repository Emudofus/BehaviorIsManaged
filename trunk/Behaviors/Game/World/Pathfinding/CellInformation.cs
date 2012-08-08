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