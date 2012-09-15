using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BiM.Behaviors.Data;
using BiM.Behaviors.Game.Actors.Fighters;
using BiM.Behaviors.Game.Alignement;
using BiM.Behaviors.Game.Fights;
using BiM.Behaviors.Game.Guilds;
using BiM.Behaviors.Game.Items;
using BiM.Behaviors.Game.Shortcuts;
using BiM.Behaviors.Game.Spells;
using BiM.Behaviors.Game.World;
using BiM.Behaviors.Game.World.Pathfinding;
using BiM.Protocol.Data;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;
using BiM.Protocol.Types;
using NLog;
using Job = BiM.Behaviors.Game.Jobs.Job;

namespace BiM.Behaviors.Game.Actors.RolePlay
{
    public class PlayedCharacter : Character
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public delegate void FightJoinedHandler(PlayedCharacter character, Fight fight);
        public event FightJoinedHandler FightJoined;


        private void NotifyFightJoined(Fight fight)
        {
            var evnt = FightJoined;
            if (evnt != null)
                evnt(this, fight);
        }

        public delegate void FightLeftHandler(PlayedCharacter character, Fight fight);
        public event FightLeftHandler FightLeft;

        private void NotifyFightLeft(Fight fight)
        {
            var evnt = FightLeft;
            if (evnt != null)
                evnt(this, fight);
        }

        public PlayedCharacter(Bot bot, CharacterBaseInformations informations)
        {
            if (informations == null) throw new ArgumentNullException("informations");

            Bot = bot;

            Id = informations.id;
            Level = informations.level;
            Name = informations.name;
            Breed = DataProvider.Instance.Get<Breed>(informations.breed);
            Look = informations.entityLook;
            Sex = informations.sex;

            Inventory = new Inventory(this);
            Stats = new Stats.PlayerStats(this);
            SpellsBook = new SpellsBook(this);
            Shortcuts = new ShortcutBar(this);
            Jobs = new List<Job>();
            Emotes = new List<Emoticon>();
        }

        public Bot Bot
        {
            get;
            private set;
        }

        public Breed Breed
        {
            get;
            set;
        }

        /// <summary>
        /// True = female, False = male
        /// </summary>
        public bool Sex
        {
            get;
            set;
        }

        public int Level
        {
            get;
            set;
        }

        public Stats.PlayerStats Stats
        {
            get;
            set;
        }

        public Inventory Inventory
        {
            get;
            set;
        }

        public SpellsBook SpellsBook
        {
            get;
            set;
        }

        public ShortcutBar Shortcuts
        {
            get;
            set;
        }

        public List<Emoticon> Emotes
        {
            get;
            set;
        }

        public List<Jobs.Job> Jobs
        {
            get;
            set;
        }

        public Guild Guild
        {
            get;
            set;
        }

        public override World.IContext Context
        {
            get
            {
                return base.Context;
            }
            protected set
            {
                base.Context = value;
            }
        }

        public byte RegenRate
        {
            get;
            set;
        }

        /// <summary>
        /// Not recommanded to use this
        /// </summary>
        public GameContextEnum ContextType
        {
            get;
            private set;
        }

        public Fight Fight
        {
            get { return Fighter != null ? Fighter.Fight : null; }
        }

        public PlayedFighter Fighter
        {
            get;
            private set;
        }

        #region Movements

        public bool CanMove()
        {
            return Map != null;
        }

        public void Move(short cellId)
        {
            if (CanMove())
                Move(Map.Cells[cellId]);
        }

        public void Move(Cell cell)
        {
            if (cell == null) throw new ArgumentNullException("cell");

            var pathfinder = new Pathfinder(Map, Map);
            var path = pathfinder.FindPath(Position.Cell, cell, true);

            if (IsMoving())
                CancelMove();

            if (NotifyStartMoving(path))
                Bot.SendToServer(new GameMapMovementRequestMessage(path.GetClientPathKeys(), Map.Id));
        }

        public void CancelMove()
        {
            if (!IsMoving())
                return;

            NotifyStopMoving(true);

            Bot.SendToServer(new GameMapMovementCancelMessage(Position.Cell.Id));
        }

        #endregion

        #region Cells Highlighting

        public void HighlightCell(Cell cell, Color color)
        {
            Bot.SendToClient(new DebugHighlightCellsMessage(color.ToArgb(), new[] { cell.Id }));
        }

        public void HighlightCells(IEnumerable<Cell> cells, Color color)
        {
            Bot.SendToClient(new DebugHighlightCellsMessage(color.ToArgb(), cells.Select(entry => entry.Id).ToArray()));
        }

        public void ResetCellsHighlight()
        {
            Bot.SendToClient(new DebugClearHighlightCellsMessage());
        }

        #endregion

        #region Contexts
        
        // We don't really need to handle the contexts

        public bool IsInContext()
        {
            return (int)ContextType != 0;
        }

        public void ChangeContext(GameContextEnum context)
        {
            ContextType = context;
        }

        public void LeaveContext()
        {
            var lastContext = ContextType;
            ContextType = 0;

            if (lastContext == GameContextEnum.FIGHT && IsFighting())
                LeaveFight();
        }

        #endregion

        #region Fights

        public void TryStartFightWith(GroupMonster monster)
        {
            // todo
            var cell = monster.Position.Cell;

            Move(cell);
        }

        public void EnterFight(GameFightJoinMessage message)
        {
            if (IsFighting())
                throw new Exception("Player already fighting !");

            var fight = new Fight(message, Map);
            Fighter = new PlayedFighter(this, fight);

            NotifyFightJoined(Fight);
        }

        public void LeaveFight()
        {
            if (!IsFighting())
            {
                logger.Error("Cannot leave the fight : the character is not in fight");
                return;
            }

            if (Fight.Phase != FightPhase.Ended)
            {
                // todo : have to leave fight
            }

            var fight = Fight;
            Fighter = null;

            NotifyFightLeft(fight);
        }

        public bool IsFighting()
        {
            return Fighter != null;
        }

        #endregion

        #region Update Method

        public void Update(InventoryContentMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            Inventory.Update(msg);
        }

        public void Update(ShortcutBarContentMessage msg)
        {
            // todo
        }

        public void Update(EmoteListMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            Emotes = msg.emoteIds.Select(entry => DataProvider.Instance.Get<Emoticon>(entry)).ToList();
        }

        public void Update(JobDescriptionMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            Jobs = msg.jobsDescription.Select(entry => new Jobs.Job(this, entry)).ToList();
        }

        public void Update(SetCharacterRestrictionsMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            Restrictions = msg.restrictions;
        }

        public void Update(CharacterStatsListMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            Stats.Update(msg.stats);
        }

        public void Update(SpellListMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            SpellsBook.Update(msg);
        }

        public void Update(GameRolePlayCharacterInformations msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            Position = new ObjectPosition(Map, Map.Cells[msg.disposition.cellId], (DirectionsEnum) msg.disposition.direction);
            Update(msg.humanoidInfo);

            Name = msg.name;
            Look = msg.look;
            if (Alignement == null)
                Alignement = new AlignmentInformations(msg.alignmentInfos);
            else
                Alignement.Update(msg.alignmentInfos);
        }

        #endregion

        public void Update(GameFightPlacementPossiblePositionsMessage msg)
        {
            if (msg == null) throw new ArgumentException("msg");
            Fighter.SetTeam(Fight.GetTeam((FightTeamColor) msg.teamNumber));
            Fight.Update(msg);
        }
    }
}