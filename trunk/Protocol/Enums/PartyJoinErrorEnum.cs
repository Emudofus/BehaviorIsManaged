#region License GNU GPL
// PartyJoinErrorEnum.cs
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
    public enum PartyJoinErrorEnum
    {
        PARTY_JOIN_ERROR_UNKNOWN = 0,
        PARTY_JOIN_ERROR_PLAYER_NOT_FOUND = 1,
        PARTY_JOIN_ERROR_PARTY_NOT_FOUND = 2,
        PARTY_JOIN_ERROR_PARTY_FULL = 3,
        PARTY_JOIN_ERROR_PLAYER_BUSY = 4,
        PARTY_JOIN_ERROR_PLAYER_ALREADY_INVITED = 6,
        PARTY_JOIN_ERROR_PLAYER_TOO_SOLLICITED = 7,
        PARTY_JOIN_ERROR_PLAYER_LOYAL = 8,
        PARTY_JOIN_ERROR_UNMODIFIABLE = 9,
        PARTY_JOIN_ERROR_UNMET_CRITERION = 10,
    }
}