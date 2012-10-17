using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Behaviors.Game.Actors;
using BiM.Behaviors.Game.Actors.Fighters;
using BiM.Behaviors.Game.World;
using BiM.Behaviors.Game.World.Pathfinding;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;
using BiM.Protocol.Types;
using NLog;

namespace BiM.Behaviors.Game.Fights
{
    public class Fight : IContext, IMapDataProvider
    {
        public delegate void StateChangedHandler(Fight fight, FightPhase phase);
        public event StateChangedHandler StateChanged;

        protected void OnStateChanged(FightPhase phase)
        {
            StateChangedHandler handler = StateChanged;
            if (handler != null) handler(this, phase);
        }


        public delegate void TurnHandler(Fight fight, Fighter fighter);
        public event TurnHandler TurnStarted;
        public event TurnHandler TurnEnded;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();


        public Fight(GameFightJoinMessage msg, Map map)
        {
            Map = map;
            StartTime = DateTime.Now - TimeSpan.FromMilliseconds(msg.timeMaxBeforeFightStart);
            CanCancelFight = msg.canBeCancelled;
            CanSayReady = msg.canSayReady;
            IsSpectator = msg.isSpectator;
            Type = (FightTypeEnum) msg.fightType;

            RedTeam = new FightTeam(this, FightTeamColor.Red);
            BlueTeam = new FightTeam(this, FightTeamColor.Blue);

            if (msg.timeMaxBeforeFightStart > 0)
                Phase = FightPhase.Placement;

            TimeLine = new TimeLine(this);
        }

        public int Id
        {
            get;
            private set;
        }

        public DateTime StartTime
        {
            get;
            private set;
        }

        public TimeSpan TimeUntilStart
        {
            get { return Phase == FightPhase.Placement && StartTime > DateTime.Now ? StartTime - DateTime.Now : TimeSpan.Zero; }
        }

        public bool CanCancelFight
        {
            get;
            private set;
        }

        public bool CanSayReady
        {
            get;
            private set;
        }

        public bool IsSpectator
        {
            get;
            private set;
        }

        public FightTypeEnum Type
        {
            get;
            private set;
        }

        public FightPhase Phase
        {
            get;
            private set;
        }

        public Map Map
        {
            get;
            private set;
        }

        public FightTeam RedTeam
        {
            get;
            private set;
        }

        public FightTeam BlueTeam
        {
            get;
            private set;
        }

        public TimeLine TimeLine
        {
            get;
            private set;
        }

        public Fighter CurrentPlayer
        {
            get { return TimeLine.CurrentPlayer; }
        }

        public int Round
        {
            get;
            private set;
        }


        #region IContext Members

        /// <summary>
        /// Returns null if not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ContextActor RemoveContextActor(int id)
        {
            Fighter fighter = GetFighter(id);

            if (fighter == null)
                return null;

            if (RemoveFighter(fighter))
                return fighter;

            return null;
        }

        public void Tick(int dt)
        {
            if (RedTeam != null)
                foreach (var fighter in RedTeam.Fighters)
                {
                    fighter.Tick(dt);
                }
            if (BlueTeam != null)
                foreach (var fighter in BlueTeam.Fighters)
                {
                    fighter.Tick(dt);
                }
        }

        public ContextActor GetContextActor(int id)
        {
            return GetFighter(id);
        }

        public ContextActor[] GetContextActors(Cell cell)
        {
            return GetConcatedFighters().Where(entry => entry.Cell == cell).Cast<ContextActor>().ToArray();
        }

        #endregion


        public FightTeam GetTeam(sbyte color)
        {
            return GetTeam((FightTeamColor) color);
        }

        public FightTeam GetTeam(FightTeamColor color)
        {
            if (color == FightTeamColor.Red) return RedTeam;
            else if (color == FightTeamColor.Blue) return BlueTeam;
            else
                throw new Exception(string.Format("Color {0} is not a valid team id !", color));
        }

        protected void SetPhase(FightPhase phase)
        {
            Phase = phase;
            OnStateChanged(phase);
        }

