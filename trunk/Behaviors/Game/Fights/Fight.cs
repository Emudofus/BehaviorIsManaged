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
            set;
        }

        public TimeSpan TimeUntilStart
        {
            get { return Phase == FightPhase.Placement && StartTime > DateTime.Now ? StartTime - DateTime.Now : TimeSpan.Zero; }
        }

        public bool CanCancelFight
        {
            get;
            set;
        }

        public bool CanSayReady
        {
            get;
            set;
        }

        public bool IsSpectator
        {
            get;
            set;
        }

        public FightTypeEnum Type
        {
            get;
            set;
        }

        public FightPhase Phase
        {
            get;
            set;
        }

        public Map Map
        {
            get;
            set;
        }

        public FightTeam RedTeam
        {
            get;
            set;
        }

        public FightTeam BlueTeam
        {
            get;
            set;
        }

        public FightTeam GetTeam(sbyte color)
        {
            return GetTeam((FightTeamColor)color);
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

        public bool HasStarted()
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

            var bot = BotManager.Instance.GetCurrentBot();

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
        /// Returns null if not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ContextActor RemoveActor(int id)
        {
            var fighter = GetFighter(id);

            if (fighter == null)
                return null;

            if (RemoveFighter(fighter))
                return fighter;

            return null;
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
                return new CharacterFighter(fighter as GameFightCharacterInformations, Map, this);
            if (fighter is GameFightMonsterInformations)
                return new MonsterFighter(fighter as GameFightMonsterInformations, Map, this);

            throw new Exception(string.Format("Fighter of type {0} not handled", fighter.GetType()));
        }

        protected IEnumerable<Fighter> GetConcatedFighters()
        {
            return RedTeam.Fighters.Concat(BlueTeam.Fighters);
        }

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
            return (T)GetConcatedFighters().FirstOrDefault(entry => entry is T && entry.Id == id);
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

        public ContextActor[] GetActors(Cell cell)
        {
            return GetConcatedFighters().Where(entry => entry.Position.Cell == cell).Cast<ContextActor>().ToArray();
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

            foreach (var disposition in msg.dispositions)
            {
                var fighter = GetFighter(disposition.id);

                // seems like the client don't cares
                if (fighter == null)
                    logger.Error(string.Format("Received a unknown id ({0}) in GameEntitiesDispositionMessage", disposition.id));
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
            var fighter = GetFighter(msg.characterId);

            fighter.IsReady = msg.isReady;
        }
    }
}