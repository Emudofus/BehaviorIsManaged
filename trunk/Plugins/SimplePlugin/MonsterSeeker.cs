using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BiM.Behaviors;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Core.Config;
using BiM.Core.Messages;
using BiM.Protocol.Messages;

namespace SimplePlugin
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

            GroupMonster[] groups = bot.Character.Map.Actors.Where(entry => entry is GroupMonster).Select(entry => entry as GroupMonster).ToArray();
            List<string> alreadySignaled = new List<string>();
            for (int i = 0; i < groups.Length; i++)
            {
                string monster = SeekedInGroup(groups[i], bot);
                if (!String.IsNullOrEmpty(monster))
                {
                    if (!alreadySignaled.Contains(monster))
                    {
                        alreadySignaled.Add(monster);
                        
                        bot.ChatManager.PrintMessageToClient(String.Format("Le monstre <b>'{0}'</b> a été trouvé sur cette carte.", monster), System.Drawing.Color.Green);
                    }
                }
                string archi = SeekedArchiMonsterInGroup(groups[i], bot);
                if (!String.IsNullOrEmpty(archi))
                {
                    bot.ChatManager.PrintMessageToClient(String.Format("L'archimonstre <b>'{0}'</b> a été trouvé sur cette carte.", archi), System.Drawing.Color.Red);
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
                bot.ChatManager.PrintMessageToClient("Invalid use. Use help command for more informations.", System.Drawing.Color.Firebrick);
                return;
            }
            if (MonsterSeeker.AddMonster(parameters[0].ToLowerInvariant()))
                bot.ChatManager.PrintMessageToClient(String.Format("Le monstre '{0}' est désormais recherché.", parameters[0]), System.Drawing.Color.Green);
            else
                bot.ChatManager.PrintMessageToClient(String.Format("Le monstre '{0}' est déjà recherché.", parameters[0]), System.Drawing.Color.DarkSlateGray);
        }

        internal static void HandleStopCommand(string[] parameters, Bot bot)
        {
            if (parameters.Length != 1)
            {
                bot.ChatManager.PrintMessageToClient("Invalid use. Use help command for more informations.", System.Drawing.Color.Firebrick);
                return;
            }
            if (MonsterSeeker.RemoveMonster(parameters[0].ToLowerInvariant()))
                bot.ChatManager.PrintMessageToClient(String.Format("Le monstre '{0}' n'est désormais plus recherché.", parameters[0]), System.Drawing.Color.Green);
            else
                bot.ChatManager.PrintMessageToClient(String.Format("Le monstre '{0}' n'était pas recherché.", parameters[0]), System.Drawing.Color.DarkSlateGray);
        }

        internal static void HandleListCommand(string[] parameters, Bot bot)
        {
            if (MonsterSeeker.SeekedMonsters.Count > 0)
                bot.ChatManager.PrintMessageToClient(String.Format("Les monstres suivants sont recherchés :\n{0}", MonsterSeeker.SeekedMonsters.Aggregate((a, b) => a + ", " + b)), System.Drawing.Color.DarkSlateGray);
            else
                bot.ChatManager.PrintMessageToClient("Aucun monstre n'est recherché.", System.Drawing.Color.DarkSlateGray);
        }
    }
}