        public void StartFight()
        {
            if (Phase != FightPhase.Placement)
            {
                logger.Error("Cannot start the fight : the fight is not in Placement phase (Phase={0})", Phase);
                return;
            }

            SetPhase(FightPhase.Fighting);
            StartTime = DateTime.Now;
        }

        public void EndFight(GameFightEndMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            SetPhase(FightPhase.Ended);

            // todo : manage the panel
        }

        public void SetRound(int round)
        {
            Round = round;

            //evnt
        }

        public void StartTurn(int playerId)
        {
            var fighter = GetFighter(playerId);

            if (fighter == null)
                throw new InvalidOperationException(string.Format("Fighter {0} not found, cannot start turn", playerId));

            TimeLine.SetCurrentPlayer(fighter);

            var evnt = TurnStarted;
            if (evnt != null)
                evnt(this, TimeLine.CurrentPlayer);

            if (TimeLine.CurrentPlayer != null)
                TimeLine.CurrentPlayer.NotifyTurnStarted();
        }

        public void EndTurn()
        {
            var evnt = TurnEnded;
            if (evnt != null)
                evnt(this, TimeLine.CurrentPlayer);

            if (TimeLine.CurrentPlayer != null)
                TimeLine.CurrentPlayer.NotifyTurnEnded();

            TimeLine.ResetCurrentPlayer();
        }

        public bool HasFightStarted()
        {
            return DateTime.Now >= StartTime;
        }

        public void AddFighter(Fighter fighter)
        {
            if (fighter == null) throw new ArgumentNullException("fighter");

            fighter.Team.AddFighter(fighter);

            if (Phase == FightPhase.Placement)
                TimeLine.RefreshTimeLine();
        }

        public void AddFighter(GameFightFighterInformations fighter)
        {
            if (fighter == null) throw new ArgumentNullException("fighter");

            Bot bot = BotManager.Instance.GetCurrentBot();

            var existingFighter = GetFighter(fighter.contextualId);

            if (existingFighter != null)
            {
                existingFighter.Update(fighter);
            }
            else
            {
                // just update the team because it's not done
                if (fighter.contextualId == bot.Character.Id)
                {
                    bot.Character.Fighter.SetTeam(GetTeam(fighter.teamId));
                }
                else
                {
                    AddFighter(CreateFighter(fighter));
                }
            }
        }

        public bool RemoveFighter(Fighter fighter)
        {
            if (fighter == null) throw new ArgumentNullException("fighter");
            return fighter.Team.RemoveFighter(fighter);
        }

        /// <summary>
        /// Create a Fighter instance corresponding to the network given type
        /// </summary>
        /// <param name="fighter"></param>
        /// <returns></returns>
        public Fighter CreateFighter(GameFightFighterInformations fighter)
        {
            if (fighter == null) throw new ArgumentNullException("fighter");
            if (fighter is GameFightCharacterInformations)
                return new CharacterFighter(fighter as GameFightCharacterInformations, this);
            if (fighter is GameFightMonsterInformations)
                return new MonsterFighter(fighter as GameFightMonsterInformations, this);

            throw new Exception(string.Format("Fighter of type {0} not handled", fighter.GetType()));
        }

        protected IEnumerable<Fighter> GetConcatedFighters()
        {
            return RedTeam.Fighters.Concat(BlueTeam.Fighters);
        }

        /// <summary>
        /// To get the fighters sort by Initiative use TimeLine.Fighters
        /// </summary>
        /// <returns></returns>
        public Fighter[] GetFighters()
        {
            return GetConcatedFighters().ToArray();
        }

        public Fighter GetFighter(int id)
        {
            return GetConcatedFighters().FirstOrDefault(entry => entry.Id == id);
        }

        public T GetFighter<T>(int id) where T : Fighter
        {
            return (T) GetConcatedFighters().FirstOrDefault(entry => entry is T && entry.Id == id);
        }

        public Fighter GetFighter(Cell cell)
        {
            // i assume 2 fighters can be on the same cell (i.g if someone carry someone)
            return GetConcatedFighters().FirstOrDefault(entry => entry.Cell == cell);
        }

