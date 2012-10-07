using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BiM.Behaviors;
using BiM.Core.Messages;
using BiM.Host.UI.Bot;
using BiM.Host.UI.MDI;
using BiM.Protocol.Messages;
using NLog;

namespace BiM.Host.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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

        private void CloseChildByLogin(string login)
        {
            foreach (var child in MdiContainer.Children.ToArray())
            {
                if (child.Content == null)
                    child.Close();

                else if (child.Content is BotControl)
                {
                    var childBot = ( (BotControl)child.Content ).Bot;

                    if (childBot.Disposed && ( childBot.ClientInformations == null || string.IsNullOrEmpty(childBot.ClientInformations.Login) ))
                        child.Close();
                    else if (childBot.ClientInformations != null && childBot.ClientInformations.Login == login.ToLower())
                    {
                        if (!childBot.Disposed)
                            // wtf cannot log it ?!
                            ;//logger.Error("A bot with the same login ({0}) is already opened and not disposed !");
                        else
                        {
                            child.Close();
                        }
                    }
                }
            }
        }

        public void AddChild(Behaviors.Bot bot)
        {
            var child = new MdiChild();
            child.Title = "Bot";
            child.Content = new BotControl(bot);

            child.Closed += OnChildClosed;

            MdiContainer.Children.Add(child);
        }

        private void OnChildClosed(object sender, RoutedEventArgs e)
        {
            var child = (MdiChild)sender;

            if (child.Content != null)
            {
                ( (BotControl)child.Content ).Bot.Dispose();
            }
        }

        public void RemoveChild(Behaviors.Bot bot)
        {
            var childs = MdiContainer.Children.
                Where(entry => entry.Content is BotControl && ( entry.Content as BotControl ).Bot == bot).ToArray();

            foreach (var child in childs)
            {
                child.Content = null;
                child.Close();
            }
        }
    }
}
