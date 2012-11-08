#region License GNU GPL
// GuildCreationResultEnum.cs
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
    public enum GuildCreationResultEnum
    {
        GUILD_CREATE_OK = 1,
        GUILD_CREATE_ERROR_NAME_INVALID = 2,
        GUILD_CREATE_ERROR_ALREADY_IN_GUILD = 3,
        GUILD_CREATE_ERROR_NAME_ALREADY_EXISTS = 4,
        GUILD_CREATE_ERROR_EMBLEM_ALREADY_EXISTS = 5,
        GUILD_CREATE_ERROR_LEAVE = 6,
        GUILD_CREATE_ERROR_CANCEL = 7,
        GUILD_CREATE_ERROR_REQUIREMENT_UNMET = 8,
        GUILD_CREATE_ERROR_EMBLEM_INVALID = 9,
    }
}