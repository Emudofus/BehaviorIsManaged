using System.ComponentModel;
using BiM.Core.IO;

namespace BiM.Protocol.Tools.D2p
{
    public class D2pProperty : INotifyPropertyChanged
    {
        public D2pProperty()
        {
            
        }

        public D2pProperty(string key, string property)
        {
            Key = key;
            Value = property;
        }

        public string Key
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        public void ReadProperty(IDataReader reader)
        {
            Key = reader.ReadUTF();
            Value = reader.ReadUTF();
        }

        public void WriteProperty(IDataWriter writer)
        {
            writer.WriteUTF(Key);
            writer.WriteUTF(Value);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}