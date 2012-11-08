#region License GNU GPL
// TaxCollectorErrorReasonEnum.cs
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
    public enum TaxCollectorErrorReasonEnum
    {
        TAX_COLLECTOR_ERROR_UNKNOWN = 0,
        TAX_COLLECTOR_NOT_FOUND = 1,
        TAX_COLLECTOR_NOT_OWNED = 2,
        TAX_COLLECTOR_NO_RIGHTS = 3,
        TAX_COLLECTOR_MAX_REACHED = 4,
        TAX_COLLECTOR_ALREADY_ONE = 5,
        TAX_COLLECTOR_CANT_HIRE_YET = 6,
        TAX_COLLECTOR_CANT_HIRE_HERE = 7,
        TAX_COLLECTOR_NOT_ENOUGH_KAMAS = 8,
    }
}