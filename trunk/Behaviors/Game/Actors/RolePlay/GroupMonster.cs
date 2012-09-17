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

        public GroupMonster(GameRolePlayGroupMonsterInformations gameRolePlayGroupMonsterInformations, Map map)
        {
            Id = gameRolePlayGroupMonsterInformations.contextualId;
            Look = gameRolePlayGroupMonsterInformations.look;
            Position = new ObjectPosition(map, gameRolePlayGroupMonsterInformations.disposition);

            // Gets monsters infos.
            List<Monster> monsters = new List<Monster>();
            // Main monster.
            monsters.Add(new Monster(gameRolePlayGroupMonsterInformations.staticInfos.mainCreatureLightInfos.creatureGenericId));
            // Other monsters of the group.
            monsters.AddRange(gameRolePlayGroupMonsterInformations.staticInfos.underlings.Select(entry => new Monster(entry.creatureGenericId)));
            m_monsters = monsters.ToArray();
        }

        public Monster[] Monsters
        {
            get { return m_monsters; }
        }
    }
}