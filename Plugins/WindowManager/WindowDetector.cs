#region License GNU GPL
// WindowDetector.cs
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using BiM.Behaviors;
using BiM.Behaviors.Frames;
using BiM.Behaviors.Messages;
using BiM.Core.Messages;
using BiM.MITM.Network;
using BiM.Protocol.Messages;
using NLog;
using WindowManager.Api32;

namespace WindowManager
{
    public static class WindowDetectorRegister
    {
        [MessageHandler(typeof(CharacterSelectedSuccessMessage))]
        public static void OnCharacterSelectedSuccessMessage(Bot bot, CharacterSelectedSuccessMessage message)
        {
            if (bot.Dispatcher is NetworkMessageDispatcher)
                bot.AddFrame(new WindowDetector(bot));
        } 
    }

    public class WindowDetector : Frame<WindowDetector>
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private const int DelayToCloseWindow = 1000;

        private Process m_botProcess;

        public WindowDetector(Bot bot)
            : base (bot)
        {
            try
            {
                m_botProcess = ClientFinder.GetProcessUsingPort(( (IPEndPoint)( (ConnectionMITM)( (NetworkMessageDispatcher)bot.Dispatcher ).Client ).Socket.RemoteEndPoint ).Port);
            }
            catch
            {
                logger.Error("Cannot find dofus process. WindowDetector disabled");
                Bot.RemoveFrame(this);
            }

            if (m_botProcess == null)
            {
                logger.Error("Cannot find dofus process. WindowDetector disabled");
                Bot.RemoveFrame(this);
            }
        }

        /// <summary>
        /// This is used to avoid inactivity inhibition of the client. 
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="message"></param>
        [MessageHandler(typeof(GameFightTurnEndMessage))]
        private void HandleGameFightTurnEndMessage(Bot bot, GameFightTurnEndMessage message)
        {
            if (bot.Character != null && bot.Character.Id == message.id)
                CloseWindow();
        }

        [MessageHandler(typeof(GameFightEndMessage))]
        private void HandleGameFightEndMessage(Bot bot, GameFightEndMessage message)
        {
            CloseWindow();
        }

        [MessageHandler(typeof (CharacterLevelUpInformationMessage))]
        private void HandleCharacterLevelUpInformationMessage(Bot bot, CharacterLevelUpInformationMessage message)
        {
            CloseWindow();
        }

        [MessageHandler(typeof (JobLevelUpMessage))]
        public void HandleJobLevelUpMessage(Bot bot, JobLevelUpMessage message)
        {
            CloseWindow();
        }

        public Process ClientProcess
        {
            get { return m_botProcess; }
        }

        public void CloseWindow(int delay = DelayToCloseWindow)
        {
            Bot.CallDelayed(DelayToCloseWindow, InternalCloseWindow);
        }

        public void SendKey(Keys key)
        {
            User32Wrapper.SendKey(m_botProcess.MainWindowHandle, key);
        }

        public void SendChar(char c)
        {
            User32Wrapper.SendChar(m_botProcess.MainWindowHandle, c);
        }

        private void InternalCloseWindow()
        {
            User32Wrapper.SendKey(m_botProcess.MainWindowHandle, Keys.Enter);
        }

    }
}