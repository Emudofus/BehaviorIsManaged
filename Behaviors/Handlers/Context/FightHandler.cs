#region License GNU GPL
// FightHandler.cs
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
using BiM.Behaviors.Data;
using BiM.Behaviors.Frames;
using BiM.Behaviors.Game.Fights;
using BiM.Behaviors.Game.Spells;
using BiM.Core.Messages;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;
using NLog;
using BiM.Protocol.Types;
using BiM.Behaviors.Game.Actors.Fighters;

namespace BiM.Behaviors.Handlers.Context
{
    public class FightHandler : Frame<FightHandler>
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public FightHandler(Bot bot)
            : base(bot)
        {
        }

        [MessageHandler(typeof(GameFightStartingMessage))]
        public static void HandleGameFightStartingMessage(Bot bot, GameFightStartingMessage message)
        {
            bot.Character.SpellsBook.FightStart(message);
        }

        [MessageHandler(typeof (GameFightJoinMessage))]
        public static void HandleGameFightJoinMessage(Bot bot, GameFightJoinMessage message)
        {
            if (bot == null || bot.Character == null)
            {
                logger.Error("Character is not properly initialized.");
                return; // Can't handle the message
            }
            bot.Character.EnterFight(message);
        }

        [MessageHandler(typeof(GameFightPlacementPossiblePositionsMessage))]
        public void HandleGameFightPlacementPossiblePositionsMessage(Bot bot, GameFightPlacementPossiblePositionsMessage message)
        {
            if (bot == null || bot.Character == null)
            {
                logger.Error("Character is not properly initialized.");
                return; // Can't handle the message
            }
            bot.Character.Update(message);
        }

        [MessageHandler(typeof(GameFightOptionStateUpdateMessage))]
        public void HandleGameFightOptionStateUpdateMessage(Bot bot, GameFightOptionStateUpdateMessage message)
        {
            if (bot == null || bot.Character == null || bot.Character.Fight == null)
            {
                logger.Error("Fight is not properly initialized.");
                return; // Can't handle the message
            }
            bot.Character.Fight.Update(message);
        }

        [MessageHandler(typeof(GameFightShowFighterMessage))]
        public void HandleGameFightShowFighterMessage(Bot bot, GameFightShowFighterMessage message)
        {
            if (bot == null || bot.Character == null || bot.Character.Fight == null)
            {
                logger.Error("Fight is not properly initialized.");
                return; // Can't handle the message
            }
            bot.Character.Fight.AddActor(message.informations);
        }

        [MessageHandler(typeof(GameFightRefreshFighterMessage))]
        public void HandleGameFightRefreshFighterMessage(Bot bot, GameFightRefreshFighterMessage message)
        {
            if (bot == null || bot.Character == null || bot.Character.Fight == null)
            {
                logger.Error("Fight is not properly initialized.");
                return; // Can't handle the message
            }
            bot.Character.Fight.Update(message);

        }

        [MessageHandler(typeof(GameFightLeaveMessage))]
        public void HandleGameFightLeaveMessage(Bot bot, GameFightLeaveMessage message)
        {
        }

        [MessageHandler(typeof(GameEntitiesDispositionMessage))]
        public void HandleGameEntitiesDispositionMessage(Bot bot, GameEntitiesDispositionMessage message)
        {
            if (bot == null || bot.Character == null || bot.Character.Fight == null)
            {
                logger.Error("Fight is not properly initialized.");
                return; // Can't handle the message
            }
            bot.Character.Fight.Update(message);
        }

        [MessageHandler(typeof(GameFightUpdateTeamMessage))]
        public void HandleGameFightUpdateTeamMessage(Bot bot, GameFightUpdateTeamMessage message)
        {
            if (bot == null || bot.Character == null || bot.Character.Fight == null)
            {
                logger.Error("Fight is not properly initialized.");
                return; // Can't handle the message
            }
            if (bot.Character.Fight.Id == message.fightId)
                bot.Character.Fight.Update(message);
        }

