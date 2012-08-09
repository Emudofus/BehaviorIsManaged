namespace BiM.Behaviors.Game.World.Pathfinding
{
    public abstract class CellInformationProvider
    {
        public abstract Map Map
        {
            get;
        }

        public abstract bool IsCellWalkable(Cell cell);
        public abstract CellInformation GetCellInformation(Cell cell);
    }
}