#region License GNU GPL
// ServerConnectionErrorEnum.cs
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
    public enum ServerConnectionErrorEnum
    {
        SERVER_CONNECTION_ERROR_DUE_TO_STATUS = 0,
        SERVER_CONNECTION_ERROR_NO_REASON = 1,
        SERVER_CONNECTION_ERROR_ACCOUNT_RESTRICTED = 2,
        SERVER_CONNECTION_ERROR_COMMUNITY_RESTRICTED = 3,
        SERVER_CONNECTION_ERROR_LOCATION_RESTRICTED = 4,
        SERVER_CONNECTION_ERROR_SUBSCRIBERS_ONLY = 5,
        SERVER_CONNECTION_ERROR_REGULAR_PLAYERS_ONLY = 6,
    }
}