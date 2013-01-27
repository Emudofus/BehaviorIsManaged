#region License GNU GPL
// AutoFight.cs
// 
// Copyright (C) 2012, 2013 - BehaviorIsManaged
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

// Author : FastFrench - antispam@laposte.net
#endregion
using System;
using BiM.Behaviors;
using BiM.Behaviors.Data.D2O;
using BiM.Behaviors.Frames;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Core.Messages;
using BiM.Protocol.Messages;

namespace zFFFightPlugin
{
  partial class FFight
  {
    static void SetFrame(Bot bot, Mode mode)
    {
      if (bot.HasFrame<FFight>())
      {
        bot.Character.SendInformation("Set existing FFight to {0} mode", mode);
        bot.GetFrame<FFight>().Mode = mode;
      }
      else
        if (bot.AddFrame(new FFight(bot, mode)))
          bot.Character.SendInformation("Experimental AI fight started in {0} mode", mode);
        else
          bot.Character.SendInformation("Failed to start a new FFight frame !");

    }

    [MessageHandler(typeof(ChatClientMultiMessage))]
    public static void HandleChatMessage(Bot bot, ChatClientMultiMessage message)
    {
      if (message.content == ".help")
      {
        message.BlockNetworkSend();// do not send this message to the server                
        bot.Character.SendInformation(".dump spells");
        bot.Character.SendInformation(".dump all");
        bot.Character.SendInformation(".FF ? => Show all plugins running");
        bot.Character.SendInformation(".FF on or .FF auto => Starts experimental AI fight in automatic mode");
        bot.Character.SendInformation(".FF fol => Put the experimental AI fight in follower mode");
        bot.Character.SendInformation(".FF gat => Put the experimental AI fight in gathering mode (not implemented yet)");
        bot.Character.SendInformation(".FF off or .FF man => Disable experimental AI fight (manual mode)");
        bot.Character.SendInformation(".FF stats => gives some stats");
        bot.Character.SendInformation(".FF all => Starts experimental AI fight on all Bots");
        bot.Character.SendInformation(".FF /all => Stops experimental AI fight on all Bots");
        bot.Character.SendInformation("message <Level> => Filters the messages received from the bot to the Dofus client. <Level> is a bit field (4 bits, so values range from 0 to 7)");

      }
      else if (message.content == ".dump all")
      {
        message.BlockNetworkSend();// do not send this message to the server                
        XmlDumper.DumpAll();
      }
      else if (message.content == ".dump spells")
      {
        message.BlockNetworkSend();// do not send this message to the server                
        XmlDumper.SpellsDumper("_Spells.xml");
      }

      if (message.content.StartsWith(".FF"))
      {
        message.BlockNetworkSend();// do not send this message to the server                
        if (message.content == ".FF ?")
        {
          int BotNo = 0;
          int FrameNo = 0;
          foreach (Bot subBot in BotManager.Instance.Bots)
          {
            BotNo++;
            foreach (IFrame frame in subBot.Frames)
            {
              FrameNo++;
              bot.Character.SendInformation("Bot {0} ({3}) Frame {1} : {2}", BotNo, FrameNo, frame.GetType().Name, subBot.Character);
            }
          }

        }
        else
          if (message.content == ".FF all")
          {
            bot.Character.SendInformation("Experimental AI fight started for all played characters (set to follower mode for non-leaders of parties)");
            foreach (Bot subBot in BotManager.Instance.Bots)
            {
              if (subBot.AddFrame(new FFight(subBot)))
              {
                subBot.Character.SendInformation("Experimental AI fight started");
                bot.Character.SendInformation("FF started for {0}", bot.Character);
              }
              else
              {
                subBot.Character.SendInformation("Can't start FF");
                bot.Character.SendInformation("Can't start FF for {0}", bot.Character);
              }

            }
          }
          else if (message.content == ".FF /all")
          {
            bot.Character.SendInformation("Experimental AI fight stopped for all played characters (set to manual mode)");
            foreach (Bot subBot in BotManager.Instance.Bots)
            {
              if (subBot.RemoveFrame<FFight>())
              {
                subBot.Character.SendInformation("Experimental AI fight stopped");
                bot.Character.SendInformation("FF stopped for {0}", bot.Character);
              }
              else
              {
                subBot.Character.SendInformation("Failed to stop Experimental AI fight. Probably not running ?");
                bot.Character.SendInformation("Can't stop FF for {0}", bot.Character);
              }
            }
          }
          else if (message.content.StartsWith(".FF fol", StringComparison.InvariantCultureIgnoreCase))
            SetFrame(bot, Mode.Follower);
          else if (message.content.StartsWith(".FF gat", StringComparison.InvariantCultureIgnoreCase))
            SetFrame(bot, Mode.Ressources);
          else if (message.content.StartsWith(".FF auto", StringComparison.InvariantCultureIgnoreCase) || message.content.StartsWith(".FF on", StringComparison.InvariantCultureIgnoreCase))
            SetFrame(bot, Mode.AutomaticFight);
          else if (message.content.StartsWith(".FF man", StringComparison.InvariantCultureIgnoreCase) || message.content.StartsWith(".FF off", StringComparison.InvariantCultureIgnoreCase))
            SetFrame(bot, Mode.Manual);
          else if (message.content == ".FF stats")
          {
            if (!bot.HasFrame<FFight>())
            {
              bot.Character.SendInformation("Experimental AI fight is NOT running");
            }
            else
            {
              FFight fightBot = bot.GetFrame<FFight>();
              bot.Character.SendInformation("Experimental AI fight IS running in mode {0}", fightBot.Mode);
              fightBot.Dump();
            }
          }
      }

      PlayedCharacter PC = bot.Character;
      if (message.content == "?")
      {
        message.BlockNetworkSend();// do not send this message to the server
        PC.SendInformation(String.Format("Position : NF{0} - F{1}", PC.Cell, PC.Fighter != null ? PC.Fighter.Cell.ToString() : "N/A"));
        /*PC.ResetCellsHighlight();
        if (PC.Fighter != null)
        {
            PC.HighlightCells(PC.Fight.BlueTeam.FightersAlive.Select(fighter => fighter.Cell), Color.Blue);
            PC.HighlightCells(PC.Fight.RedTeam.FightersAlive.Select(fighter => fighter.Cell), Color.Red);
            PC.HighlightCell(PC.Fighter.Cell, Color.Pink);
        }
        else
            PC.HighlightCell(PC.Cell, Color.Pink);*/
      }
      if (message.content.StartsWith("message"))
      {
        message.BlockNetworkSend();// do not send this message to the server
        string sdbgLevel = message.content.Replace("message", "").Trim();
        PlayedCharacter.MessageLevel dbgLevel = PC.InformationLevel;
        if (PlayedCharacter.MessageLevel.TryParse(sdbgLevel, out dbgLevel))
        {
          PC.SendMessage(String.Format("MessageLevel was {0}, it is now {1}", PC.InformationLevel, dbgLevel));
          PC.InformationLevel = dbgLevel;
        }
        else
          PC.SendMessage(String.Format("MessageLevel is {0}", PC.InformationLevel));
      }

    }
  }
}
