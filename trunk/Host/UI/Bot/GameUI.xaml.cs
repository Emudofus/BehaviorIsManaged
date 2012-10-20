using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BiM.Behaviors.Game.Actors.RolePlay;

namespace BiM.Host.UI.Bot
{
    /// <summary>
    /// Interaction logic for GameUI.xaml
    /// </summary>
    public partial class GameUI : UserControl
    {
        public GameUI(Behaviors.Bot bot)
        {
            Bot = bot;
            InitializeComponent();
        }

        public Behaviors.Bot Bot
        {
            get;
            private set;
        }

        public PlayedCharacter Character
        {
            get { return Bot.Character; }
        }
    }
}
