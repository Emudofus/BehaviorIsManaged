#region License GNU GPL
// TchatCommand.cs
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
