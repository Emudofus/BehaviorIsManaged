using BiM.Behaviors.Data;
using BiM.Behaviors.Game.Fights;
using BiM.Behaviors.Game.World;
using BiM.Protocol.Data;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Actors.Fighters
{
    public class MonsterFighter : Fighter
    {
        public MonsterFighter(GameFightMonsterInformations msg, Map map, Fight fight)
        {
            Id = msg.contextualId;
            Fight = fight;
            Look = msg.look;
            Position = new ObjectPosition(map, msg.disposition);
            Team = fight.GetTeam((FightTeamColor) msg.teamId);
            IsAlive = msg.alive;
            MonsterTemplate = DataProvider.Instance.Get<Monster>(msg.creatureGenericId);
            MonsterGrade = MonsterTemplate.grades[msg.creatureGrade];
        }

        public override int Id
        {
            get;
            protected set;
        }

        public MonsterGrade MonsterGrade
        {
            get;
            set;
        }

        public Monster MonsterTemplate
        {
            get;
            set;
        }
    }
}