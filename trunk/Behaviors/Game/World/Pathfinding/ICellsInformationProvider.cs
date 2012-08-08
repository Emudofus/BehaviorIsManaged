namespace BiM.Behaviors.Game.World.Pathfinding
{
    public interface ICellsInformationProvider
    {
        Map Map
        {
            get;
        }

        bool IsCellWalkable(Cell cell);
        CellInformation GetCellInformation(Cell cell);
    }
}