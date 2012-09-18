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
        public static void HandleGameFightPlacementPossiblePositionsMessage(Bot bot, GameFightPlacementPossiblePositionsMessage message)
        {
            if (!bot.Character.IsFighting())
                logger.Error("Received GameFightPlacementPossiblePositionsMessage but character is not in fight !");
            else
                bot.Character.Update(message);
        }

        [MessageHandler(typeof(GameFightOptionStateUpdateMessage))]
        public static void HandleGameFightOptionStateUpdateMessage(Bot bot, GameFightOptionStateUpdateMessage message)
        {

        }

        [MessageHandler(typeof(GameFightShowFighterMessage))]
        public static void HandleGameFightShowFighterMessage(Bot bot, GameFightShowFighterMessage message)
        {
            if (!bot.Character.IsFighting())
                logger.Error("Received GameFightShowFighterMessage but character is not in fight !");
            else
                bot.Character.Fight.AddFighter(message.informations);
        }

        [MessageHandler(typeof(GameFightRefreshFighterMessage))]
        public static void HandleGameFightRefreshFighterMessage(Bot bot, GameFightRefreshFighterMessage message)
        {
            if (!bot.Character.IsFighting())
                logger.Error("Received GameFightRefreshFighterMessage but character is not in fight !");
            else
                bot.Character.Fight.Update(message);

        }

        [MessageHandler(typeof(GameFightLeaveMessage))]
        public static void HandleGameFightLeaveMessage(Bot bot, GameFightLeaveMessage message)
        {
        }

        [MessageHandler(typeof(GameEntitiesDispositionMessage))]
        public static void HandleGameEntitiesDispositionMessage(Bot bot, GameEntitiesDispositionMessage message)
        {
            if (!bot.Character.IsFighting())
                logger.Error("Received GameEntitiesDispositionMessage but character is not in fight !");
            else
                bot.Character.Fight.Update(message);
        }

        [MessageHandler(typeof(GameFightUpdateTeamMessage))]
        public static void HandleGameFightUpdateTeamMessage(Bot bot, GameFightUpdateTeamMessage message)
        {
            if (!bot.Character.IsFighting())
                logger.Error("Received GameFightUpdateTeamMessage but character is not in fight !");
            else
                bot.Character.Fight.Update(message);
        }

        [MessageHandler(typeof(GameFightRemoveTeamMemberMessage))]
        public static void HandleGameFightRemoveTeamMemberMessage(Bot bot, GameFightRemoveTeamMemberMessage message)
        {
            // message also used in roleplay context
            if (bot.Character.IsFighting())
                bot.Character.Fight.GetTeam(message.teamId).RemoveFighter(message.charId);
        }

        [MessageHandler(typeof(GameFightHumanReadyStateMessage))]
        public static void HandleGameFightHumanReadyStateMessage(Bot bot, GameFightHumanReadyStateMessage message)
        {
            if (!bot.Character.IsFighting())
                logger.Error("Received GameFightHumanReadyStateMessage but character is not in fight !");
            else
                bot.Character.Fight.Update(message);

        }

        [MessageHandler(typeof (GameFightStartMessage))]
        public static void HandleGameFightStartMessage(Bot bot, GameFightStartMessage message)
        {
            if (!bot.Character.IsFighting())
                logger.Error("Received GameFightStartMessage but character is not in fight !");
            else
                bot.Character.Fight.StartFight();
        }

        [MessageHandler(typeof(GameFightEndMessage))]
        public static void HandleGameFightEndMessage(Bot bot, GameFightEndMessage message)
        {
            bot.Character.Fight.EndFight(message);
            bot.Character.LeaveFight();
        }

        [MessageHandler(typeof(GameFightSynchronizeMessage))]
        public static void HandleGameFightSynchronizeMessage(Bot bot, GameFightSynchronizeMessage message)
        {
            if (!bot.Character.IsFighting())
                logger.Error("Received GameFightSynchronizeMessage but character is not in fight !");
            else
                bot.Character.Fight.Update(message);
        }

        [MessageHandler(typeof(GameFightNewRoundMessage))]
        public static void HandleGameFightNewRoundMessage(Bot bot, GameFightNewRoundMessage message)
        {
            if (!bot.Character.IsFighting())
                logger.Error("Received GameFightNewRoundMessage but character is not in fight !");
            else
                bot.Character.Fight.SetRound(message.roundNumber);
        }

        [MessageHandler(typeof(GameFightTurnStartMessage))]
        public static void HandleGameFightTurnStartMessage(Bot bot, GameFightTurnStartMessage message)
        {
            if (!bot.Character.IsFighting())
                logger.Error("Received GameFightTurnStartMessage but character is not in fight !");
            else
                bot.Character.Fight.StartTurn(message.id);
        }

        [MessageHandler(typeof(GameFightTurnListMessage))]
        public static void HandleGameFightTurnListMessage(Bot bot, GameFightTurnListMessage message)
        {
            if (!bot.Character.IsFighting())
                logger.Error("Received GameFightTurnListMessage but character is not in fight !");
            else
                bot.Character.Fight.Update(message);

        }

        [MessageHandler(typeof(GameFightTurnEndMessage))]
        public static void HandleGameFightTurnEndMessage(Bot bot, GameFightTurnEndMessage message)
        {
            if (!bot.Character.IsFighting())
                logger.Error("Received GameFightTurnEndMessage but character is not in fight !");
            else
                bot.Character.Fight.EndTurn();

        }

        [MessageHandler(typeof(GameActionFightSpellCastMessage))]
        public static void HandleGameActionFightSpellCastMessage(Bot bot, GameActionFightSpellCastMessage message)
        {

        }

        [MessageHandler(typeof(GameActionFightDispellableEffectMessage))]
        public static void HandleGameActionFightDispellableEffectMessage(Bot bot, GameActionFightDispellableEffectMessage message)
        {

        }

        [MessageHandler(typeof(GameActionFightMarkCellsMessage))]
        public static void HandleGameActionFightMarkCellsMessage(Bot bot, GameActionFightMarkCellsMessage message)
        {

        }

        [MessageHandler(typeof(GameActionFightUnmarkCellsMessage))]
        public static void HandleGameActionFightUnmarkCellsMessage(Bot bot, GameActionFightUnmarkCellsMessage message)
        {

        }

        [MessageHandler(typeof(GameActionFightTriggerGlyphTrapMessage))]
        public static void HandleGameActionFightTriggerGlyphTrapMessage(Bot bot, GameActionFightTriggerGlyphTrapMessage message)
        {

        }
    }
}