using System.ComponentModel;
using BiM.Protocol.Enums;

namespace BiM.Game.Chat
{
    public abstract class ChatMessage : INotifyPropertyChanged
    {
        // note : I have to encapsulate this protocol part because ChatAbstractServerMessage and ChatAbstractClientMessage
        // are not bound, and this is not good

        public string Content
        {
            get;
            set;
        }

        public ChatActivableChannelsEnum Channel
        {
            get;
            set;
        }

        // todo
        public object Items
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}