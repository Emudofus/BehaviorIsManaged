using System.ComponentModel;

namespace BiM.Game.Items
{
    public abstract class ItemBase : INotifyPropertyChanged
    {
        public int Guid
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}