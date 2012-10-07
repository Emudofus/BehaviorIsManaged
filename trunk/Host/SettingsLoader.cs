using System;
using System.Linq;
using BiM.Behaviors;
using BiM.Core.Config;
using BiM.Core.Messages;
using BiM.Protocol.Messages;

namespace BiM.Host
{
    public static class SettingsLoader
    {
        static SettingsLoader()
        {
            BotManager.Instance.BotRemoved += OnBotRemoved;
        }

        public const string AccountPlaceHolder = "?account?";
        public const string CharacterPlaceHolder = "?character?";

        [Configurable("SettingsFile", "Path of settings files, " + 
            AccountPlaceHolder + " is replaced by the account name and "+
            CharacterPlaceHolder + " by the character name. You must use them !")]

        public readonly static string SettingsFile = "./settings/" + AccountPlaceHolder + "/" + CharacterPlaceHolder + ".xml";

        [MessageHandler(typeof(CharacterSelectedSuccessMessage))]
        public static void HandleCharacterSelectedSuccessMessage(Bot bot, CharacterSelectedSuccessMessage message)
        {
            if (!SettingsFile.Contains(AccountPlaceHolder) || !SettingsFile.Contains(CharacterPlaceHolder))
            {
                throw new Exception(string.Format("Configurable entry 'SettingsFile' must contains {0} and {1} to differentiate bot settings",
                    AccountPlaceHolder, CharacterPlaceHolder));
            }

            var settingsPath = SettingsFile.Replace(AccountPlaceHolder, bot.ClientInformations.Login).
                Replace(CharacterPlaceHolder, bot.Character.Name);

            bot.LoadSettings(settingsPath);
        }

        public static void OnBotRemoved(BotManager botManager, Bot bot)
        {
            bot.SaveSettings();
        }
    }
}