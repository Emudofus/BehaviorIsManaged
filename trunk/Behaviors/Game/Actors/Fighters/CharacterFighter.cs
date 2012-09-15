using BiM.Behaviors.Data;
using BiM.Behaviors.Game.Alignement;
using BiM.Behaviors.Game.Fights;
using BiM.Behaviors.Game.World;
using BiM.Protocol.Data;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Actors.Fighters
{
    public class CharacterFighter : Fighter
    {
        protected CharacterFighter(Fight fight)
        {
            Fight = fight;
        }

        public CharacterFighter(GameFightCharacterInformations msg, Map map, Fight fight)
        {
            Id = msg.contextualId;
            Fight = fight;
            Look = msg.look;
            Position = new ObjectPosition(map, msg.disposition);
            Team = fight.GetTeam((FightTeamColor) msg.teamId);
            IsAlive = msg.alive;
            Alignment = new AlignmentInformations(msg.alignmentInfos);
            Breed = DataProvider.Instance.Get<Breed>(msg.breed);
        }

        public override int Id
        {
            get;
            protected set;
        }

        public virtual AlignmentInformations Alignment
        {
            get;
            set;
        }

        public virtual Breed Breed
        {
            get;
            set;
        }
    }
}