        [MessageHandler(typeof(GameFightRemoveTeamMemberMessage))]
        public void HandleGameFightRemoveTeamMemberMessage(Bot bot, GameFightRemoveTeamMemberMessage message)
        {
            if (bot == null || bot.Character == null || bot.Character.Fight == null)
            {
                logger.Error("Fight is not properly initialized.");
                return; // Can't handle the message
            }
            if (bot.Character.Fight.Id == message.fightId)
                bot.Character.Fight.RemoveActor(message.charId);
        }

        [MessageHandler(typeof(GameFightHumanReadyStateMessage))]
        public void HandleGameFightHumanReadyStateMessage(Bot bot, GameFightHumanReadyStateMessage message)
        {
            if (bot == null || bot.Character == null || bot.Character.Fight == null)
            {
                logger.Error("Fight is not properly initialized.");
                return; // Can't handle the message
            }
            bot.Character.Fight.Update(message);

        }

        [MessageHandler(typeof (GameFightStartMessage))]
        public void HandleGameFightStartMessage(Bot bot, GameFightStartMessage message)
        {
            if (bot == null || bot.Character == null || bot.Character.Fight == null)
            {
                logger.Error("Fight is not properly initialized.");
                return; // Can't handle the message
            }
            bot.Character.Fight.StartFight();            
        }

        [MessageHandler(typeof(GameFightEndMessage))]
        public void HandleGameFightEndMessage(Bot bot, GameFightEndMessage message)
        {
            if (bot == null || bot.Character == null || bot.Character.Fight == null)
            {
                logger.Error("Fight is not properly initialized.");
                return; // Can't handle the message
            }
            bot.Character.Fight.EndFight(message);
            bot.Character.LeaveFight();
        }

        [MessageHandler(typeof(GameFightSynchronizeMessage))]
        public void HandleGameFightSynchronizeMessage(Bot bot, GameFightSynchronizeMessage message)
        {
            if (bot == null || bot.Character == null || bot.Character.Fight == null)
            {
                logger.Error("Fight is not properly initialized.");
                return; // Can't handle the message
            }
            bot.Character.Fight.Update(message);
        }

        [MessageHandler(typeof(GameFightNewRoundMessage))]
        public void HandleGameFightNewRoundMessage(Bot bot, GameFightNewRoundMessage message)
        {
            if (bot == null || bot.Character == null || bot.Character.Fight == null)
            {
                logger.Error("Fight is not properly initialized.");
                return; // Can't handle the message
            }
            bot.Character.Fight.SetRound(message.roundNumber);
        }

        [MessageHandler(typeof(GameFightTurnStartMessage))]
        public void HandleGameFightTurnStartMessage(Bot bot, GameFightTurnStartMessage message)
        {
            if (bot == null || bot.Character == null || bot.Character.Fight == null)
            {
                logger.Error("Fight is not properly initialized.");
                return; // Can't handle the message
            }
            bot.Character.Fight.StartTurn(message.id);
        }

        [MessageHandler(typeof(GameFightTurnListMessage))]
        public void HandleGameFightTurnListMessage(Bot bot, GameFightTurnListMessage message)
        {
            if (bot == null || bot.Character == null || bot.Character.Fight == null)
            {
                logger.Error("Fight is not properly initialized.");
                return; // Can't handle the message
            }
            bot.Character.Fight.Update(message);
        }

        [MessageHandler(typeof(SequenceEndMessage))]
        public void HandleSequenceEndMessage(Bot bot, SequenceEndMessage message)
        {
            if (bot == null || bot.Character == null || bot.Character.Fight == null)
            {
                logger.Error("Fight is not properly initialized.");
                return; // Can't handle the message
            }            
            bot.Character.Fight.EndSequence(message);
        }
        
