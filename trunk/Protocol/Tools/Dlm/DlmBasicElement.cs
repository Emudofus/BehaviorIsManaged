using System;
using BiM.Core.IO;

namespace BiM.Protocol.Tools.Dlm
{
    public abstract class DlmBasicElement
    {
        public enum ElementTypesEnum
        {
            Graphical = 2,
            Sound = 33
        }

        protected DlmBasicElement(DlmCell cell)
        {
            Cell = cell;
        }

        public DlmCell Cell
        {
            get;
            private set;
        }

        public static DlmBasicElement ReadFromStream(DlmCell cell, BigEndianReader reader)
        {
            var type = reader.ReadByte();

            switch ((ElementTypesEnum) type)
            {
                case ElementTypesEnum.Graphical:
                    return DlmGraphicalElement.ReadFromStream(cell, reader);

                case ElementTypesEnum.Sound:
                    return DlmSoundElement.ReadFromStream(cell, reader);

                default:
                    throw new Exception("Unknown element ID : " + type + " CellID : " + cell.Id);
            }
        }  
    }
}