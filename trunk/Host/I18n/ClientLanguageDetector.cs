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