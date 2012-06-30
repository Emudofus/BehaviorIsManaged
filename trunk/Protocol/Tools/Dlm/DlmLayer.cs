using System.ComponentModel;
using BiM.Core.IO;

namespace BiM.Protocol.Tools.Dlm
{
    public class DlmLayer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public DlmLayer(DlmMap map)
        {
            Map = map;
        }

        public DlmMap Map
        {
            get;
            set;
        }
        public int LayerId
        {
            get;
            set;
        }

        public DlmCell[] Cells
        {
            get;
            set;
        }

        public static DlmLayer ReadFromStream(DlmMap map, BigEndianReader reader)
        {
            var layer = new DlmLayer(map);

            layer.LayerId = reader.ReadInt();
            layer.Cells = new DlmCell[reader.ReadShort()];
            for (int i = 0; i < layer.Cells.Length; i++)
            {
                layer.Cells[i] = DlmCell.ReadFromStream(layer, reader);
            }

            return layer;
        }
    }
}