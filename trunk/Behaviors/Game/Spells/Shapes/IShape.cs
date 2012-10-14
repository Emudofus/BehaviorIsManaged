using BiM.Behaviors.Game.World;
using BiM.Protocol.Enums;

namespace BiM.Behaviors.Game.Spells.Shapes
{
    public interface IShape
    {
        uint Surface
        {
            get;
        }

        byte MinRadius
        {
            get;
            set;
        }

        DirectionsEnum Direction
        {
            get;
            set;
        }

        byte Radius
        {
            get;
            set;
        }

        Cell[] GetCells(Cell centerCell, Map map);
    }
}