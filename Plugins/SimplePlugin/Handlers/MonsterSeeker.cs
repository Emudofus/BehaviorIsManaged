#region License GNU GPL
// MonsterSeeker.cs
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BiM.Behaviors;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Core.Config;
using BiM.Core.Messages;
using BiM.Protocol.Messages;

namespace SimplePlugin.Handlers
{
    public class MonsterSeeker
    {
        [Configurable("AllowSeeker")]
        public static bool AllowSeeker = false;

        public static bool SeekArchiMonster = true;

        private static List<string> m_seeked = new List<string>();

        [MessageHandler(typeof(MapComplementaryInformationsDataMessage))]
        public static void HandleMapMessage(Bot bot, MapComplementaryInformationsDataMessage message)
        {
            if (!AllowSeeker) return;

            GroupMonster[] groups = bot.Character.Map.Actors.OfType<GroupMonster>().ToArray();
            var alreadySignaled = new List<string>();
            foreach (GroupMonster @group in groups)
            {
                string monster = SeekedInGroup(@group, bot);
                if (!String.IsNullOrEmpty(monster))
                {
                    if (!alreadySignaled.Contains(monster))
                    {
                        alreadySignaled.Add(monster);

                        bot.Character.SendMessage(String.Format("Le monstre <b>'{0}'</b> a été trouvé sur cette carte.", monster), System.Drawing.Color.Green);
                    }
                }
                string archi = SeekedArchiMonsterInGroup(@group, bot);
                if (!String.IsNullOrEmpty(archi))
                {
                    bot.Character.SendMessage(String.Format("L'archimonstre <b>'{0}'</b> a été trouvé sur cette carte.", archi), System.Drawing.Color.Red);
                }
            }
        }

        private static string SeekedInGroup(GroupMonster group, Bot bot)
        {
            if (group == null) return null;

            string[] monsters = group.Monsters.Where(entry => IsSeeking(entry.Name.ToLowerInvariant())).Select(entry => entry.Name).ToArray();
            if (monsters.Length == 0) return null;
            
            return monsters.FirstOrDefault();
        }

        private static string SeekedArchiMonsterInGroup(GroupMonster group, Bot bot)
        {
            if (group == null) return null;
            if (!SeekArchiMonster) return null;

            string[] monsters = group.Monsters.Where(entry => entry.IsArchMonster).Select(entry => entry.Name).ToArray();
            if (monsters.Length == 0) return null;

            return monsters.Aggregate((a,b) => a + ", " + b);
        }

        internal static ReadOnlyCollection<string> SeekedMonsters
        {
            get { return m_seeked.AsReadOnly(); }
        }

        internal static bool AddMonster(string name)
        {
            if (m_seeked.Contains(name))
                return false;
            m_seeked.Add(name);
            return true;
        }

        internal static bool RemoveMonster(string name)
        {
            if (!m_seeked.Contains(name))
                return false;
            m_seeked.Remove(name);
            return true;
        }

        internal static bool IsSeeking(string name)
        {
            return m_seeked.Contains(name);
        }

        internal static void HandleSeekCommand(string[] parameters, Bot bot)
        {
            if (parameters.Length != 1)
            {
                bot.Character.SendMessage("Invalid use. Use help command for more informations.", System.Drawing.Color.Firebrick);
                return;
            }
            if (MonsterSeeker.AddMonster(parameters[0].ToLowerInvariant()))
                bot.Character.SendMessage(String.Format("Le monstre '{0}' est désormais recherché.", parameters[0]), System.Drawing.Color.Green);
            else
                bot.Character.SendMessage(String.Format("Le monstre '{0}' est déjà recherché.", parameters[0]), System.Drawing.Color.DarkSlateGray);
        }

        internal static void HandleStopCommand(string[] parameters, Bot bot)
        {
            if (parameters.Length != 1)
            {
                bot.Character.SendMessage("Invalid use. Use help command for more informations.", System.Drawing.Color.Firebrick);
                return;
            }
            if (MonsterSeeker.RemoveMonster(parameters[0].ToLowerInvariant()))
                bot.Character.SendMessage(String.Format("Le monstre '{0}' n'est désormais plus recherché.", parameters[0]), System.Drawing.Color.Green);
            else
                bot.Character.SendMessage(String.Format("Le monstre '{0}' n'était pas recherché.", parameters[0]), System.Drawing.Color.DarkSlateGray);
        }

        internal static void HandleListCommand(string[] parameters, Bot bot)
        {
            if (MonsterSeeker.SeekedMonsters.Count > 0)
                bot.Character.SendMessage(String.Format("Les monstres suivants sont recherchés :\n{0}", MonsterSeeker.SeekedMonsters.Aggregate((a, b) => a + ", " + b)), System.Drawing.Color.DarkSlateGray);
            else
                bot.Character.SendMessage("Aucun monstre n'est recherché.", System.Drawing.Color.DarkSlateGray);
        }
    }
}
