using System.ComponentModel;
using BiM.Core.IO;

namespace BiM.Protocol.Tools.Ele.Datas
{
    public class BlendedGraphicalElementData : NormalGraphicalElementData, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public BlendedGraphicalElementData(EleInstance instance, int id)
            : base(instance, id)
        {
        }

        public string BlendMode
        {
            get;
            set;
        }

        public override EleGraphicalElementTypes Type
        {
            get { return EleGraphicalElementTypes.BLENDED; }
        }

        public static new BlendedGraphicalElementData ReadFromStream(EleInstance instance, int id, BigEndianReader reader)
        {
            var data = new BlendedGraphicalElementData(instance, id);

            data.Gfx = reader.ReadInt();
            data.Height = reader.ReadUInt();
            data.HorizontalSymmetry = reader.ReadBoolean();
            data.Origin = new System.Drawing.Point(reader.ReadShort(), reader.ReadShort());
            data.Size = new System.Drawing.Point(reader.ReadShort(), reader.ReadShort());

            data.BlendMode = reader.ReadUTF7BitLength();

            return data;
        }
    }
}