#region License GNU GPL
// GuildInformationsTypeEnum.cs
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
    public enum GuildInformationsTypeEnum
    {
        INFO_GENERAL = 1,
        INFO_MEMBERS = 2,
        INFO_BOOSTS = 3,
        INFO_PADDOCKS = 4,
        INFO_HOUSES = 5,
        INFO_TAX_COLLECTOR = 6,
        INFO_TAX_COLLECTOR_LEAVE = 7,
    }
}