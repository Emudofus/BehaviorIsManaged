#region License GNU GPL
// PathSchema.cs
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

using System.Drawing;
using System.Linq;
using BiM.Behaviors.Game.Actors.RolePlay;

namespace BiM.Behaviors.Waypoints
{
    public class PathSchema : MovementSchema
    {
        public bool StartFromFirst
        {
            get;
            set;
        }

        public SchemaElement[] Path
        {
            get;
            set;
        }

        public override bool CanStart(PlayedCharacter character)
        {
            if (Path.Length == 0)
                return false;

            return (StartFromFirst && Path[0].MapId == character.Map.Id && Path[0].SubMapId == character.SubMap.SubMapId) ||
                Path.Any(x => character.Map.Id == x.MapId && character.SubMap.SubMapId == x.SubMapId);
        }
    }
}