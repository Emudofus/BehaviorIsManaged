using BiM.Protocol.Enums;

namespace BiM.Behaviors.Game.World.Pathfinding
{
    public class PathElement
    {
        public PathElement(Cell cell, DirectionsEnum direction)
        {
            Cell = cell;
            Direction = direction;
        }

        public Cell Cell { get; set; }
        public DirectionsEnum Direction { get; set; }
    }
}