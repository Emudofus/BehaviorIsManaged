#region License GNU GPL
// SettingsLoader.cs
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