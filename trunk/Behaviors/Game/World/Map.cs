namespace BiM.Behaviors.Game.World
{
    public class Map : IContext
    {
        public const uint Width = 14;
        public const uint Height = 20;

        public const uint MapSize = Width * Height * 2;

        public Cell[] Cells
        {
            get;
            set;
        }
    }
}