#region License GNU GPL
// ExchangeTypeEnum.cs
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
    public enum ExchangeTypeEnum
    {
        NPC_SHOP = 0,
        PLAYER_TRADE = 1,
        NPC_TRADE = 2,
        CRAFT = 3,
        DISCONNECTED_VENDOR = 4,
        STORAGE = 5,
        SHOP_STOCK = 6,
        TAXCOLLECTOR = 8,
        NPC_MODIFY_TRADE = 9,
        BIDHOUSE_SELL = 10,
        BIDHOUSE_BUY = 11,
        MULTICRAFT_CRAFTER = 12,
        MULTICRAFT_CUSTOMER = 13,
        JOB_INDEX = 14,
        MOUNT = 15,
        MOUNT_STABLE = 16,
        NPC_RESURECT_PET = 17,
        NPC_TRADE_MOUNT = 18,
        REALESTATE_HOUSE = 19,
        REALESTATE_FARM = 20,
    }
}