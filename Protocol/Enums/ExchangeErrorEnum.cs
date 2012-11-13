#region License GNU GPL
// ExchangeErrorEnum.cs
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
    public enum ExchangeErrorEnum
    {
        REQUEST_IMPOSSIBLE = 1,
        REQUEST_CHARACTER_OCCUPIED = 2,
        REQUEST_CHARACTER_JOB_NOT_EQUIPED = 3,
        REQUEST_CHARACTER_TOOL_TOO_FAR = 4,
        REQUEST_CHARACTER_OVERLOADED = 5,
        REQUEST_CHARACTER_NOT_SUSCRIBER = 6,
        REQUEST_CHARACTER_RESTRICTED = 7,
        BUY_ERROR = 8,
        SELL_ERROR = 9,
        MOUNT_PADDOCK_ERROR = 10,
        BID_SEARCH_ERROR = 11,
    }
}