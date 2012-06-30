using System;
using System.ComponentModel;
using BiM.Core.IO;
using BiM.Protocol.Tools.Ele.Datas;

namespace BiM.Protocol.Tools.Ele
{
    public abstract class EleGraphicalData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public EleGraphicalData(EleInstance instance, int id)
        {
            Instance = instance;
            Id = id;
        }

        public EleInstance Instance
        {
            get;
            set;
        }

        public int Id
        {
            get;
            set;
        }

        public abstract EleGraphicalElementTypes Type
        {
            get;
        }

        public static EleGraphicalData ReadFromStream(EleInstance instance, BigEndianReader reader)
        {
            var id = reader.ReadInt();
            var type = (EleGraphicalElementTypes)reader.ReadByte();

            switch (type)
            {
                   case EleGraphicalElementTypes.ANIMATED:
                    return AnimatedGraphicalElementData.ReadFromStream(instance, id, reader);
                   case EleGraphicalElementTypes.BLENDED:
                    return BlendedGraphicalElementData.ReadFromStream(instance, id, reader);
                   case EleGraphicalElementTypes.BOUNDING_BOX:
                    return BoundingBoxGraphicalElementData.ReadFromStream(instance, id, reader);
                   case EleGraphicalElementTypes.ENTITY:
                    return EntityGraphicalElementData.ReadFromStream(instance, id, reader);
                   case EleGraphicalElementTypes.NORMAL:
                    return NormalGraphicalElementData.ReadFromStream(instance, id, reader);
                   case EleGraphicalElementTypes.PARTICLES:
                    return ParticlesGraphicalElementData.ReadFromStream(instance, id, reader);
                default:
                    throw new Exception("Unknown graphical data of type " + type);
            }
        }
    }
}