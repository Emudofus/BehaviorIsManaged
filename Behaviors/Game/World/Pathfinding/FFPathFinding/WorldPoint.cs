#region License GNU GPL
// Spell.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

// Author : FastFrench - antispam@laposte.net
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BiM.Behaviors.Game.World.Pathfinding.FFPathFinding
{
    public class WorldPoint
    {
        /// <summary>
        /// Set point Coordinates from mapId
        /// </summary>
        /// <param name="mapId"></param>
        public WorldPoint(int mapId)
        {
            WorldId = (mapId & 0x3FFC0000) >> 18;
            X = (mapId & 0x0003FFFF) >> 9 & 511;
            Y = mapId & 0x0000001FF;
            if ((X & 0x00000100) == 0x00000100)
            {
                X = -(X & 0x000000FF);
            }
            if ((Y & 0x00000100) == 0x00000100)
            {
                Y = -(Y & 0x000000FF);
            }
        }

        public WorldPoint(int mapId, WorldMap.Direction direction)
            : this(mapId)
            {
                Move(direction);
            }

        public WorldPoint(WorldPoint point) { X = point.X; Y = point.Y; WorldId = point.WorldId; }
        public WorldPoint(WorldPoint point, WorldMap.Direction direction) : this(point)
        {
            Move(direction);
        }
        
        public WorldPoint(int x, int y, int worldId) { X = x; Y = y; WorldId = worldId; }
        public WorldPoint(int x, int y) { X = x; Y = y; WorldId = 0; }
        public WorldPoint() { X = int.MinValue; Y = int.MinValue; WorldId = 0; }
        public static WorldPoint EmptyPoint = new WorldPoint();
        public bool isEmpty() { return this == null || (X == EmptyPoint.X && Y == EmptyPoint.Y); }
        public int X;
        public int Y;
        public int WorldId;

        /// <summary>
        /// Compute mapId from WorldPoint coordinates
        /// </summary>
        /// <returns></returns>
        public int MapID
        {
            get
            {
                const int MAP_COORDS_MAX = 0x00000200;
                const int WORLD_ID_MAX = 0x00002000;

                if ((X > MAP_COORDS_MAX) || (Y > MAP_COORDS_MAX) || (WorldId > WORLD_ID_MAX))
                {
                    throw new System.ArgumentException("Coordinates or world identifier out of range.");
                }

                int _X = Math.Abs(X) & 255;
                int _Y = Math.Abs(Y) & 255;
                int _WorldId = WorldId & 4095;
                if (X < 0)
                {
                    _X = _X | 0x00000100;
                }
                if (Y < 0)
                {
                    _Y = _Y | 0x00000100;
                }
                int _mapId = _WorldId << 18 | ((_X << 9) | _Y);
                return _mapId;
            }
        }

        public void Move(WorldMap.Direction direction)
        {
            switch (direction)
            {
                case WorldMap.Direction.Bottom:
                        Y += 1;
                        break;
                case WorldMap.Direction.Top:
                        Y -= 1;
                        break;
                case WorldMap.Direction.Left:
                        X -= 1;
                        break;
                case WorldMap.Direction.Right:
                        X += 1;
                        break;
            }

        }

        public WorldPoint GetNeighbourPosition(WorldMap.Direction Direction)
        {
            return new WorldPoint(this, Direction);
        }
    }
}
