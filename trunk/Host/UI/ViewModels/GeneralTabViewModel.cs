using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Documents;
using System.Windows.Media;
using BiM.Behaviors;
using BiM.Host.UI.Views;
using NLog;

namespace BiM.Host.UI.ViewModels
{
    public class GeneralTabViewModel : IViewModel<GeneralTab>
    {
        private static Dictionary<LogLevel, Color> m_colorsRules = new Dictionary<LogLevel, Color>()
        {
            {LogLevel.Debug, Colors.Gray},
            {LogLevel.Trace, Colors.DarkGray},
            {LogLevel.Info, Colors.Black},
            {LogLevel.Warn, Colors.Orange},
            {LogLevel.Error, Colors.Red},
            {LogLevel.Fatal, Colors.DarkRed},
        };

        public GeneralTabViewModel(Bot bot)
        {
            bot.LogNotified += OnLog;
        }

        public GeneralTab View
        {
            get;
            set;
        }

        private void OnLog(Bot bot, LogLevel level, string caller, string message)
        {
            var color = m_colorsRules[level];

            View.Dispatcher.BeginInvoke(new Action(() => View.AppendText(string.Format("[{0}] {1} : {2}\n", DateTime.Now.ToString("hh:mm:ss"), caller, message), color)));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        object IViewModel.View
        {
            get { return View; }
            set { View = (GeneralTab)value; }
        }
    }
}