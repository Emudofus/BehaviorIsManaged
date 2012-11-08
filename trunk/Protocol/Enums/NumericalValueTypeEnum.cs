#region License GNU GPL
// NumericalValueTypeEnum.cs
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
    public enum NumericalValueTypeEnum
    {
        NUMERICAL_VALUE_DEFAULT = 0,
        NUMERICAL_VALUE_COLLECT = 1,
        NUMERICAL_VALUE_CRAFT = 2,
        NUMERICAL_VALUE_PADDOCK = 3,
        NUMERICAL_VALUE_RED = 64,
        NUMERICAL_VALUE_BLUE = 65,
        NUMERICAL_VALUE_GREEN = 66,
    }
}