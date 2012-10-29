using System.Windows.Media;

namespace BiM.Host.UI.ViewModels
{
    public class PaneViewModel : ViewModelBase
    {
        public string Title
        {
            get;
            protected set;
        }

        public ImageSource IconSource
        {
            get;
            protected set;
        }

        public string ContentId
        {
            get;
            protected set;
        }

        public bool IsSelected
        {
            get;
            protected set;
        }

        public bool IsActive
        {
            get;
            protected set;
        }
    }
}