using BiM.Behaviors.Game.Actors;

namespace BiM.Behaviors.Game.World.Pathfinding
{
    public interface IMapDataProvider : IContext
    {
        CellList Cells
        {
            get;
        }

        // todo, replace with a method that return the marks
        bool CellMarked(Cell cell);
    }
}