        public Fighter[] GetFighters(Cell centerCell, int radius)
        {
            return GetConcatedFighters().Where(entry => entry.Cell.IsInRadius(centerCell, radius)).ToArray();
        }

        public Fighter[] GetFighters(Cell centerCell, int minRadius, int radius)
        {
            return GetConcatedFighters().Where(entry => entry.Cell.IsInRadius(centerCell, minRadius, radius)).ToArray();
        }

        public void Update(GameFightPlacementPossiblePositionsMessage msg)
        {
            if (msg == null)
                throw new ArgumentException("msg");

            RedTeam.Update(msg);
            BlueTeam.Update(msg);

            SetPhase(FightPhase.Placement);
        }

        public void Update(GameEntitiesDispositionMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");

            foreach (IdentifiedEntityDispositionInformations disposition in msg.dispositions)
            {
                Fighter fighter = GetFighter(disposition.id);

                // seems like the client don't cares
                if (fighter != null)
                    fighter.Update(disposition);
            }
        }

        public void Update(GameFightUpdateTeamMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            Id = msg.fightId;

            GetTeam(msg.team.teamId).Update(msg.team);
        }

        public void Update(GameFightHumanReadyStateMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            Fighter fighter = GetFighter(msg.characterId);

            fighter.IsReady = msg.isReady;
        }

        public void Update(GameFightSynchronizeMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");

            TimeLine.Update(msg);

            foreach (var info in msg.fighters)
            {
                var fighter = GetFighter(info.contextualId);

                if (fighter == null)
                {
                    logger.Error("(GameFightSynchronizeMessage) Fighter {0} not found", info.contextualId);
                }
                else
                {
                    fighter.Update(info);
                }
            }
        }

        public void Update(GameFightRefreshFighterMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");

            if (!( msg.informations is GameFightFighterInformations ))
            {
                logger.Error("(GameFightRefreshFighterMessage) Cannot update a fighter with a {0} instance", msg.informations.GetType());
            }
            else
            {
                var fighter = GetFighter(msg.informations.contextualId);

                if (fighter == null)
                {
                    logger.Error("(GameFightRefreshFighterMessage) Fighter {0} not found", msg.informations.contextualId);
                }
                else
                {
                    fighter.Update(msg.informations as GameFightFighterInformations);
                }
            }

            if (Phase == FightPhase.Placement)
                TimeLine.RefreshTimeLine();
        }

        public void Update(GameFightTurnListMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            foreach (var deadsId in msg.deadsIds)
            {
                var fighter = GetFighter(deadsId);

                if (fighter == null)
                {
                    logger.Error("(GameFightTurnListMessage) Fighter {0} not found", deadsId);
                }
                else
                {
                    fighter.IsAlive = false;
                }
            }

            foreach (var id in msg.ids)
            {
                var fighter = GetFighter(id);

                if (fighter == null)
                {
                    logger.Error("(GameFightTurnListMessage) Fighter {0} not found", id);
                }
                else
                {
                    fighter.IsAlive = false;
                }
            }
        }

        public void Update(GameFightOptionStateUpdateMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            Id = msg.fightId;
            GetTeam(msg.teamId).Update(msg);
        }

        public bool IsActor(Cell cell)
        {
            return GetConcatedFighters().Any(x => x.Cell == cell);
        }

        public bool IsCellMarked(Cell cell)
        {
            return false;
        }

        public object[] GetMarks(Cell cell)
        {
            return new object[0];
        }

        public bool IsCellWalkable(Cell cell, bool throughEntities = false, Cell previousCell = null)
        {
            if (!cell.Walkable)
                return false;

            if (cell.NonWalkableDuringFight)
                return false;

            // compare the floors
            if (Map.UsingNewMovementSystem && previousCell != null)
            {
                var floorDiff = Math.Abs(cell.Floor) - Math.Abs(previousCell.Floor);

                if (cell.MoveZone != previousCell.MoveZone || cell.MoveZone == previousCell.MoveZone && cell.MoveZone == 0 && floorDiff > Map.ElevationTolerance)
                    return false;
            }

            // todo : LoS

            return true;
        }
    }
}