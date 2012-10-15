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
    }
}