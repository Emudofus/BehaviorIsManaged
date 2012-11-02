using System.Windows.Threading;
using BiM.Host.UI.ViewModels;

namespace BiM.Host.UI.Views
{
    public interface IView<T> : IView
        where T : IViewModel
    {
        T ViewModel
        {
            get;
            set;
        }
    }

    public interface IView
    {
        object ViewModel
        {
            get;
            set;
        }

        Dispatcher Dispatcher
        {
            get;
        }
    }
}