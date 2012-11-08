#region License GNU GPL
// DirectionsEnum.cs
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
using System.Collections.Generic;

namespace BiM.Protocol.Enums
{
    public enum DirectionsEnum
    {
        DIRECTION_EAST = 0,
        DIRECTION_SOUTH_EAST = 1,
        DIRECTION_SOUTH = 2,
        DIRECTION_SOUTH_WEST = 3,
        DIRECTION_WEST = 4,
        DIRECTION_NORTH_WEST = 5,
        DIRECTION_NORTH = 6,
        DIRECTION_NORTH_EAST = 7,
    }
}