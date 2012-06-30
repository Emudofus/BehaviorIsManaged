using System.ComponentModel;
using BiM.Core.IO;

namespace BiM.Protocol.Tools.Ele.Datas
{
    public class EntityGraphicalElementData : EleGraphicalData, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public EntityGraphicalElementData(EleInstance instance, int id)
            : base(instance, id)
        {
        }

        public string EntityLook
        {
            get;
            set;
        }

        public bool HorizontalSymmetry
        {
            get;
            set;
        }

        public bool PlayAnimation
        {
            get;
            set;
        }

        public bool PlayAnimStatic
        {
            get;
            set;
        }

        public uint MinDelay
        {
            get;
            set;
        }

        public uint MaxDelay
        {
            get;
            set;
        }

        public override EleGraphicalElementTypes Type
        {
            get { return EleGraphicalElementTypes.ENTITY; }
        }

        public static EntityGraphicalElementData ReadFromStream(EleInstance instance, int id, BigEndianReader reader)
        {
            var data = new EntityGraphicalElementData(instance, id);

            data.EntityLook = reader.ReadUTF7BitLength();
            data.HorizontalSymmetry = reader.ReadBoolean();

            if (instance.Version >= 7)
            {
                data.PlayAnimation = reader.ReadBoolean();
            }

            if (instance.Version >= 6)
            {
                data.PlayAnimStatic = reader.ReadBoolean();
            }

            if (instance.Version >= 5)
            {
                data.MinDelay = reader.ReadUInt();
                data.MaxDelay = reader.ReadUInt();
            }

            return data;
        }
    }
}