using System;
using BiM.Behaviors.Data;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Behaviors.Game.Alignement;
using BiM.Behaviors.Game.Fights;
using BiM.Behaviors.Game.Stats;
using BiM.Behaviors.Game.World;
using BiM.Behaviors.Game.World.Pathfinding;
using BiM.Protocol.Data;
using BiM.Protocol.Messages;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Actors.Fighters
{
    public class PlayedFighter : CharacterFighter
    {
        public PlayedFighter(PlayedCharacter character, Fight fight)
            : base(fight)
        {
            Character = character;
            Map = character.Map;
            Cell = character.Cell;
            Direction = character.Direction;
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
            protected set { }
        }
        public override IMinimalStats Stats
        {
            get
            {
                return Character.Stats;
            }
        }

        public override AlignmentInformations Alignment
        {
            get { return Character.Alignement; }
            protected set { }
        }

        public override Breed Breed
        {
            get { return Character.Breed; }
            protected set { }
        }

        public override bool IsAlive
        {
            get { return Character.Stats.Health > 0; }
            set { }
        }

        public override string Name
        {
            get { return Character.Name; }
            protected set { }
        }

        public override int Level
        {
            get { return Character.Level; }
            protected set { }
        }

        public void SetTeam(FightTeam team)
        {
            if (Team != null)
                throw new Exception("Team already defined !");

            Team = team;
            Team.AddFighter(this);
        }

        public void ChangePrePlacement(Cell cell)
        {
            if (Fight.Phase != FightPhase.Placement)
            {
                logger.Warn("Call ChangePrePlacement({0}) but the fight is not in placement phase", cell);
                return;
            }

            if (Array.IndexOf(Team.PlacementCells, cell) == -1)
            {
                logger.Error("Placement {0} isn't valid", cell);
                return;
            }

            Character.Bot.SendToServer(new GameFightPlacementPositionRequestMessage(cell.Id));
        }

        public bool CanCastSpell(Spells.Spell spell, Fighter fighter)
        {
            return CanCastSpell(spell, fighter.Cell);
        }

        public bool CanCastSpell(Spells.Spell spell, Cell cell)
        {
            // todo spells modifications
            // todo states

            // todo LoS

            if (!IsPlaying())
                return false;

            if (spell.LevelTemplate.apCost > Stats.CurrentAP)
                return false;

            if (!IsInSpellRange(cell, spell.LevelTemplate))
                return false;

            return true;
        }

        public bool CastSpell(Spells.Spell spell, Cell cell)
        {
            if (!CanCastSpell(spell, cell))
                return false;

            Character.Bot.SendToServer(new GameActionFightCastRequestMessage((short) spell.Template.id, cell.Id));

            return true;
        }

        public void Move(Cell cell)
        {
            Move(cell, Stats.CurrentMP);
        }

        public void Move(Cell cell, int mp)
        {
            if(!IsPlaying())
                return;

            var pathfinding = new Pathfinder(Map, Fight, false);
            var path = pathfinding.FindPath(Cell, cell, false, Stats.CurrentMP > mp ? Stats.CurrentMP : mp);

            if (NotifyStartMoving(path))
                Character.Bot.SendToServer(new GameMapMovementRequestMessage(path.GetClientPathKeys(), Map.Id));
        }

        public void PassTurn()
        {
            if (IsPlaying())
            {
                Character.Bot.SendToServer(new GameFightTurnFinishMessage());
            }
        }
    }
}