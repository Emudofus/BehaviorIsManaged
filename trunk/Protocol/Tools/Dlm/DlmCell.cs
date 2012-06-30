using System.ComponentModel;
using BiM.Core.IO;

namespace BiM.Protocol.Tools.Dlm
{
    public class DlmCell : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public DlmCell(DlmLayer layer)
        {
            Layer = layer;
        }

        private DlmLayer m_layer;

        public DlmLayer Layer
        {
            get { return m_layer; }
            set { m_layer = value; }
        }

        public short Id
        {
            get;
            set;
        }

        public DlmBasicElement[] Elements
        {
            get;
            set;
        }

        public static DlmCell ReadFromStream(DlmLayer layer, BigEndianReader reader)
        {
            var cell = new DlmCell(layer);

            cell.Id = reader.ReadShort();
            cell.Elements = new DlmBasicElement[reader.ReadShort()];

            for (int i = 0; i < cell.Elements.Length; i++)
            {
                DlmBasicElement element =
                    DlmBasicElement.ReadFromStream(cell, reader);
                cell.Elements[i] = element;
            }

            return cell;
        }
    }
}