#region License GNU GPL
// GatherTest.cs
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
using BiM.Behaviors;
using BiM.Behaviors.Frames;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Behaviors.Game.Interactives;
using BiM.Core.Messages;
using BiM.Core.Threading;
using BiM.Protocol.Messages;

namespace SimplePlugin.Handlers
{
    public class GatherTestRegister
    {
        [MessageHandler(typeof(ChatClientMultiMessage))]
        public static void HandleChatMessage(Bot bot, ChatClientMultiMessage message)
        {
            if (message.content == ".gather on")
            {
                message.BlockNetworkSend();// do not send this message to the server

                if (!bot.Character.Jobs.Any(x => x.JobTemplate.id == GatherTest.FARMER_JOB))
                {
                    bot.Character.SendMessage("You haven't the farmer job");
                }
                else
                {
                    bot.AddFrame(new GatherTest(bot));
                    bot.Character.SendMessage("Gathering ...");
                }
            }
            else if (message.content == ".gather off")
            {
                message.BlockNetworkSend();// do not send this message to the server


                bot.RemoveFrame<GatherTest>();
                bot.Character.SendMessage("Stop gathering ...");

            }
        }
    }

    public class GatherTest : Frame<GatherTest>
    {
        public const int GatherTickTime = 5000;

        public const int WHEATH_INTERACTIVE = 38;
        public const int FARMER_JOB = 28;
        public const int SCYTHE_ITEM = 577;

        private SimplerTimer m_timeoutGatherTimer;
        private int m_lastTriedInteractive;

        public bool m_stopped;

        public GatherTest(Bot bot)
            : base(bot)
        {

        }

        public override void OnAttached()
        {
            Bot.Character.StopUsingInteractive += OnStopUsingInteractive;
            Bot.Character.StartUsingInteractive += OnStartUsingInteractive;
            CheckRessources();
        }

        public override void OnDetached()
        {
            m_stopped = true;
            Bot.Character.StopUsingInteractive -= OnStopUsingInteractive;
            Bot.Character.StartUsingInteractive -= OnStartUsingInteractive;
        }

        private void CheckRessources()
        {
            if (m_stopped)
                return;

            DisposeTimer();

            if (Bot.Character.IsFighting() || Bot.Character.IsUsingInteractive() || Bot.Character.IsMoving())
            {
                Bot.CallDelayed(GatherTickTime, CheckRessources);
                return;
            }

            var ressource = Bot.Character.Map.Interactives.Where(x => x.Type != null && x.Type.id == WHEATH_INTERACTIVE && x.Cell != null &&
                x.EnabledSkills.Count > 0 &&
                x.State == InteractiveState.None && x.Id != m_lastTriedInteractive).
                OrderBy(x => x.Cell.ManhattanDistanceTo(Bot.Character.Cell)).FirstOrDefault();

            if (ressource == null)
            {
                // should change map here
                Bot.CallDelayed(GatherTickTime, CheckRessources);
                return;
            }

            // avoid being stucked
            m_lastTriedInteractive = ressource.Id;

            var item = Bot.Character.Inventory.GetItemByTemplate(SCYTHE_ITEM);
            if (item == null)
            {
                Bot.Character.SendMessage("You haven't the farmer tool");
                Bot.RemoveFrame<GatherTest>();
                Bot.Character.SendMessage("Stop gathering ...");
                return;
            }

            if (!item.IsEquipped)
            {
                if (!Bot.Character.Inventory.Equip(item))
                {
                    Bot.Character.SendMessage("Cannot equip the farmer tool");
                    // todo : determin why ?
                    Bot.CallDelayed(GatherTickTime, CheckRessources);
                }
            }
            else // try to gather the ressource the next tick else the tool wouldn't be equipped
            {
                var skill = ressource.EnabledSkills.FirstOrDefault();

                if (skill == null)
                    return;

                if (!Bot.Character.UseInteractiveObject(skill))
                    Bot.CallDelayed(GatherTickTime, CheckRessources);
                else
                    m_timeoutGatherTimer = Bot.CallDelayed(4000, InteractiveUseTimeout);
            }
        }

        private void OnStartUsingInteractive(RolePlayActor actor, InteractiveObject interactive, InteractiveSkill skill, DateTime? usageEndTime)
        {
            DisposeTimer();
        }

        private void OnStopUsingInteractive(RolePlayActor actor, InteractiveObject interactive, InteractiveSkill skill)
        {
            CheckRessources();
        }

        private void InteractiveUseTimeout()
        {
            // timeout
            if (!Bot.Character.IsUsingInteractive())
                CheckRessources();
        }

        private void DisposeTimer()
        {
            if (m_timeoutGatherTimer != null)
                m_timeoutGatherTimer.Dispose();
        }

    }
}