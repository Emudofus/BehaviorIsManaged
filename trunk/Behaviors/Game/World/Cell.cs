using System;
using System.Drawing;

namespace BiM.Behaviors.Game.World
{
    public class Cell
    {
        public const int StructSize = 2 + 2 + 1 + 1 + 1 + 4;

        private static readonly Point[] s_orthogonalGridReference = new Point[Map.MapSize];
        private static bool m_initialized;

        private static readonly Point s_vectorRight = new Point(1, 1);
        private static readonly Point s_vectorDownRight = new Point(1, 0);
        private static readonly Point s_vectorDown = new Point(1, -1);
        private static readonly Point s_vectorDownLeft = new Point(0, -1);
        private static readonly Point s_vectorLeft = new Point(-1, -1);
        private static readonly Point s_vectorUpLeft = new Point(-1, 0);
        private static readonly Point s_vectorUp = new Point(-1, 1);
        private static readonly Point s_vectorUpRight = new Point(0, 1);

        public short Floor;
        public short Id;
        public byte LosMov;
        public byte MapChangeData;
        public uint MoveZone;
        public byte Speed;

        private Point? m_point;
        public Point Point
        {
            get
            {
                return m_point != null ? m_point.Value : ( m_point = GetPointFromCell(Id) ).Value;
            }
        }

        public bool Walkable
        {
            get { return (LosMov & 1) == 1; }
        }

        public bool LineOfSight
        {
            get { return (LosMov & 2) == 2; }
        }

        public bool NonWalkableDuringFight
        {
            get { return (LosMov & 4) == 4; }
        }

        public bool Red
        {
            get { return (LosMov & 8) == 8; }
        }

        public bool Blue
        {
            get { return (LosMov & 16) == 16; }
        }

        public bool FarmCell
        {
            get { return (LosMov & 32) == 32; }
        }

        public bool Visible
        {
            get { return (LosMov & 64) == 64; }
        }

        public bool NonWalkableDuringRP
        {
            get { return (LosMov & 128) == 128; }
        }

        #region Point-Cell
        public static short GetCellFromPoint(Point point)
        {
            if (!m_initialized)
                InitializeStaticGrid();

            return (short)( ( point.X - point.Y ) * Map.Width + point.Y + ( point.X - point.Y ) / 2 );
        }

        public static Point GetPointFromCell(short id)
        {
            if (!m_initialized)
                InitializeStaticGrid();

            if (id < 0 || id > Map.MapSize)
                throw new IndexOutOfRangeException("Cell identifier out of bounds (" + id + ").");

            var point = s_orthogonalGridReference[id];
            
            return point;
        }

        /// <summary>
        /// Initialize a static 2D plan that is used as reference to convert a cell to a (X,Y) point
        /// </summary>
        private static void InitializeStaticGrid()
        {
            int posX = 0;
            int posY = 0;
            int cellCount = 0;

            for (int x = 0; x < Map.Height; x++)
            {
                for (int y = 0; y < Map.Width; y++)
                    s_orthogonalGridReference[cellCount++] = new Point(posX + y, posY + y);

                posX++;

                for (int y = 0; y < Map.Width; y++)
                    s_orthogonalGridReference[cellCount++] = new Point(posX + y, posY + y);

                posY--;
            }

            m_initialized = true;
        }
        #endregion

        #region Serialization

        public byte[] Serialize()
        {
            var bytes = new byte[StructSize];

            bytes[0] = (byte) (Id >> 8);
            bytes[1] = (byte) (Id & 0xFF);

            bytes[2] = (byte) (Floor >> 8);
            bytes[3] = (byte) (Floor & 0xFF);

            bytes[4] = LosMov;
            bytes[5] = MapChangeData;
            bytes[6] = Speed;

            bytes[7] = (byte) (MoveZone >> 24);
            bytes[8] = (byte) (MoveZone >> 16);
            bytes[9] = (byte) (MoveZone >> 8);
            bytes[10] = (byte) (MoveZone & 0xFF);

            return bytes;
        }

        public void Deserialize(byte[] data, int index = 0)
        {
            Id = (short) ((data[index + 0] << 8) | data[index + 1]);

            Floor = (short) ((data[index + 2] << 8) | data[index + 3]);

            LosMov = data[index + 4];
            MapChangeData = data[index + 5];
            Speed = data[index + 6];

            MoveZone = (uint) ((data[index + 7] << 24) | (data[index + 8] << 16) | (data[index + 9] << 8) | (data[index + 10]));
        }
        #endregion
    }
}