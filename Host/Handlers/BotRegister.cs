#region License GNU GPL
// BotRegister.cs
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
using System.Windows;
using BiM.Behaviors;
using BiM.Behaviors.Messages;
using BiM.Core.Messages;
using BiM.Host.UI;
using BiM.Host.UI.ViewModels;
using BiM.Host.UI.Views;
using BiM.Protocol.Messages;

namespace BiM.Host.Handlers
{
    public class BotRegister
    {
        [MessageHandler(typeof (BotAddedMessage))]
        public static void HandleBotAddedMessage(object dummy, BotAddedMessage message)
        {
            var model = new BotViewModel(message.Bot);
            var layout = UIManager.Instance.AddDocument(model, () => new BotControl());
            layout.Title = "Bot #" + message.Bot.Id;

            layout = model.AddDocument(new GeneralTabViewModel(message.Bot), () => new GeneralTab());
            layout.Title = "General";
        }

        [MessageHandler(typeof (BotRemovedMessage))]
        public static void HandleBotRemovedMessage(object dummy, BotRemovedMessage message)
        {
            var models = UIManager.Instance.GetBotsViewModel();
            var matching = models.FirstOrDefault(x => x.Bot == message.Bot);

            if (matching != null)
                matching.Dispose();
        }
    }
}