using System.Drawing;
using BiM.Behaviors;
using BiM.Behaviors.Game.Actors.Fighters;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Behaviors.Game.Fights;
using BiM.Core.Messages;
using BiM.Protocol.Messages;

namespace FightPlugin
{
    public static class AutoFight
    {
        [MessageHandler(typeof(CharacterSelectedSuccessMessage))]
        public static void HandleCharacterSelectedSuccessMessage(Bot bot, CharacterSelectedSuccessMessage message)
        {
            bot.Character.FightJoined += OnFightJoined;
            bot.Character.FightLeft += OnFightLeft;
        }

        private static void OnFightJoined(PlayedCharacter character, Fight fight)
        {
            character.Fighter.TurnStarted += OnTurnStarted;
        }

        private static void OnFightLeft(PlayedCharacter character, Fight fight)
        {
            character.Fighter.TurnStarted -= OnTurnStarted;
        }

        private static void OnTurnStarted(Fighter fighter)
        {
            var bot = BotManager.Instance.GetCurrentBot();

            StartIA(fighter as PlayedFighter);
        }

        private static void StartIA(PlayedFighter fighter)
        {
            var nearestMonster = GetNearestEnnemy(fighter);
            var shortcut = fighter.Character.SpellShortcuts.Get(1);

            if (shortcut == null)
            {
                fighter.Character.SendMessage("No spell on slot 1");
            }
            else
                fighter.CastSpell(shortcut.GetSpell(), nearestMonster.Cell);
        }

        private static Fighter GetNearestEnnemy(PlayedFighter fighter)
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