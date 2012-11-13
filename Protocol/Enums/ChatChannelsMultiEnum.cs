#region License GNU GPL
// ChatChannelsMultiEnum.cs
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
    public enum ChatChannelsMultiEnum
    {
        CHANNEL_GLOBAL = 0,
        CHANNEL_TEAM = 1,
        CHANNEL_GUILD = 2,
        CHANNEL_ALIGN = 3,
        CHANNEL_PARTY = 4,
        CHANNEL_SALES = 5,
        CHANNEL_SEEK = 6,
        CHANNEL_NOOB = 7,
        CHANNEL_ADMIN = 8,
        CHANNEL_ADS = 12,
        CHANNEL_ARENA = 13,
    }
}