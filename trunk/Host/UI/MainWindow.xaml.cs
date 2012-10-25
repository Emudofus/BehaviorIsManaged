using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using AvalonDock.Layout;
using BiM.Behaviors;
using BiM.Core.Messages;
using BiM.Host.UI.Bot;
using BiM.Protocol.Messages;
using NLog;

namespace BiM.Host.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private object m_locker = new object();
        // temp
        private static bool m_initialized = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            Task.Factory.StartNew(InitializeHost);

            base.OnInitialized(e);
        }

        private void InitializeHost()
        {
            Host.Initialize();
            MessageDispatcher.RegisterSharedContainer(this);

            BotManager.Instance.BotAdded += OnBotAdded;
            BotManager.Instance.BotRemoved += OnBotRemoved;

            Host.Start();
        }

        private void OnBotAdded(BotManager manager, Behaviors.Bot bot)
        {
            Dispatcher.Invoke(new Action(() => AddChild(bot)));
        }

        private void OnBotRemoved(BotManager manager, Behaviors.Bot bot)
        {
            // Dispatcher.Invoke(new Action(() => RemoveChild(bot)));
            // note : remove the window only if another bot with the same name is created
        }

        [MessageHandler(typeof(IdentificationMessage))]
        public void HandleIdentificationMessage(Behaviors.Bot bot, IdentificationMessage message)
        {
            Dispatcher.Invoke(new Action(() => CloseChildByLogin(message.login)));
        }

        public BotControl GetBotControl(Behaviors.Bot bot)
        {
            lock (m_locker)
            {
                return BotsPane.Children.Select(x => x.Content).
                    FirstOrDefault(x => x is BotControl && (x as BotControl).Bot == bot) as BotControl;
            }
        }

        public BotControl AddChild(Behaviors.Bot bot)
        {
            var control = new BotControl(bot)
            {
                DataContext = bot,
            };
            var document = new LayoutDocument()
            {
                Content = control,
            };

            document.Closed += OnBotFormClosed;

            lock (m_locker)
            {
                BotsPane.Children.Add(document);
            }

            return control;
        }

        public bool RemoveChild(Behaviors.Bot bot)
        {
            lock (m_locker)
            {
                var childs = BotsPane.Children.
                    Where(entry => entry.Content is BotControl && (entry.Content as BotControl).Bot == bot).ToArray();

                bool removed = false;

                foreach (var child in childs)
                {
                    child.Content = null;
                    child.Close();
                    removed = true;
                }

                return removed;
            }
        }

    
        private void OnBotFormClosed(object sender, EventArgs e)
        {
            var child = (LayoutDocument)sender;

            if (child.Content != null)
            {
                ( (BotControl)child.Content ).Bot.Dispose();
            }
        }

        private void CloseChildByLogin(string login)
        {
            lock (m_locker)
            {
                foreach (var child in BotsPane.Children.ToArray())
                {
                    if (child.Content == null)
                        child.Close();

                    else if (child.Content is BotControl)
                    {
                        var childBot = ((BotControl) child.Content).Bot;

                        if (childBot.Disposed && (childBot.ClientInformations == null || string.IsNullOrEmpty(childBot.ClientInformations.Login)))
                            child.Close();
                        else if (childBot.ClientInformations != null && childBot.ClientInformations.Login == login.ToLower())
                        {
                            if (!childBot.Disposed)
                                // wtf cannot log it ?!
                                ; //logger.Error("A bot with the same login ({0}) is already opened and not disposed !");
                            else
                            {
                                child.Close();
                            }
                        }
                    }
                }
            }
        }
    }
}
