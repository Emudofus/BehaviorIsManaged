using System;
using BiM.Behaviors.Data;
using BiM.Behaviors.Game.Alignement;
using BiM.Behaviors.Game.Fights;
using BiM.Behaviors.Game.Stats;
using BiM.Behaviors.Game.World;
using BiM.Protocol.Data;
using BiM.Protocol.Types;
using NLog;

namespace BiM.Behaviors.Game.Actors.Fighters
{
    public class CharacterFighter : Fighter
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected CharacterFighter(Fight fight)
        {
            Fight = fight;
        }

        public CharacterFighter(GameFightCharacterInformations msg, Fight fight)
        {
            Id = msg.contextualId;
            Fight = fight;
            Look = msg.look;
            Position = new ObjectPosition(Fight.Map, msg.disposition);
            Team = fight.GetTeam((FightTeamColor) msg.teamId);
            IsAlive = msg.alive;
            Alignment = new AlignmentInformations(msg.alignmentInfos);
            Breed = DataProvider.Instance.Get<Breed>(msg.breed);
            Stats = new MinimalStats(msg.stats);
        }

        public override int Id
        {
            get;
            protected set;
        }

        public virtual AlignmentInformations Alignment
        {
            get;
            protected set;
        }

        public virtual Breed Breed
        {
            get;
            protected set;
        }

        public void Update(GameFightCharacterInformations msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            Id = msg.contextualId;
            Look = msg.look;
            Position.Update(msg.disposition);
            IsAlive = msg.alive;
            Alignment = new AlignmentInformations(msg.alignmentInfos);
            Breed = DataProvider.Instance.Get<Breed>(msg.breed);
            Stats.Update(msg.stats);
        }

        public override void Update(GameFightFighterInformations informations)
        {
            if (informations == null) throw new ArgumentNullException("informations");

            if (informations is GameFightCharacterInformations)
            {
                Update(informations as GameFightCharacterInformations);
            }
            else
            {
                logger.Error("Cannot update a {0} with a {1} instance", GetType(), informations.GetType()); 
                base.Update(informations);
            }
        }
    }
}