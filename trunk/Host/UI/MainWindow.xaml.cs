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

        protected override void OnActivated(EventArgs e)
        {
            // temp
            if (!Program.Initialized && !m_initialized)
            {
                m_initialized = true;
                Task.Factory.StartNew(
                    () =>
                        {
                            Program.Initialize();
                            Program.Start();
                        });
            }

            base.OnActivated(e);
        }

        protected override void OnInitialized(EventArgs e)
        {            
            

            base.OnInitialized(e);
        }
    }
}
