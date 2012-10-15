using System;
using BiM.Behaviors.Game.World;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Actors.RolePlay
{


    public class Npc : RolePlayActor
    {
        public Npc(GameRolePlayNpcWithQuestInformations informations, Map map)
            : this ((GameRolePlayNpcInformations)informations, map)
        {

        }

        public Npc(GameRolePlayNpcInformations informations, Map map)
        {
            Id = informations.contextualId;
            Look = informations.look;
            Map = map;
            Update(informations.disposition);
        }
    }
}