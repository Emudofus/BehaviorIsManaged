using System;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Behaviors.Game.Alignement;
using BiM.Behaviors.Game.Fights;
using BiM.Protocol.Data;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Actors.Fighters
{
    public class PlayedFighter : CharacterFighter
    {
        public PlayedFighter(PlayedCharacter character, Fight fight)
            : base(fight)
        {
            Character = character;
            Fight = fight;

            if (character.Position != null)
                Position = character.Position.Clone();
        }

        public PlayedCharacter Character
        {
            get;
            private set;
        }

        public override int Id
        {
            get { return Character.Id; }
            protected set { }
        }

        public override EntityLook Look
        {
            get { return Character.Look; }
            set { }
        }

        public override AlignmentInformations Alignment
        {
            get { return Character.Alignement; }
            set { }
        }

        public override Breed Breed
        {
            get { return Character.Breed; }
            set { }
        }

        public override bool IsAlive
        {
            get { return Character.Stats.Health > 0; }
            set { }
        }

        public override string Name
        {
            get { return Character.Name; }
            set { }
        }

        public override int Level
        {
            get { return Character.Level; }
            set { }
        }

        public void SetTeam(FightTeam team)
        {
            if (Team != null)
                throw new Exception("Team already defined !");

            Team = team;
            Team.AddFighter(this);
        }
    }
}