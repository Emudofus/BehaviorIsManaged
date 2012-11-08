#region License GNU GPL
// ChatErrorEnum.cs
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
    public enum ChatErrorEnum
    {
        CHAT_ERROR_UNKNOWN = 0,
        CHAT_ERROR_RECEIVER_NOT_FOUND = 1,
        CHAT_ERROR_INTERIOR_MONOLOGUE = 2,
        CHAT_ERROR_NO_GUILD = 3,
        CHAT_ERROR_NO_PARTY = 4,
        CHAT_ERROR_ALIGN = 5,
        CHAT_ERROR_INVALID_MAP = 6,
        CHAT_ERROR_NO_PARTY_ARENA = 7,
        CHAT_ERROR_NO_TEAM = 8,
    }
}