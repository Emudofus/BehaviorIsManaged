using System;
using BiM.Behaviors.Game.World;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Actors.RolePlay
{
    public class TaxCollector : RolePlayActor
    {
        public TaxCollector(GameRolePlayTaxCollectorInformations informations, Map map)
        {
            Id = informations.contextualId;
            Look = informations.look;
            Map = map;
            Update(informations.disposition);

        }
    }
}