namespace BiM.Behaviors.Game.World.Pathfinding
{
    public abstract class CellInformationProvider
    {
        public abstract Map Map
        {
            get;
        }

        public abstract bool IsCellWalkable(Cell cell, bool fight = false, Cell previousCell = null);
        public abstract CellInformation GetCellInformation(Cell cell);
    }
}