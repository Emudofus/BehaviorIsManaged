#region License GNU GPL
// PresetUseResultEnum.cs
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
    public enum PresetUseResultEnum
    {
        PRESET_USE_OK = 1,
        PRESET_USE_OK_PARTIAL = 2,
        PRESET_USE_ERR_UNKNOWN = 3,
        PRESET_USE_ERR_CRITERION = 4,
        PRESET_USE_ERR_BAD_PRESET_ID = 5,
    }
}