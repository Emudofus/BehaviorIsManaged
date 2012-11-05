using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using BiM.Host.UI.Views;

namespace BasicPlugin.Chat
{
    /// <summary>
    /// Interaction logic for ChatView.xaml
    /// </summary>
    public partial class ChatView : UserControl, IView<ChatViewModel>
    {
        private ChatViewModel m_viewModel;

        public ChatView()
        {
            InitializeComponent();
        }

        #region IView<ChatViewModel> Members

        object IView.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ChatViewModel) value; }
        }

        public ChatViewModel ViewModel
        {
            get { return m_viewModel; }
            set
            {
                m_viewModel = value;
                DataContext = null;
                DataContext = value;
            }
        }

        #endregion

        private void OnTextKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ViewModel.SendTextCommand.Execute(null);
        }

        public void AppendText(string text)
        {
            var tr = new TextRange(ChatTextBox.Document.ContentEnd, ChatTextBox.Document.ContentEnd)
            {
                Text = text
            };
        }

        public void AppendText(string text, Color foreColor)
        {
            var tr = new TextRange(ChatTextBox.Document.ContentEnd, ChatTextBox.Document.ContentEnd)
            {
                Text = text
            };

            tr.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(foreColor));
        }

        public void AppendText(string text, Color foreColor, Color backColor)
        {
            var tr = new TextRange(ChatTextBox.Document.ContentEnd, ChatTextBox.Document.ContentEnd)
            {
                Text = text
            };

            tr.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(foreColor));
            tr.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(backColor));
        }
    }
}