#region License GNU GPL
// SpellTargetType.cs
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

namespace BiM.Behaviors.Game.Effects
{
    [Flags]
    public enum SpellTargetType
    {
        NONE = 0,
        SELF = 0x1,
        ALLY_1 = 0x2,
        ALLY_2 = 0x4,
        ALLY_SUMMONS = 0x8,
        ALLY_STATIC_SUMMONS = 0x10,
        ALLY_3 = 0x20,
        ALLY_4 = 0x40,
        ALLY_5 = 0x80,
        ENEMY_1 = 0x100,
        ENEMY_2 = 0x200,
        ENEMY_SUMMONS = 0x400,
        ENEMY_STATIC_SUMMONS = 0x800,
        ENEMY_3 = 0x1000,
        ENEMY_4 = 0x2000,
        ENEMY_5 = 0x4000,
        ALLIES = 0xFE,
        ENEMIES = 0x7F00,
        ALL = 0x7FFF,
        ONLY_SELF = 0x8000,
    }
}