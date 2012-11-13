#region License GNU GPL
// ConnectionHandler.cs
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
using BiM.Core.Messages;
using BiM.Protocol.Messages;
using NLog;

namespace BiM.Behaviors.Handlers.Connection
{
    public class ConnectionHandler
    {
        [MessageHandler(typeof(HelloConnectMessage))]
        public static void HandleHelloConnectMessage(Bot bot, HelloConnectMessage message)
        {
            bot.ClientInformations.Update(message);
        }

        [MessageHandler(typeof(IdentificationMessage))]
        public static void HandleIdentificationMessage(Bot bot, IdentificationMessage message)
        {
            bot.ClientInformations.Update(message);
        }

        [MessageHandler(typeof (IdentificationFailedMessage))]
        public static void HandleIdentificationFailedMessage(Bot bot, IdentificationFailedMessage message)
        {
            bot.ClientInformations.Update(message);
        }

        [MessageHandler(typeof(IdentificationFailedBannedMessage))]
        public static void HandleIdentificationFailedBannedMessage(Bot bot, IdentificationFailedBannedMessage message)
        {
            bot.ClientInformations.Update(message);
        }

        [MessageHandler(typeof(IdentificationSuccessMessage))]
        public static void HandleIdentificationSuccessMessage(Bot bot, IdentificationSuccessMessage message)
        {
            bot.ClientInformations.Update(message);
        }

        [MessageHandler(typeof(IdentificationFailedForBadVersionMessage))]
        public static void HandleIdentificationFailedForBadVersionMessage(Bot bot, IdentificationFailedForBadVersionMessage message)
        {
            bot.ClientInformations.Update(message);
        }

        [MessageHandler(typeof(SelectedServerDataMessage))]
        public static void HandleSelectedServerDataMessage(Bot bot, SelectedServerDataMessage message)
        {
            bot.ClientInformations.Update(message);
        }

        [MessageHandler(typeof(ServersListMessage))]
        public static void HandleServersListMessage(Bot bot, ServersListMessage message)
        {
            bot.ClientInformations.Update(message);
            bot.Display = DisplayState.ServerSelection;
        }

        [MessageHandler(typeof(AccountCapabilitiesMessage))]
        public static void HandleAccountCapabilitiesMessage(Bot bot, AccountCapabilitiesMessage message)
        {
            bot.ClientInformations.Update(message);
        }
    }
}