        [MessageHandler(typeof(GameFightTurnEndMessage))]
        public void HandleGameFightTurnEndMessage(Bot bot, GameFightTurnEndMessage message)
        {
            if (bot == null || bot.Character == null || bot.Character.Fight == null)
            {
                logger.Error("Fight is not properly initialized.");
                return; // Can't handle the message
            }
            bot.Character.Fight.EndTurn();
            bot.Character.SpellsBook.EndTurn();
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

        [MessageHandler(typeof (GameActionFightSummonMessage))]
        public void HandleGameActionFightSummonMessage(Bot bot, GameActionFightSummonMessage message)
        {
            if (bot == null || bot.Character == null || bot.Character.Fight == null)
            {
                logger.Error("Fight is not properly initialized.");
                return; // Can't handle the message
            }
            bot.Character.Fight.AddActor(message.summon);
        }

        #region stats update
        

        [MessageHandler(typeof(GameActionFightLifePointsLostMessage))]
        public void HandleGameActionFightLifePointsLostMessage(Bot bot, GameActionFightLifePointsLostMessage message)
        {
            if (bot == null || bot.Character == null || bot.Character.Fight == null)
            {
                logger.Error("Fight is not properly initialized.");
                return; // Can't handle the message
            }
            var fighter = bot.Character.Fight.GetFighter(message.targetId);

            if (fighter == null)
                logger.Error("Fighter {0} has lost {2} HP cast but doesn't exist, or is it {1} ?", message.targetId, message.sourceId, message.loss);
            else
            {
                fighter.UpdateHP(message);
            }
        }

        [MessageHandler(typeof(GameActionFightLifeAndShieldPointsLostMessage))]
        public void HandleGameActionFightLifeAndShieldPointsLostMessage(Bot bot, GameActionFightLifeAndShieldPointsLostMessage message)
        {
            HandleGameActionFightLifePointsLostMessage(bot, message);
        }

        [MessageHandler(typeof(GameActionFightLifePointsGainMessage))]
        public void HandleGameActionFightLifePointsGainMessage(Bot bot, GameActionFightLifePointsGainMessage message)
        {
            if (bot == null || bot.Character == null || bot.Character.Fight == null)
            {
                logger.Error("Fight is not properly initialized.");
                return; // Can't handle the message
            }
            var fighter = bot.Character.Fight.GetFighter(message.targetId);

            if (fighter == null)
                logger.Error("Fighter {0} has gain {2} HP cast but doesn't exist, or is it {1} ?", message.targetId, message.sourceId, message.delta);
            else
            {
                fighter.UpdateHP(message);
            }            
        }

        [MessageHandler(typeof(GameActionFightPointsVariationMessage))]
        public void HandleGameActionFightPointsVariationMessage(Bot bot, GameActionFightPointsVariationMessage message)
        {
            if (bot == null || bot.Character == null || bot.Character.Fight == null)
            {
                logger.Error("Fight is not properly initialized.");
                return; // Can't handle the message
            }
            var fighter = bot.Character.Fight.GetFighter(message.targetId);

            if (fighter == null)
                logger.Error("Fighter {0} has lost {2} ?P points but doesn't exist, or is it {1} ?", message.sourceId, message.targetId, -message.delta);
            else
            {
                fighter.Update(message);
            }            
        }
        
        
        [MessageHandler(typeof(GameActionFightTackledMessage))]
        public void HandleGameActionFightTackledMessage(Bot bot, GameActionFightTackledMessage message)
        {            
        }
 
        [MessageHandler(typeof(GameActionAcknowledgementMessage))]
        public void HandleGameActionAcknowledgementMessage(Bot bot, GameActionAcknowledgementMessage message)
        {
            if (bot == null || bot.Character == null || bot.Character.Fight == null)
            {
                logger.Error("Fight is not properly initialized.");
                return; // Can't handle the message
            }
            PlayedFighter fighter = bot.Character.Fighter;

            if (fighter != null)
            {
                fighter.Update(message);
            }            
        }

        [MessageHandler(typeof(GameActionFightDeathMessage))]
        public void HandleGameActionFightDeathMessage(Bot bot, GameActionFightDeathMessage message)
        {
            if (bot == null || bot.Character == null || bot.Character.Fight == null)
            {
                logger.Error("Fight is not properly initialized.");
                return; // Can't handle the message
            }
            bot.Character.Fight.Update(message);            
        }

        #endregion stats update

    }
}