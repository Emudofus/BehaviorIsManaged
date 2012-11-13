#region License GNU GPL
// IMapDataProvider.cs
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