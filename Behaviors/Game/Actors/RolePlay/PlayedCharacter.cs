#region License GNU GPL
// PlayedCharacter.cs
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
using System.Drawing;
using System.Linq;
using BiM.Behaviors.Data;
using BiM.Behaviors.Game.Actors.Fighters;
using BiM.Behaviors.Game.Alignement;
using BiM.Behaviors.Game.Fights;
using BiM.Behaviors.Game.Guilds;
using BiM.Behaviors.Game.Interactives;
using BiM.Behaviors.Game.Items;
using BiM.Behaviors.Game.Shortcuts;
using BiM.Behaviors.Game.Spells;
using BiM.Behaviors.Game.Stats;
using BiM.Behaviors.Game.World;
using BiM.Behaviors.Game.World.Pathfinding;
using BiM.Behaviors.Handlers.Context;
using BiM.Core.Collections;
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


        private void OnFightJoined(Fight fight)
        {
            var evnt = FightJoined;
            if (evnt != null)
                evnt(this, fight);
        }

        public delegate void FightLeftHandler(PlayedCharacter character, Fight fight);
        public event FightLeftHandler FightLeft;

        private void OnFightLeft(Fight fight)
        {
            var evnt = FightLeft;
            if (evnt != null)
                evnt(this, fight);
        }

        public delegate void MapJoinedHandler(PlayedCharacter character, Map map);
        public event MapJoinedHandler MapJoined;

        private void OnMapJoined(Map map)
        {
            MapJoinedHandler handler = MapJoined;
            if (handler != null) handler(this, map);
        }

        private ObservableCollectionMT<Job> m_jobs;
        private ReadOnlyObservableCollectionMT<Job> m_readOnlyJobs;

        private ObservableCollectionMT<Emoticon> m_emotes;
        private ReadOnlyObservableCollectionMT<Emoticon> m_readOnlyEmotes;

        private InteractiveSkill m_useAfterMove;
        private int? m_nextMap;

        public PlayedCharacter(Bot bot, CharacterBaseInformations informations)
        {
            if (informations == null) throw new ArgumentNullException("informations");

            Bot = bot;

            Id = informations.id;
            Level = informations.level;
            Name = informations.name;
            Breed = new Breeds.Breed(DataProvider.Instance.Get<Breed>(informations.breed));
            Look = informations.entityLook;
            Sex = informations.sex;

            Inventory = new Inventory(this);
            Stats = new Stats.PlayerStats(this);
            SpellsBook = new SpellsBook(this);
            SpellShortcuts = new SpellShortcutBar(this);
            GeneralShortcuts = new GeneralShortcutBar(this);

            m_jobs = new ObservableCollectionMT<Job>();
            m_readOnlyJobs = new ReadOnlyObservableCollectionMT<Job>(m_jobs);
            m_emotes = new ObservableCollectionMT<Emoticon>();
            m_readOnlyEmotes = new ReadOnlyObservableCollectionMT<Emoticon>(m_emotes);
        }

        public override void Tick(int dt)
        {
            base.Tick(dt);
            UpdateRegen();
        }

        public Bot Bot
        {
            get;
            private set;
        }

        public Breeds.Breed Breed
        {
            get;
            private set;
        }

        /// <summary>
        /// True = female, False = male
        /// </summary>
        public bool Sex
        {
            get;
            private set;
        }

        public int Level
        {
            get;
            private set;
        }

        public Stats.PlayerStats Stats
        {
            get;
            private set;
        }

        public Inventory Inventory
        {
            get;
            private set;
        }

        public SpellsBook SpellsBook
        {
            get;
            private set;
        }

        public SpellShortcutBar SpellShortcuts
        {
            get;
            private set;
        }

        public GeneralShortcutBar GeneralShortcuts
        {
            get;
            private set;
        }

        public ReadOnlyObservableCollectionMT<Emoticon> Emotes
        {
            get { return m_readOnlyEmotes; }
        }

        public ReadOnlyObservableCollectionMT<Job> Jobs
        {
            get { return m_readOnlyJobs; }
        }

        public Guild Guild
        {
            get;
            private set;
        }

        public byte RegenRate
        {
            get;
            set;
        }

        public DateTime? RegenStartTime
        {
            get;
            set;
        }

        /// <summary>
        /// Call this periodically, to update Health according to current regen rate
        /// </summary>
        public void UpdateRegen()
        {
            if (IsFighting()) return; // No regen in fights ? Well... anyway it would be turn-based

            if (RegenRate > 0 && RegenStartTime != null)
            {
                double elapsedSeconds = (DateTime.Now - RegenStartTime).Value.TotalSeconds;
                if (elapsedSeconds < 3.0) return; // Avoids significative errors when UpdateRegen is called too often.
                var regainedLife = (int)Math.Floor(elapsedSeconds / (RegenRate / 10.0f));
                if (regainedLife <= 0) return; // Avoids significative errors when UpdateRegen is called too often when regenRate is low.
                if (Stats.Health + regainedLife > Stats.MaxHealth)
                {
                    Stats.Health = Stats.MaxHealth;
                    RegenRate = 0;
                    RegenStartTime = null;
                }
                else
                {
                    Stats.Health += regainedLife;
                    RegenStartTime = DateTime.Now;
                }
            }
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
            return Map != null && !IsFighting();
        }

        public bool Move(short cellId, ISimplePathFinder pathFinder = null, int minDistance = 0)
        {
            if (CanMove())
                return Move(Map.Cells[cellId], pathFinder, minDistance);

            return false;
        }

        public bool Move(Path path, Cell dest)
        {
            if (IsMoving())
                CancelMove(false);

            // DEBUG
            //SendMessage(String.Format("Move {0} => {1} : {2}", Cell, dest, String.Join<Cell>(",", path.Cells)));
            //ResetCellsHighlight();
            //HighlightCells(path.Cells, Color.YellowGreen);
            if (path.Cells.Length == 0)
            {
                SendMessage("Empty path skipped", Color.Gray);
                return false;
            }
            else
                if (path.Start.Id != Cell.Id)
                {
                    SendMessage(String.Format("Path start with {0} instead of {1}", path.Start, Cell), Color.Red);
                    return false;
                }

            Bot.SendToServer(new GameMapMovementRequestMessage(path.GetClientPathKeys(), Map.Id));
            return true;

        }

        public bool Move(Cell cell, ISimplePathFinder pathFinder = null, int minDistance = 0)
        {
            if (cell == null) throw new ArgumentNullException("cell");

            if (!CanMove())
                return false;
            if (pathFinder==null)
                if (minDistance == 0)
                    pathFinder = new Pathfinder(Map, Map);
                else
                    pathFinder = new BiM.Behaviors.Game.World.Pathfinding.FFPathFinding.PathFinder(Map, false);
            Path path = null;
            if (pathFinder is IAdvancedPathFinder)
                path = (pathFinder as IAdvancedPathFinder).FindPath(Cell, cell, false, -1, minDistance);
            else
                path = pathFinder.FindPath(Cell, cell, false, -1);

            return Move(path, cell);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refused">true if path were rejected (not even started)</param>
        public void CancelMove(bool refused)
        {
            if (!IsMoving())
                return;

            NotifyStopMoving(true, refused);

            Bot.SendToServer(new GameMapMovementCancelMessage(Cell.Id));
        }

        public override void NotifyStopMoving(bool canceled, bool refused)
        {
            base.NotifyStopMoving(canceled, refused);

            if (m_useAfterMove != null)
            {
                if (!canceled)
                {
                    var skill = m_useAfterMove;
                    Bot.AddMessage(() => UseInteractiveObject(skill)); // call next tick
                }

                m_useAfterMove = null;
            }

            if (m_nextMap != null)
            {
                if (!canceled)
                {
                    var id = m_nextMap.Value;
                    Bot.AddMessage(() => Bot.SendToServer(new ChangeMapMessage(id)));
                }

                m_nextMap = null;
            }
        }

        public bool ChangeMap(MapNeighbour neighbour, ISimplePathFinder pathfinder=null)
        {
            var neighbourId = GetNeighbourId(neighbour);
            if (neighbourId < 0)
                return false;

            var mapChangeData = Map.MapChangeDatas[neighbour];
            Path path = null;
            bool found = false;
                
            if (pathfinder != null && pathfinder is IAdvancedPathFinder)
                path = (pathfinder as IAdvancedPathFinder).FindPath(Cell, Map.Cells.Where(x => x != Cell && (x.MapChangeData & mapChangeData) != 0 && Map.IsCellWalkable(x)), true);                
            else
            {
                var cells = Map.Cells.Where(x => x != Cell && (x.MapChangeData & mapChangeData) != 0 && Map.IsCellWalkable(x)).
                    OrderBy(x => x.DistanceTo(Cell));

                if (pathfinder == null)
                    pathfinder = new Pathfinder(Map, Map, true, true);
                var enumerator = cells.GetEnumerator();
                while ((path == null || path.IsEmpty()) && (found = enumerator.MoveNext()))
                {
                    path = pathfinder.FindPath(Cell, enumerator.Current, true);
                }

            
            }
            if (path != null)
            {
                if (Move(path, path.End))
                {
                    m_nextMap = neighbourId;
                    return true;
                }
                return false;
            }

            return false;
        }

        private int GetNeighbourId(MapNeighbour neighbour)
        {
            switch (neighbour)
            {
                case MapNeighbour.Top:
                    return Map.TopNeighbourId;
                case MapNeighbour.Bottom:
                    return Map.BottomNeighbourId;
                case MapNeighbour.Right:
                    return Map.RightNeighbourId;
                case MapNeighbour.Left:
                    return Map.LeftNeighbourId;
                default:
                    return -1;
            }
        }

        #endregion

        #region Chat
        public void Say(string message)
        {
            Say(message, ChatActivableChannelsEnum.CHANNEL_GLOBAL);
        }

        public void Say(string message, ChatActivableChannelsEnum channel)
        {
            Bot.SendToServer(new ChatClientMultiMessage(message, (sbyte)channel));
        }

        public void SayTo(string message, string receiverName)
        {
            Bot.SendToServer(new ChatClientPrivateMessage(message, receiverName));
        }

        public void SendTextInformation(TextInformationTypeEnum type, short id, params object[] parameters)
        {
            Bot.SendToClient(new TextInformationMessage((sbyte)type, id, parameters.Select(entry => entry.ToString()).ToArray()));

        }

        /// <summary>
        /// Send a message to the client's chat
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(string message)
        {
            SendTextInformation(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 0, message);
        }

        /// <summary>
        /// Send a message to the client's chat
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(string message, Color color)
        {
            SendMessage(string.Format("<font color=\"#{0}\">{1}</font>", color.ToArgb().ToString("X"), message));
        }

        public void OpenPopup(string message)
        {
            OpenPopup(message, "BiM", 0);
        }

        public void OpenPopup(string message, string sender, byte lockDuration)
        {
            Bot.SendToClient(new PopupWarningMessage(lockDuration, sender, message));
        }

        #endregion

        #region Stats

        public void SpendStatsPoints(BoostableStat stat, short amount)
        {
            if (amount > Stats.SpellsPoints)
                amount = (short) Stats.SpellsPoints;

            Bot.SendToServer(new StatsUpgradeRequestMessage((sbyte)stat, amount));
        }

        public short GetPointsForBoostAmount(BoostableStat stat, short amount)
        {
            var currentPoints = GetBoostableStatPoints(stat);
            var threshold = Breed.GetThreshold(currentPoints, stat);
            var nextThreshold = Breed.GetNextThreshold(currentPoints, stat);
            short pointsToSpend = 0;
            int totalBoost = 0;

            while (totalBoost < amount)
            {
                short boost = 0;
                if (nextThreshold != null && currentPoints >= nextThreshold.PointsThreshold)
                {
                    threshold = Breed.GetThreshold(currentPoints, stat);
                    nextThreshold = Breed.GetNextThreshold(currentPoints, stat);
                }

                // must fill the current threshold first
                if (nextThreshold != null && amount > ( nextThreshold.PointsThreshold - currentPoints ))
                {
                    boost = (short)( nextThreshold.PointsThreshold - pointsToSpend );
                    pointsToSpend += (short)( boost * threshold.PointsPerBoost );

                    boost = (short)( boost * threshold.BoostPerPoints );
                }
                else
                {
                    pointsToSpend += (short)( amount * threshold.PointsPerBoost );

                    boost = (short)( amount * threshold.BoostPerPoints );
                }

                amount -= boost;
                currentPoints += boost;
                totalBoost += boost;
            }

            return pointsToSpend;
        }

        /// <summary>
        /// Gives the amount of boost of a given stat with the given points
        /// </summary>
        /// <param name="stat"></param>
        /// <param name="pointsToSpend"></param>
        /// <returns></returns>
        public int GetBoostAmountWithPoints(BoostableStat stat, int pointsToSpend)
        {
            var currentPoints = GetBoostableStatPoints(stat);
            var threshold = Breed.GetThreshold(currentPoints, stat);
            var nextThreshold = Breed.GetNextThreshold(currentPoints, stat);

            if (pointsToSpend < threshold.PointsPerBoost)
                return 0;

            int totalBoost = 0;
            while (pointsToSpend >= threshold.PointsPerBoost)
            {
                int boost = 0;
                if (nextThreshold != null && currentPoints >= nextThreshold.PointsThreshold)
                {
                    threshold = Breed.GetThreshold(currentPoints, stat);
                    nextThreshold = Breed.GetNextThreshold(currentPoints, stat);
                }

                if (pointsToSpend - threshold.PointsPerBoost < 0)
                    break;

                if (nextThreshold != null && ( pointsToSpend / (double)threshold.PointsPerBoost ) > ( nextThreshold.PointsThreshold - currentPoints ))
                {
                    boost = (short)( threshold.PointsThreshold - pointsToSpend );
                    pointsToSpend -= (short)( boost * threshold.PointsPerBoost );

                    boost = (short)( boost * threshold.BoostPerPoints );
                }
                else
                {
                    boost = (short)Math.Floor(pointsToSpend / (double)threshold.PointsPerBoost);
                    pointsToSpend -= (short)( boost * threshold.PointsPerBoost );

                    boost = (short)( boost * threshold.BoostPerPoints );
                }

                currentPoints += boost;
                totalBoost += boost;
            }

            return totalBoost;
        }

        private int GetBoostableStatPoints(BoostableStat stat)
        {
            switch (stat)
            {
                case BoostableStat.Agility:
                    return Stats.Agility.Base;
                case BoostableStat.Strength:
                    return Stats.Strength.Base;
                case BoostableStat.Intelligence:
                    return Stats.Intelligence.Base;
                case BoostableStat.Chance:
                    return Stats.Chance.Base;
                case BoostableStat.Wisdom:
                    return Stats.Wisdom.Base;
                case BoostableStat.Vitality:
                    return Stats.Vitality.Base;
                default:
                    throw new ArgumentException("stat");
            }
        }

        #endregion

        #region Interactives
        public bool UseInteractiveObject(InteractiveSkill skill)
        {
            m_useAfterMove = null;

            if (!Map.Interactives.Contains(skill.Interactive) || !skill.IsEnabled())
                return false;

            if (skill.Interactive.Cell != null && !skill.Interactive.Cell.IsAdjacentTo(Cell))
            {
                var cell = skill.Interactive.Cell.GetAdjacentCells(x => Map.CanStopOnCell(x)).
                    OrderBy(x => x.ManhattanDistanceTo(Cell)).FirstOrDefault();

                if (cell == null)
                    return false;

                if (Move(cell))
                {
                    m_useAfterMove = skill;
                    return true;
                }
            }
            else
            {
                Bot.SendToServer(new InteractiveUseRequestMessage(skill.Interactive.Id, skill.Id));
                return true;
            }

            return false;
        }

        #endregion

        #region Jobs
        public Job GetJob(int id)
        {
            return m_jobs.FirstOrDefault(x => x.JobTemplate.id == id);
        }
        #endregion

        #region Shortcuts

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

        public void EnterMap(Map map)
        {
            Map = map;
            Context = map;

            Bot.AddFrame(new RolePlayHandler(Bot));
            OnMapJoined(map);
        }

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

            Bot.RemoveFrame<RolePlayHandler>();
            Bot.RemoveFrame<FightHandler>();
        }

        #endregion

        #region Fights

        public void TryStartFightWith(GroupMonster monster, ISimplePathFinder pathFinder=null)
        {
            // todo
            var cell = monster.Cell;

            Move(cell, pathFinder);
        }

        public void EnterFight(GameFightJoinMessage message)
        {
            if (IsFighting())
                throw new Exception("Player already fighting !");

            var fight = new Fight(message, Map);
            Fighter = new PlayedFighter(this, fight);

            Context = Fight;
            Bot.AddFrame(new FightHandler(Bot));
            OnFightJoined(Fight);
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

            Context = Map;
            Bot.RemoveFrame<FightHandler>();
            OnFightLeft(Fight);

            Fighter = null;
        }

        public bool IsFighting()
        {
            return Fighter != null;
        }

        #endregion

        #region Update Method


        public void Update(ShortcutBarContentMessage msg)
        {
            if (((ShortcutBarEnum)msg.barType) == ShortcutBarEnum.GENERAL_SHORTCUT_BAR)
                GeneralShortcuts.Update(msg);
            else
                SpellShortcuts.Update(msg);
        }

        public void Update(EmoteListMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            m_emotes.Clear();
            foreach (var emoteId in msg.emoteIds)
            {
                m_emotes.Add(DataProvider.Instance.Get<Emoticon>(emoteId));
            }
        }

        public void Update(JobDescriptionMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            m_jobs.Clear();
            foreach (var job in msg.jobsDescription)
            {
                m_jobs.Add(new Job(this, job));
            }
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
        public void Update(GameFightStartMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            SpellsBook.FightStart(msg);
        }
        
        public void Update(GameRolePlayCharacterInformations msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            Update(msg.disposition);
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

            if (Fighter.Team == null)
            {
                // it's a bit tricky ...
                Fighter.SetTeam(Fight.GetTeam((FightTeamColor) msg.teamNumber));
                Fight.AddActor(Fighter);
            }

            Fight.Update(msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TargetId"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public IEnumerable<Spells.Spell> GetAvailableSpells(int? TargetId, Spells.Spell.SpellCategory category)
        {
            foreach (Spells.Spell spell in SpellsBook.GetAvailableSpells(TargetId, category))
                if (spell.LevelTemplate.apCost <= Stats.CurrentAP)
                    yield return spell;
        }

        public void Update(LifePointsRegenEndMessage message)
        {
            Stats.Health = message.lifePoints;  
            // TODO : unfortunately, we never receive a message saying we have full health. 
            // So have to consider regen rate and compute current health based on it. 
        }

        internal void Update(GameMapNoMovementMessage message)
        {
            if (IsMoving())
                NotifyStopMoving(true, true);
        }
    }
}