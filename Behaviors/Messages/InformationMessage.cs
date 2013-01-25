using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BiM.Core.Messages;

namespace BiM.Behaviors.Messages
{
    public class InformationMessage : Message
    {
        public string Message { get; private set; }
        public InformationMessage(string message)
        {
            Message = message;
        }
        static public void SendInformationMessage(Bot bot, string message, params object[] args)
        {
            bot.SendLocal(new InformationMessage(string.Format(message, args)));
        }
    }
}
