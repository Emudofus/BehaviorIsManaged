using System;
using BiM.Behaviors.Game.World;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Actors.RolePlay
{
    public class Prism : RolePlayActor
    {
        public Prism(GameRolePlayPrismInformations informations, Map map)
        {
            Id = informations.contextualId;
            Look = informations.look;
            Position = new ObjectPosition(map, informations.disposition);
        }
    }
}