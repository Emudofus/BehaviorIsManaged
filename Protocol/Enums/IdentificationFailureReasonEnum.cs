#region License GNU GPL
// IdentificationFailureReasonEnum.cs
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
    public enum IdentificationFailureReasonEnum
    {
        BAD_VERSION = 1,
        WRONG_CREDENTIALS = 2,
        BANNED = 3,
        KICKED = 4,
        IN_MAINTENANCE = 5,
        TOO_MANY_ON_IP = 6,
        TIME_OUT = 7,
        BAD_IPRANGE = 8,
        CREDENTIALS_RESET = 9,
        EMAIL_UNVALIDATED = 10,
        SERVICE_UNAVAILABLE = 53,
        UNKNOWN_AUTH_ERROR = 99,
        SPARE = 100,
    }
}