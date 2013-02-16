#region License GNU GPL
// ZoneSchema.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
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
using System.Linq;
using BiM.Behaviors.Game.Actors.RolePlay;

namespace BiM.Behaviors.Profiles
{
    public class ZoneSchema : MovementSchema
    {
        public int[] SubAreas
        {
            get;
            set;
        }

        public SchemaElement[] Maps
        {
            get;
            set;
        }

        public bool IsInZone(PlayedCharacter character)
        {
            return Array.IndexOf(SubAreas, character.Map.SubAreaId) != -1 ||
                Maps.Any(x => character.Map.Id == x.MapId && character.SubMap.SubMapId == x.SubMapId);
        }

        public override bool CanStart(PlayedCharacter character)
        {
            return IsInZone(character);
        }
    }
}