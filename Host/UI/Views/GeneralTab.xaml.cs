#region License GNU GPL
// GeneralTab.xaml.cs
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
using System.Windows.Media;
using BiM.Host.UI.ViewModels;

namespace BiM.Host.UI.Views
{
    /// <summary>
    /// Interaction logic for GeneralTab.xaml
    /// </summary>
    public partial class GeneralTab : UserControl, IView<GeneralTabViewModel>
    {
        //private int m_lineCount;

        public GeneralTab()
        {
            InitializeComponent();
        }

        public int MaxLines
        {
            get;
            set;
        }

        #region IView<GeneralTabViewModel> Members

        public GeneralTabViewModel ViewModel
        {
            get;
            set;
        }

        object IView.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (GeneralTabViewModel) value; }
        }

        #endregion

        public void AppendText(string text)
        {
            var tr = new TextRange(RichTextBox.Document.ContentEnd, RichTextBox.Document.ContentEnd)
                         {
                             Text = text
                         };
        }

        public void AppendText(string text, Color foreColor)
        {
            var tr = new TextRange(RichTextBox.Document.ContentEnd, RichTextBox.Document.ContentEnd)
                         {
                             Text = text
                         };

            tr.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(foreColor));
        }

        public void AppendText(string text, Color foreColor, Color backColor)
        {
            var tr = new TextRange(RichTextBox.Document.ContentEnd, RichTextBox.Document.ContentEnd)
                         {
                             Text = text
                         };

            tr.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(foreColor));
            tr.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(backColor));
        }
    }
}