#region License GNU GPL
// ClientLanguageDetector.cs
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
using System.Collections.Generic;
using BiM.Behaviors;
using BiM.Core.I18n;
using BiM.Core.Messages;
using BiM.Protocol.Messages;

namespace BiM.Host.I18n
{
    public class ClientLanguageDetector
    {
        private static Dictionary<string, Languages> m_langsShortcuts = new Dictionary<string, Languages>()
        {
            {"fr", Languages.French},
            {"de", Languages.German},
            {"en", Languages.English},
            {"es", Languages.Spanish},
            {"it", Languages.Italian},
            {"ja", Languages.Japanish},
            {"nl", Languages.Dutsh},
            {"pt", Languages.Portugese},
            {"ru", Languages.Russish},
        };

        private static bool m_languageFound;

        [MessageHandler(typeof(AuthenticationTicketMessage))]
        public static void HandleAuthenticationTicketMessage(Bot bot, AuthenticationTicketMessage message)
        {
            if (m_languageFound)
                return;

            Languages lang;
            if (!m_langsShortcuts.TryGetValue(message.lang, out lang))
                return;

            Host.ChangeLanguage(lang);
            m_languageFound = true;
        }
    }
}