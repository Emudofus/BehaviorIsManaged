using BiM.Behaviors.Game.Actors;

namespace BiM.Behaviors.Game.World.Pathfinding
{
    /// <summary>
    /// Provide informations on data that relies the context and the map (i.g if an actor is on a cell)
    /// </summary>
    public interface IMapDataProvider : IContext
    {
        bool IsActor(Cell cell);

        // todo, replace with a method that return the marks
        bool IsCellMarked(Cell cell);
        object[] GetMarks(Cell cell);
            
        bool IsCellWalkable(Cell cell, bool throughEntities = false, Cell previousCell = null);
    }
}