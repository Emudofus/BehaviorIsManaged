using BiM.Behaviors.Game.World;
using BiM.Protocol.Enums;

namespace BiM.Behaviors.Game.Spells.Shapes
{
    public class Single : IShape
    {
        public uint Surface
        {
            get
            {
                return 1;
            }
        }

        public byte MinRadius
        {
            get
            {
                return 1;
            }
            set { }
        }

        public DirectionsEnum Direction
        {
            get
            {
                return DirectionsEnum.DIRECTION_NORTH; 
            }
            set { }
        }

        public byte Radius
        {
            get { return 1; }
            set {  }
        }

        public Cell[] GetCells(Cell centerCell, Map map)
        {
            return new [] {centerCell};
        }
    }
}