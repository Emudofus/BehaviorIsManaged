using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Behaviors.Game.Actors;
using BiM.Behaviors.Game.Actors.Fighters;
using BiM.Behaviors.Game.World;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;
using BiM.Protocol.Types;
using NLog;

namespace BiM.Behaviors.Game.Fights
{
    public class Fight : IContext
    {
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

        public ContextActor GetContextActor(int id)
        {
            return GetFighter(id);
        }

        public ContextActor[] GetContextActors(Cell cell)
        {
            return GetConcatedFighters().Where(entry => entry.Position.Cell == cell).Cast<ContextActor>().ToArray();
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

        public void StartFight()
        {
            if (Phase != FightPhase.Placement)
            {
                logger.Error("Cannot start the fight : the fight is not in Placement phase (Phase={0})", Phase);
                return;
            }

            Phase = FightPhase.Fighting;
            StartTime = DateTime.Now;
        }

        public void EndFight(GameFightEndMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            Phase = FightPhase.Ended;

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

            TimeLine.CurrentPlayer.NotifyTurnStarted();
        }

        public void EndTurn()
        {
            TimeLine.ResetCurrentPlayer();

            var evnt = TurnEnded;
            if (evnt != null)
                evnt(this, TimeLine.CurrentPlayer);


            TimeLine.CurrentPlayer.NotifyTurnEnded();
        }

        public bool HasFightStarted()
        {
            return DateTime.Now >= StartTime;
        }

        public void AddFighter(Fighter fighter)
        {
            if (fighter == null) throw new ArgumentNullException("fighter");
            fighter.Team.AddFighter(fighter);
        }

        public void AddFighter(GameFightFighterInformations fighter)
        {
            if (fighter == null) throw new ArgumentNullException("fighter");

            Bot bot = BotManager.Instance.GetCurrentBot();

            // do not add character twice
            if (fighter.contextualId == bot.Character.Id)
                return;

            AddFighter(CreateFighter(fighter));
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
            return GetConcatedFighters().FirstOrDefault(entry => entry.Position.Cell == cell);
        }

        public Fighter[] GetFighters(Cell centerCell, int radius)
        {
            return GetConcatedFighters().Where(entry => entry.Position.Cell.IsInRadius(centerCell, radius)).ToArray();
        }

        public Fighter[] GetFighters(Cell centerCell, int minRadius, int radius)
        {
            return GetConcatedFighters().Where(entry => entry.Position.Cell.IsInRadius(centerCell, minRadius, radius)).ToArray();
        }

        public void Update(GameFightPlacementPossiblePositionsMessage msg)
        {
            if (msg == null)
                throw new ArgumentException("msg");

            RedTeam.Update(msg);
            BlueTeam.Update(msg);
        }

        public void Update(GameEntitiesDispositionMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");

            foreach (IdentifiedEntityDispositionInformations disposition in msg.dispositions)
            {
                Fighter fighter = GetFighter(disposition.id);

                // seems like the client don't cares
                if (fighter == null)
                    logger.Error(string.Format("(GameEntitiesDispositionMessage) Fight {0} not found", disposition.id));
                else
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
    }
}