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
using BiM.Core.Messages;
using BiM.Host.UI;
using BiM.Host.UI.ViewModels;
using BiM.Host.UI.Views;
using BiM.Protocol.Messages;

namespace BiM.Host.Handlers
{
    public class BotRegister
    {
        [MessageHandler(typeof (IdentificationMessage))]
        public static void HandleIdentificationMessage(Bot bot, IdentificationMessage message)
        {
            var models = UIManager.Instance.GetBotsViewModel();
            var matching = models.FirstOrDefault(x => x.Bot != null && 
                x.Bot.ClientInformations != null && 
                x.Bot.ClientInformations.Login.Equals(message.login, StringComparison.OrdinalIgnoreCase));


            if (matching != null)
            {
                matching.Bot.Dispose();
                matching.Dispose();
            }

            var model = new BotViewModel(bot);
            var layout = UIManager.Instance.AddDocument(model, () => new BotControl());
            layout.Title = bot.ClientInformations.Login;

            layout = model.AddDocument(new GeneralTabViewModel(bot), () => new GeneralTab());
            layout.Title = "General";
        } 
    }
}