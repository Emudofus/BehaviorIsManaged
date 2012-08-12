using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Behaviors.Data;
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
using Job = BiM.Behaviors.Game.Jobs.Job;

namespace BiM.Behaviors.Game.Actors.RolePlay
{
    public class PlayedCharacter : Character
    {

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
            Stats = new Stats.Stats(this);
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

        public Stats.Stats Stats
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

            var pathfinder = new Pathfinder(Map.CellInformationProvider, Map);
            var path = pathfinder.FindPath(Position.Cell, cell, true);

            NotifyStartMoving(path);
            Bot.SendToServer(new GameMapMovementRequestMessage(path.GetCompressedPathKeys(), Map.Id));
        }

        public void CancelMove()
        {
            if (!IsMoving())
                return;

            throw new NotImplementedException();
        }

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
        }

        #endregion


    }
}