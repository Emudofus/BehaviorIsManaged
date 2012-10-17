using BiM.Behaviors.Data;
using BiM.Behaviors.Game.Fights;
using BiM.Behaviors.Game.Spells;
using BiM.Core.Messages;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;
using NLog;

namespace BiM.Behaviors.Handlers.Context
{
    public class FightHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [MessageHandler(typeof(GameFightStartingMessage))]
        public static void HandleGameFightStartingMessage(Bot bot, GameFightStartingMessage message)
        {
            // do nothing
        }

        [MessageHandler(typeof (GameFightJoinMessage))]
        public static void HandleGameFightJoinMessage(Bot bot, GameFightJoinMessage message)
        {
            bot.Character.EnterFight(message);
        }

        [MessageHandler(typeof(GameFightPlacementPossiblePositionsMessage))]
        public void HandleGameFightPlacementPossiblePositionsMessage(Bot bot, GameFightPlacementPossiblePositionsMessage message)
        {
            bot.Character.Update(message);
        }

        [MessageHandler(typeof(GameFightOptionStateUpdateMessage))]
        public void HandleGameFightOptionStateUpdateMessage(Bot bot, GameFightOptionStateUpdateMessage message)
        {
            bot.Character.Fight.Update(message);
        }

        [MessageHandler(typeof(GameFightShowFighterMessage))]
        public void HandleGameFightShowFighterMessage(Bot bot, GameFightShowFighterMessage message)
        {
            bot.Character.Fight.AddFighter(message.informations);
        }

        [MessageHandler(typeof(GameFightRefreshFighterMessage))]
        public void HandleGameFightRefreshFighterMessage(Bot bot, GameFightRefreshFighterMessage message)
        {
            bot.Character.Fight.Update(message);

        }

        [MessageHandler(typeof(GameFightLeaveMessage))]
        public void HandleGameFightLeaveMessage(Bot bot, GameFightLeaveMessage message)
        {
        }

        [MessageHandler(typeof(GameEntitiesDispositionMessage))]
        public void HandleGameEntitiesDispositionMessage(Bot bot, GameEntitiesDispositionMessage message)
        {
            bot.Character.Fight.Update(message);
        }

        [MessageHandler(typeof(GameFightUpdateTeamMessage))]
        public void HandleGameFightUpdateTeamMessage(Bot bot, GameFightUpdateTeamMessage message)
        {
            if (bot.Character.Fight.Id == message.fightId)
                bot.Character.Fight.Update(message);
        }

        [MessageHandler(typeof(GameFightRemoveTeamMemberMessage))]
        public void HandleGameFightRemoveTeamMemberMessage(Bot bot, GameFightRemoveTeamMemberMessage message)
        {
            if (bot.Character.Fight.Id == message.fightId)
                bot.Character.Fight.GetTeam(message.teamId).RemoveFighter(message.charId);
        }

        [MessageHandler(typeof(GameFightHumanReadyStateMessage))]
        public void HandleGameFightHumanReadyStateMessage(Bot bot, GameFightHumanReadyStateMessage message)
        {
            bot.Character.Fight.Update(message);

        }

        [MessageHandler(typeof (GameFightStartMessage))]
        public void HandleGameFightStartMessage(Bot bot, GameFightStartMessage message)
        {
            bot.Character.Fight.StartFight();
        }

        [MessageHandler(typeof(GameFightEndMessage))]
        public void HandleGameFightEndMessage(Bot bot, GameFightEndMessage message)
        {
            bot.Character.Fight.EndFight(message);
            bot.Character.LeaveFight();
        }

        [MessageHandler(typeof(GameFightSynchronizeMessage))]
        public void HandleGameFightSynchronizeMessage(Bot bot, GameFightSynchronizeMessage message)
        {
            bot.Character.Fight.Update(message);
        }

        [MessageHandler(typeof(GameFightNewRoundMessage))]
        public void HandleGameFightNewRoundMessage(Bot bot, GameFightNewRoundMessage message)
        {
            bot.Character.Fight.SetRound(message.roundNumber);
        }

        [MessageHandler(typeof(GameFightTurnStartMessage))]
        public void HandleGameFightTurnStartMessage(Bot bot, GameFightTurnStartMessage message)
        {
            bot.Character.Fight.StartTurn(message.id);
        }

        [MessageHandler(typeof(GameFightTurnListMessage))]
        public void HandleGameFightTurnListMessage(Bot bot, GameFightTurnListMessage message)
        {
            bot.Character.Fight.Update(message);

        }

        [MessageHandler(typeof(GameFightTurnEndMessage))]
        public void HandleGameFightTurnEndMessage(Bot bot, GameFightTurnEndMessage message)
        {
            bot.Character.Fight.EndTurn();

        }

        [MessageHandler(typeof(GameActionFightSpellCastMessage))]
        public void HandleGameActionFightSpellCastMessage(Bot bot, GameActionFightSpellCastMessage message)
        {
            var fighter = bot.Character.Fight.GetFighter(message.sourceId);

            if (fighter == null)
                logger.Error("Fighter {0} cast a spell but doesn't exist", message.sourceId);
            else
            {
                fighter.NotifySpellCasted(new SpellCast(bot.Character.Fight, message));
            }
        }

        [MessageHandler(typeof(GameActionFightDispellableEffectMessage))]
        public void HandleGameActionFightDispellableEffectMessage(Bot bot, GameActionFightDispellableEffectMessage message)
        {

        }

        [MessageHandler(typeof(GameActionFightMarkCellsMessage))]
        public void HandleGameActionFightMarkCellsMessage(Bot bot, GameActionFightMarkCellsMessage message)
        {

        }

        [MessageHandler(typeof(GameActionFightUnmarkCellsMessage))]
        public void HandleGameActionFightUnmarkCellsMessage(Bot bot, GameActionFightUnmarkCellsMessage message)
        {

        }

        [MessageHandler(typeof(GameActionFightTriggerGlyphTrapMessage))]
        public void HandleGameActionFightTriggerGlyphTrapMessage(Bot bot, GameActionFightTriggerGlyphTrapMessage message)
        {

        }
    }
}