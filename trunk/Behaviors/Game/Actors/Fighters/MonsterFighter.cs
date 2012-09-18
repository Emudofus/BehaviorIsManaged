using System;
using BiM.Behaviors.Data;
using BiM.Behaviors.Game.Fights;
using BiM.Behaviors.Game.Stats;
using BiM.Behaviors.Game.World;
using BiM.Protocol.Data;
using BiM.Protocol.Types;
using NLog;

namespace BiM.Behaviors.Game.Actors.Fighters
{
    public class MonsterFighter : Fighter
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public MonsterFighter(GameFightMonsterInformations msg, Fight fight)
        {
            Id = msg.contextualId;
            Fight = fight;
            Look = msg.look;
            Position = new ObjectPosition(Fight.Map, msg.disposition);
            Team = fight.GetTeam((FightTeamColor) msg.teamId);
            IsAlive = msg.alive;
            MonsterTemplate = DataProvider.Instance.Get<Monster>(msg.creatureGenericId);
            MonsterGrade = MonsterTemplate.grades[msg.creatureGrade];
            Stats = new MinimalStats(msg.stats);
        }

        public override int Id
        {
            get;
            protected set;
        }

        public MonsterGrade MonsterGrade
        {
            get;
            protected set;
        }

        public Monster MonsterTemplate
        {
            get;
            protected set;
        }

        public void Update(GameFightMonsterInformations msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            Id = msg.contextualId;
            Look = msg.look;
            Position.Update(msg.disposition);
            IsAlive = msg.alive;
            MonsterTemplate = DataProvider.Instance.Get<Monster>(msg.creatureGenericId);
            MonsterGrade = MonsterTemplate.grades[msg.creatureGrade];
        }

        public override void Update(GameFightFighterInformations informations)
        {
            if (informations == null) throw new ArgumentNullException("informations");

            if (informations is GameFightMonsterInformations)
            {
                Update(informations as GameFightMonsterInformations);
            }
            else
            {
                logger.Error("Cannot update a {0} with a {1} instance", GetType(), informations.GetType());
                base.Update(informations);
            }
        }
    }
}