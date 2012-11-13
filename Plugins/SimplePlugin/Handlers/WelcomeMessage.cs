#region License GNU GPL
// WelcomeMessage.cs
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
using BiM.Behaviors;
using BiM.Behaviors.Frames;
using BiM.Behaviors.Messages;
using BiM.Core.Messages;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;

namespace SimplePlugin.Handlers
{
    internal static class WelcomeMessageRegister
    {
        [MessageHandler(typeof(BotAddedMessage))]
        public static void OnBotAdded(object sender, BotAddedMessage message)
        {
            message.Bot.AddFrame(new WelcomeMessage(message.Bot));
        } 
    }

    public class WelcomeMessage : Frame<WelcomeMessage>
    {
        private bool m_messageSent;

        public WelcomeMessage(Bot bot)
            : base(bot)
        {
        }

        [MessageHandler(typeof(GameContextCreateMessage))]
        public void HandleGameContextCreateMessage(Bot bot, GameContextCreateMessage message)
        {
            if (!m_messageSent)
            {
                var settings = bot.Settings.GetEntry<Settings>();

                bot.Character.SendMessage(settings.WelcomeMessage);

                if (settings.WelcomeMessage.EndsWith("!"))
                    settings.WelcomeMessage = settings.WelcomeMessage.Remove(settings.WelcomeMessage.Length - 1, 1);
                else
                    settings.WelcomeMessage += "!";

                m_messageSent = true;
            }
        }
    }
}