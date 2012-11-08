#region License GNU GPL
// SequenceTypeEnum.cs
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
    public enum SequenceTypeEnum
    {
        SEQUENCE_SPELL = 1,
        SEQUENCE_WEAPON = 2,
        SEQUENCE_GLYPH_TRAP = 3,
        SEQUENCE_TRIGGERED = 4,
        SEQUENCE_MOVE = 5,
        SEQUENCE_CHARACTER_DEATH = 6,
        SEQUENCE_TURN_START = 7,
        SEQUENCE_TURN_END = 8,
        SEQUENCE_FIGHT_START = 9,
    }
}