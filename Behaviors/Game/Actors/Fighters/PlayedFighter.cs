#region License GNU GPL
// PlayedFighter.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion
using System;
using System.Collections.Generic;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Behaviors.Game.Alignement;
using BiM.Behaviors.Game.Fights;
using BiM.Behaviors.Game.Stats;
using BiM.Behaviors.Game.World;
using BiM.Behaviors.Game.World.Pathfinding;
using BiM.Protocol.Data;
using BiM.Protocol.Messages;
using BiM.Protocol.Types;
using Breed = BiM.Behaviors.Game.Breeds.Breed;

namespace BiM.Behaviors.Game.Actors.Fighters
{
    public partial class PlayedFighter : CharacterFighter
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


        public override bool Summoned
        {
            get { return false; }
            protected set { }
        }

        public override Fighter Summoner
        {
            get { return null; }
            protected set { }
        }

        /// <summary>
        /// Define the fighter team. Throws Exception if already defined.
        /// </summary>
        /// <param name="team">Fighter team</param>
        public void SetTeam(FightTeam team)
        {
            if (Team != null)
                throw new Exception("Team already defined !");

            Team = team;
        }

        /// <summary>
        /// Try to change the cell during the placement phase
        /// </summary>
        /// <param name="cell">Targeted cell</param>
        /// <returns>Returns false if cannot change the placement cell</returns>
        public bool ChangePrePlacement(Cell cell)
        {
            if (Fight.Phase != FightPhase.Placement)
            {
                logger.Warn("Call ChangePrePlacement({0}) but the fight is not in placement phase", cell);
                return false;
            }

            if (Array.IndexOf(Team.PlacementCells, cell) == -1)
            {
                logger.Error("Placement {0} isn't valid", cell);
                return false;
            }

            Character.Bot.SendToServer(new GameFightPlacementPositionRequestMessage(cell.Id));
            return true;
        }

        /// <summary>
        /// Check if the player can cast a spell to the targeted fighter
        /// </summary>
        /// <param name="spell">Casted spell</param>
        /// <param name="fighter">Targeted fighter</param>
        /// <returns>False if cannot cast the spell</returns>
        public bool CanCastSpell(Spells.Spell spell, Fighter fighter, bool NoRangeCheck = false)
        {
            return CanCastSpell(spell, fighter.Cell, NoRangeCheck);
        }

        /// <summary>
        /// Check if the player can cast a spell to the targeted cell
        /// </summary>
        /// <param name="spell">Casted spell</param>
        /// <param name="cell">Targeted cell</param>
        /// <param name="NoRangeCheck">if true, then skip all checking related with caster position, 
        /// (preparatory stuff, before fight)</param>
        /// <returns>False if cannot cast the spell</returns>
        public bool CanCastSpell(Spells.Spell spell, Cell cell, bool NoRangeCheck = false)
        {
            // todo spells modifications
            // todo states

            if (!NoRangeCheck && !IsPlaying()) 
                return false;

            if (spell.LevelTemplate.apCost > Stats.CurrentAP)
                return false;

            if (!NoRangeCheck && !IsInSpellRange(cell, spell.LevelTemplate))
                return false;

            // test the LoS
            if (!NoRangeCheck && spell.LevelTemplate.castTestLos && !Fight.CanBeSeen(Cell, cell, false))
                return false;

            return true;
        }

        /// <summary>
        /// Try to cast a spell to a targeted cell
        /// </summary>
        /// <param name="spell">Spell to cast</param>
        /// <param name="cell">Targeted cell</param>
        /// <returns>False if cannot cast the spell</returns>
        public bool CastSpell(Spells.Spell spell, Cell cell)
        {
            if (!CanCastSpell(spell, cell))
                return false;

            Character.Bot.SendToServer(new GameActionFightCastRequestMessage((short)spell.Template.id, cell.Id));

            return true;
        }

        /// <summary>
        /// Try to move to the targeted Cell (truncate the path if the player hasn't enough MP)
        /// </summary>
        /// <param name="cell">Targeted cell</param>
        public bool Move(Cell cell)
        {
            return Move(cell, Stats.CurrentMP);
        }

        /// <summary>
        /// Try to move to the targeted Cell (truncate the path if the player hasn't enough MP)
        /// </summary>
        /// <param name="cell">Targeted cell</param>
        /// <param name="mp">MP to use</param>
        /// <returns>False if cannot move</returns>
        public bool Move(Cell cell, int mp)
        {
            if (!IsPlaying())
                return false;

            var pathfinding = new Pathfinder(Map, Fight, false);
            var path = pathfinding.FindPath(Cell, cell, false, Stats.CurrentMP > mp ? Stats.CurrentMP : mp);

            if (NotifyStartMoving(path))
                Character.Bot.SendToServer(new GameMapMovementRequestMessage(path.GetClientPathKeys(), Map.Id));

            return true;
        }

        /// <summary>
        /// Pass the turn
        /// </summary>
        /// <returns>False if cannot pass the turn</returns>
        public bool PassTurn()
        {
            if (IsPlaying())
            {
                Character.Bot.CallDelayed(200, () => Character.Bot.SendToServer(new GameFightTurnFinishMessage()));
                return true;
            }

            return false;
        }

    }
}