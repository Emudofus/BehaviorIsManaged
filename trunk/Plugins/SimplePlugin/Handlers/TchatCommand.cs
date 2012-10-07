using System;
using BiM.Behaviors;

namespace SimplePlugin.Handlers
{
    public class TchatCommand
    {
        private Action<string[], Bot> m_action;
        public Action<string[], Bot> Action
        {
            get { return m_action; }
        }

        private string m_name;
        public string Name
        {
            get { return m_name; }
        }

        private string m_help;
        public string Help
        {
            get { return m_help; }
        }

        private string m_commandName;
        public string CommandName
        {
            get { return m_commandName; }
        }

        public TchatCommand(string name, string help, Action<string[], Bot> action)
        {
            m_name = name;
            m_commandName = name.ToLowerInvariant();
            m_help = help;
            m_action = action;
        }
    }
}
