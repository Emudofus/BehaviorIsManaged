using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.Config;
using BiM.Core.Messages;
using BiM.Protocol.Messages;
using BiM.Behaviors;

namespace SimplePlugin
{
    public static class GameCommands
    {
        private static Bot m_bot = null;
        private static List<TchatCommand> m_commands = new List<TchatCommand>();

        [Configurable("CharCommand")]
        private static char m_charCommand = '#';

        internal static void OnHelpCommand(string[] parameters, Bot bot)
        {
            foreach (string line in GetHelpCommands())
                bot.ChatManager.PrintMessageToClient(line, System.Drawing.Color.MidnightBlue);
        }

        public static void CreateTchatCommand(string name, string help, Action<string[], Bot> action)
        {
            string commandName = name.ToLowerInvariant();
            if (m_commands.Count(entry => entry.CommandName == commandName) == 1)
                throw new InvalidOperationException(String.Format("Command {0} already exists.", name));
            m_commands.Add(new TchatCommand(name, help, action));
        }

        public static string[] GetHelpCommands()
        {
            List<string> sb = new List<string>();

            sb.Add("<b><u>Help commands :</u></b>");
            foreach (TchatCommand cmd in m_commands)
                sb.Add(GetHelpCommand(cmd.CommandName));

            return sb.ToArray();
        }

        public static string GetHelpCommand(string commandName)
        {
            TchatCommand cmd = m_commands.Where(entry => entry.CommandName == commandName).FirstOrDefault();
            if (cmd == null)
                return String.Format("Command <b>{0}</b> doesn't exist.", commandName);

            return String.Format("<b>{0}</b> : {1}", cmd.Name, cmd.Help);
        }


        [MessageHandler(typeof(ChatClientMultiMessage))]
        public static void HandleChatMessage(Bot bot, ChatClientMultiMessage message)
        {
            if (m_bot == null)
            {
                m_bot = bot;
                CreateTchatCommand("Help", "help", OnHelpCommand);
            }

            if (message.content[0] == m_charCommand)
                message.BlockNetworkSend();
            else
                return;

            string[] parts = message.content.Substring(1).Split(' ');
            string commandName = parts[0].ToLowerInvariant();
            List<string> parameters = new List<string>(parts.Length - 1);
            for (int i = 1; i < parts.Length; i++)
            {
                if (parts[i].StartsWith("\""))
                {

                    string paramInQuote = parts[i].Substring(1);
                    i++;
                    while (!parts[i].EndsWith("\""))
                    {
                        paramInQuote += " " + parts[i];
                        i++;
                    }
                    paramInQuote += " " + parts[i].Substring(0, parts[i].Length - 1);
                    parameters.Add(paramInQuote);
                }
                else
                    parameters.Add(parts[i]);
            }

            if (m_commands.Count(entry => entry.CommandName == commandName) == 1)
                m_commands.Where(entry => entry.CommandName == commandName).First().Action(parameters.ToArray(), bot);
            else
                bot.ChatManager.PrintMessageToClient(String.Format("The tchat command <b>{0}</b> doesn't exist.", commandName), System.Drawing.Color.Red);
        }
    }
}
