#region License GNU GPL
// ChatView.xaml.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion
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