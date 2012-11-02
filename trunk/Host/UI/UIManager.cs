using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AvalonDock.Layout;
using BiM.Core.Collections;
using BiM.Core.Reflection;
using BiM.Host.UI.ViewModels;
using BiM.Host.UI.Views;

namespace BiM.Host.UI
{
    public class UIManager : DockContainer<MainWindow>, INotifyPropertyChanged
    {
        private static readonly UIManager m_instance = new UIManager();

        public static UIManager Instance
        {
            get { return m_instance; }
        }

        public UIManager()
        {
            Application.Current.MainWindow.Initialized += OnInitialized;
        }

        private void OnInitialized(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
                              {
                                  Host.Initialize();
                                  Host.Start();
                              });
        }


        public BotViewModel GetBotViewModel(Behaviors.Bot bot)
        {
            lock (DocumentPane.Children)
            {
                var document = DocumentPane.Children.FirstOrDefault(x => x.Content is BotControl && ( x.Content as BotControl ).ViewModel.Bot == bot);

                return document != null ? ( document.Content as BotControl ).ViewModel : null;
            }
        }

        public BotViewModel[] GetBotsViewModel()
        {
            lock (DocumentPane.Children)
            {
                return DocumentPane.Children.Where(x => x.Content is BotViewModel).
                    Select(x => (x.Content as BotControl).ViewModel).ToArray();
            }
        }

        protected override LayoutDocumentPane DocumentPane
        {
            get { return View.DocumentPane; }
        }
    }
}