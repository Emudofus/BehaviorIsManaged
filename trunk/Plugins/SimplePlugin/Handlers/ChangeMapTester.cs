using System;
using BiM.Behaviors;
using BiM.Behaviors.Game.World;
using BiM.Core.Messages;
using BiM.Protocol.Messages;

namespace SimplePlugin.Handlers
{
    public class ChangeMapTester
    {
        [MessageHandler(typeof(ChatClientMultiMessage))]
        public static void HandleChatMessage(Bot bot, ChatClientMultiMessage message)
        {
            if (message.content.StartsWith(".move"))
            {
                message.BlockNetworkSend();// do not send this message to the server

                var splits = message.content.Split(' ');
                if (splits.Length != 2)
                {
                    bot.Character.SendMessage("syntax : .move [Right/Left/Top/Bottom]");
                    return;
                }

                MapNeighbour neighbour;
                try
                {
                    neighbour = (MapNeighbour)Enum.Parse(typeof(MapNeighbour), splits[1]);
                }
                catch (Exception)
                {
                    bot.Character.SendMessage("syntax : .move [Right/Left/Top/Bottom]");
                    return;
                }


                if (!bot.Character.ChangeMap(neighbour))
                {
                    bot.Character.SendMessage(string.Format("Cannot move to {0} !", neighbour));
                }
            }
        }
    }
}