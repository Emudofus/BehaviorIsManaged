using System.Drawing;
using BiM.Behaviors;
using BiM.Behaviors.Game.Actors.Fighters;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Behaviors.Game.Fights;
using BiM.Behaviors.Messages;
using BiM.Core.Messages;
using BiM.Protocol.Messages;

namespace FightPlugin
{
    internal static class WelcomeMessageRegister
    {
        [MessageHandler(typeof(ChatClientMultiMessage))]
        public static void HandleChatMessage(Bot bot, ChatClientMultiMessage message)
        {
            // if the client sends ".hello" in the chat
            if (message.content == ".fight on")
            {
                message.BlockNetworkSend();// do not send this message to the server

                bot.AddHandler(new AutoFight());
                bot.Character.SendMessage("Auto fight started");
            }
            else if (message.content == ".fight off")
            {
                message.BlockNetworkSend();// do not send this message to the server


                bot.RemoveHandler<AutoFight>();
                bot.Character.SendMessage("Auto fight stopped");

            }
        }
    }


    public class AutoFight
    {
        public AutoFight()
        {
            var bot = BotManager.Instance.GetCurrentBot();

            if (bot != null)
            {
                bot.Character.FightJoined += OnFightJoined;
                bot.Character.FightLeft += OnFightLeft;

                if (bot.Character.IsFighting())
                    OnFightJoined(bot.Character, bot.Character.Fight);
            }
        }

        [MessageHandler(typeof(CharacterSelectedSuccessMessage))]
        public void HandleCharacterSelectedSuccessMessage(Bot bot, CharacterSelectedSuccessMessage message)
        {
            bot.Character.FightJoined += OnFightJoined;
            bot.Character.FightLeft += OnFightLeft;
        }

        private void OnFightJoined(PlayedCharacter character, Fight fight)
        {
            character.Fighter.TurnStarted += OnTurnStarted;
        }

        private void OnFightLeft(PlayedCharacter character, Fight fight)
        {
            character.Fighter.TurnStarted -= OnTurnStarted;
        }

        private void OnTurnStarted(Fighter fighter)
        {
            var bot = BotManager.Instance.GetCurrentBot();

            StartIA(fighter as PlayedFighter);
        }

        private void StartIA(PlayedFighter fighter)
        {
            var nearestMonster = GetNearestEnnemy(fighter);
            var shortcut = fighter.Character.SpellShortcuts.Get(1);

            if (shortcut == null)
            {
                fighter.Character.SendMessage("No spell on slot 1");
            }
            else
            {
                fighter.CastSpell(shortcut.GetSpell(), nearestMonster.Cell);
            }
        }

        private Fighter GetNearestEnnemy(PlayedFighter fighter)
        {
            var ennemyTeam = fighter.Fight.GetTeam(fighter.Team.Id == FightTeamColor.Blue ? FightTeamColor.Red : FightTeamColor.Blue);

            Fighter nearestFighter = null;
            foreach (var ennemy in ennemyTeam.Fighters)
            {
                if (nearestFighter == null)
                    nearestFighter = ennemy;

                else if (fighter.Cell.DistanceTo(ennemy.Cell) < nearestFighter.Cell.DistanceTo(fighter.Cell))
                {
                    nearestFighter = ennemy;
                }
            }

            return nearestFighter;
        }
    }
}