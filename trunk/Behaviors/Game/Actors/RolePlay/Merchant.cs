using System;
using BiM.Behaviors.Game.World;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Actors.RolePlay
{
    public class Merchant : RolePlayActor
    {
        public Merchant(GameRolePlayMerchantWithGuildInformations informations, Map map)
            : this((GameRolePlayMerchantInformations)informations, map)
        {
        }

        public Merchant(GameRolePlayMerchantInformations informations, Map map)
        {
            Id = informations.contextualId;
            Look = informations.look;
            Position = new ObjectPosition(map, informations.disposition);
        }
    }
}