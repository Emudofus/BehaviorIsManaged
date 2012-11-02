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
        private int m_lineCount;

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