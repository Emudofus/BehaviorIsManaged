using System.ComponentModel;
using BiM.Core.IO;

namespace BiM.Protocol.Tools.Ele.Datas
{
    public class AnimatedGraphicalElementData : NormalGraphicalElementData, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public AnimatedGraphicalElementData(EleInstance instance, int id) : base(instance, id)
        {
        }

        public override EleGraphicalElementTypes Type
        {
            get
            {
                return EleGraphicalElementTypes.ANIMATED;
            }
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

        public static new AnimatedGraphicalElementData ReadFromStream(EleInstance instance, int id, BigEndianReader reader)
        {
            var data = new AnimatedGraphicalElementData(instance, id);

            data.Gfx = reader.ReadInt();
            data.Height = reader.ReadUInt();
            data.HorizontalSymmetry = reader.ReadBoolean();
            data.Origin = new System.Drawing.Point(reader.ReadShort(), reader.ReadShort());
            data.Size = new System.Drawing.Point(reader.ReadShort(), reader.ReadShort());

            if (instance.Version == 4)
            {
                data.MinDelay = reader.ReadUInt();
                data.MaxDelay = reader.ReadUInt();
            }

            return data;
        }
    }
}