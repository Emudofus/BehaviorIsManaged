using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using BiM.Host.UI.Views;

namespace BiM.Host.UI.ViewModels
{
    public interface IViewModel<T> : INotifyPropertyChanged, IViewModel
        where T : IView
    {
        T View
        {
            get;
            set;
        }
    }


    public interface IViewModel
    {
        object View
        {
            get;
            set;
        }
    }
}