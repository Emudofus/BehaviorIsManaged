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
            UIManager.Instance.AddDocument(model);
        } 
    }
}