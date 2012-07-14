using System.ComponentModel;

namespace BiM.Behaviors.Game.Items
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