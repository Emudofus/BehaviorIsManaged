#region License GNU GPL
// CharacterInventoryPositionEnum.cs
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
    public enum CharacterInventoryPositionEnum
    {
        ACCESSORY_POSITION_HAT = 6,
        ACCESSORY_POSITION_CAPE = 7,
        ACCESSORY_POSITION_BELT = 3,
        ACCESSORY_POSITION_BOOTS = 5,
        ACCESSORY_POSITION_AMULET = 0,
        ACCESSORY_POSITION_SHIELD = 15,
        ACCESSORY_POSITION_WEAPON = 1,
        ACCESSORY_POSITION_PETS = 8,
        INVENTORY_POSITION_RING_LEFT = 2,
        INVENTORY_POSITION_RING_RIGHT = 4,
        INVENTORY_POSITION_DOFUS_1 = 9,
        INVENTORY_POSITION_DOFUS_2 = 10,
        INVENTORY_POSITION_DOFUS_3 = 11,
        INVENTORY_POSITION_DOFUS_4 = 12,
        INVENTORY_POSITION_DOFUS_5 = 13,
        INVENTORY_POSITION_DOFUS_6 = 14,
        INVENTORY_POSITION_MOUNT = 16,
        INVENTORY_POSITION_MUTATION = 20,
        INVENTORY_POSITION_BOOST_FOOD = 21,
        INVENTORY_POSITION_FIRST_BONUS = 22,
        INVENTORY_POSITION_SECOND_BONUS = 23,
        INVENTORY_POSITION_FIRST_MALUS = 24,
        INVENTORY_POSITION_SECOND_MALUS = 25,
        INVENTORY_POSITION_ROLEPLAY_BUFFER = 26,
        INVENTORY_POSITION_FOLLOWER = 27,
        INVENTORY_POSITION_NOT_EQUIPED = 63,
    }
}