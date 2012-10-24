using System;
using System.Windows;
using System.Windows.Controls;
using BiM.Host.UI.Bot;

namespace BiM.Host.UI
{
    public static class UIManager
    {
        public static MainWindow MainWindow
        {
            get
            {
                return (MainWindow)Application.Current.MainWindow;
            }
        }

        /*public static void AddMdiChild(Control control)
        {
            MainWindow.Dispatcher.Invoke(new Action(() => MainWindow.AddChild(control)));
        }

        public static void RemoveMdiChild(Control control)
        {
            MainWindow.Dispatcher.Invoke(new Action(() => MainWindow.RemoveChild(control)));
        }*/

        public static void AddMenu(MenuItem menu)
        {
            MainWindow.Dispatcher.Invoke(new Action(() => MainWindow.Menu.Items.Add(menu)));
        }

        public static void AddMenu(object menu)
        {
            MainWindow.Dispatcher.Invoke(new Action(() => MainWindow.Menu.Items.Add(menu)));
        }

        public static void RemoveMenu(object menu)
        {
            MainWindow.Dispatcher.Invoke(new Action(() => MainWindow.Menu.Items.Remove(menu)));
        }

        public static BotControl GetBotUI(Behaviors.Bot bot)
        {
            return MainWindow.GetBotControl(bot);
        }

    }
}