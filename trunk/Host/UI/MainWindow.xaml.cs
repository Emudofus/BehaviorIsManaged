using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BiM.Behaviors;
using BiM.Host.UI.Bot;
using BiM.Host.UI.MDI;

namespace BiM.Host
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

            BotManager.Instance.BotAdded += OnBotAdded;
            BotManager.Instance.BotRemoved += OnBotRemoved;

            Host.Start();
        }

        private void OnBotAdded(BotManager manager, Bot bot)
        {
            Dispatcher.Invoke(new Action(() => AddChild(bot)));
        }


        private void OnBotRemoved(BotManager manager, Bot bot)
        {
            Dispatcher.Invoke(new Action(() => RemoveChild(bot)));
        }

        public void AddChild(Bot bot)
        {
            var child = new MdiChild();
            child.Title = "Bot";
            child.Content = new BotControl(bot);

            MdiContainer.Children.Add(child);
        }

        public void RemoveChild(Bot bot)
        {
            var childs = MdiContainer.Children.
                Where(entry => entry.Content is BotControl && ( entry.Content as BotControl ).Bot == bot).ToArray();

            foreach (var child in childs)
            {
                child.Close();
            }
        }
    }
}
