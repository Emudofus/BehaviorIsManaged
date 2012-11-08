#region License GNU GPL
// DebugLevelEnum.cs
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
    public enum DebugLevelEnum
    {
        LEVEL_TRACE = 0,
        LEVEL_DEBUG = 1,
        LEVEL_INFO = 2,
        LEVEL_WARN = 3,
        LEVEL_ERROR = 4,
        LEVEL_FATAL = 5,
    }
}