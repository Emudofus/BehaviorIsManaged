#region License GNU GPL
// GroupMonster.cs
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
using BiM.Behaviors.Game.World;
using BiM.Protocol.Types;
using System.Collections.Generic;
using BiM.Protocol.Data;
using System.Linq;
using BiM.Behaviors.Data;
using System;
using System.Text;

namespace BiM.Behaviors.Game.Actors.RolePlay
{
    public class GroupMonster : RolePlayActor
    {
        private Monster[] m_monsters;

        public GroupMonster(GameRolePlayGroupMonsterInformations informations, Map map)
        {
            Id = informations.contextualId;
            Look = informations.look;
            Map = map;
            Update(informations.disposition);
            AgeBonus = informations.ageBonus;
            LootShare = informations.lootShare;
            AlignmentSide = informations.alignmentSide;
            KeyRingBonus = informations.keyRingBonus;

            // Gets monsters infos.
            var monsters = new List<Monster>();
            // Main monster, his look correspond to the group monster look
            monsters.Add(Leader = new Monster(informations.staticInfos.mainCreatureLightInfos, informations.look));
            // Other monsters of the group.
            monsters.AddRange(informations.staticInfos.underlings.Select(entry => new Monster(entry)));
            m_monsters = monsters.ToArray();
        }

        public Monster[] Monsters
        {
            get { return m_monsters; }
        }

        public Monster Leader
        {
            get;
            private set;
        }

        public int Level
        {
            get { return (int) Monsters.Sum(x => x.Grade.level); }
        }

        public short AgeBonus
        {
            get;
            set;
        }

        public sbyte LootShare
        {
            get;
            set;
        }

        public sbyte AlignmentSide
        {
            get;
            set;
        }

        public bool KeyRingBonus
        {
            get;
            set;
        }

        public override string ToString()
        {
            return string.Format("RP#{0} at {1}", Id, Cell);
        }

    }
}