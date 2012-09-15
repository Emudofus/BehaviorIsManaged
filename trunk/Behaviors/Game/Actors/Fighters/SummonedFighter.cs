using BiM.Behaviors.Game.Fights;
using BiM.Behaviors.Game.World;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Actors.Fighters
{
    public class SummonedFighter : MonsterFighter
    {
        public SummonedFighter(GameFightMonsterInformations msg, Map map, Fight fight)
            : base(msg, map, fight)
        {
        }

        public Fighter Summoner
        {
            get;
            set;
        }
    }
}