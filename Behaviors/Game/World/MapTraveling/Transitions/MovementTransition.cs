#region License GNU GPL
// MovementTransition.cs
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
#endregion

using System;
using System.IO;
using BiM.Behaviors.Game.Actors.RolePlay;
using NLog;

namespace BiM.Behaviors.Game.World.MapTraveling.Transitions
{
    public class MovementTransition : SubMapTransition
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public MovementTransition(MapNeighbour neighbour, short[] cells)
        {
            MapNeighbour = neighbour;
            Cells = cells;
        }

        public MapNeighbour MapNeighbour
        {
            get;
            set;
        }

        public short[] Cells
        {
            get;
            set;
        }

        public override bool BeginTransition(SubMap @from, SerializableSubMap to, PlayedCharacter character)
        {
            if (!character.ChangeMap(MapNeighbour, cell => Cells == null || Cells.Length == 0 || Array.IndexOf(Cells, cell.CellId) != -1))
            {
                logger.Error("Cannot proceed transition : cannot reach {0} map from {1} (submap:{2} to {3})",
                             MapNeighbour, character.Map.Id, @from.GlobalId, to.GlobalId);

                return false;
            }

            return true;
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write((int)MapNeighbour);
        }

        public override void Deserialize(BinaryReader reader)
        {
            MapNeighbour = (MapNeighbour)reader.ReadInt32();
        }
    }
}