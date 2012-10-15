using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
using BiM.Behaviors;
using BiM.Behaviors.Messages;
using BiM.Core.Messages;
using BiM.MITM.Network;
using BiM.Protocol.Messages;
using WindowManager.Api32;

namespace WindowManager
{
    public static class WindowDetectorRegister
    {
        [MessageHandler(typeof(CharacterSelectedSuccessMessage))]
        public static void OnCharacterSelectedSuccessMessage(Bot bot, CharacterSelectedSuccessMessage message)
        {
            if (bot.Dispatcher is NetworkMessageDispatcher)
                bot.AddHandler(new WindowDetector(bot));
        } 
    }

    public class WindowDetector
    {
        private readonly Bot m_bot;
        private const int DelayToCloseWindow = 1000;

        private Process m_botProcess;

        public WindowDetector(Bot bot)
        {
            m_bot = bot;
            m_botProcess = ClientFinder.GetProcessUsingPort(( (IPEndPoint) ( (ConnectionMITM)( (NetworkMessageDispatcher)bot.Dispatcher ).Client ).Socket.RemoteEndPoint ).Port);
        }

        [MessageHandler(typeof(GameFightEndMessage))]
        public void HandleGameFightEndMessage(Bot bot, GameFightEndMessage message)
        {
            CloseWindow();
        }

        [MessageHandler(typeof (CharacterLevelUpInformationMessage))]
        public void HandleCharacterLevelUpInformationMessage(Bot bot, CharacterLevelUpInformationMessage message)
        {
            CloseWindow();
        }

        public Process ClientProcess
        {
            get { return m_botProcess; }
        }

        public void CloseWindow(int delay = DelayToCloseWindow)
        {
            m_bot.CallDelayed(DelayToCloseWindow, InternalCloseWindow);
        }

        private void InternalCloseWindow()
        {
            User32Wrapper.SendKey(m_botProcess.MainWindowHandle, Keys.Enter);
        }
    }
}