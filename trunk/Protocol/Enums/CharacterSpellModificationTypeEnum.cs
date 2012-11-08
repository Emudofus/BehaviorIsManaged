#region License GNU GPL
// CharacterSpellModificationTypeEnum.cs
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
    public enum CharacterSpellModificationTypeEnum
    {
        INVALID_MODIFICATION = 0,
        RANGEABLE = 1,
        DAMAGE = 2,
        BASE_DAMAGE = 3,
        HEAL_BONUS = 4,
        AP_COST = 5,
        CAST_INTERVAL = 6,
        CAST_INTERVAL_SET = 7,
        CRITICAL_HIT_BONUS = 8,
        CAST_LINE = 9,
        LOS = 10,
        MAX_CAST_PER_TURN = 11,
        MAX_CAST_PER_TARGET = 12,
        RANGE = 13,
    }
}