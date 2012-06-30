using System.ComponentModel;
using BiM.Core.IO;

namespace BiM.Protocol.Tools.Dlm
{
    public class DlmCellData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public DlmCellData(DlmMap map, short id)
        {
            Map = map;
            Id = id;
            LosMov = 3;
        }

        public DlmMap Map
        {
            get;
            private set;
        }

        public short Id
        {
            get;
            private set;
        }

        public short Floor
        {
            get;
            set;
        }

        public byte LosMov
        {
            get;
            set;
        }

        public byte Speed
        {
            get;
            set;
        }

        public byte MapChangeData
        {
            get;
            set;
        }

        public byte MoveZone
        {
            get;
            set;
        }

        public bool Los
        {
            get { return (LosMov & 2) >> 1 == 1; }
        }

        public bool Mov
        {
            get { return (LosMov & 1) == 1 && !NonWalkableDuringFight && !FarmCell; }
        }

        public bool NonWalkableDuringFight
        {
            get { return (LosMov & 4) >> 2 == 1; }
        }

        public bool Red
        {
            get { return (LosMov & 8) >> 3 == 1; }
        }

        public bool Blue
        {
            get { return (LosMov & 16) >> 4 == 1; }
        }

        public bool FarmCell
        {
            get { return (LosMov & 32) >> 5 == 1; }
        }

        public bool Visible
        {
            get { return (LosMov & 64) >> 6 == 1; }
        }

        public static DlmCellData ReadFromStream(DlmMap map, short id, BigEndianReader reader)
        {
            var cell = new DlmCellData(map, id);

            cell.Floor = (short) (reader.ReadByte()*10);

            if (cell.Floor == -1280)
            {
                return cell;
            }

            cell.LosMov = reader.ReadByte();
            cell.Speed = reader.ReadByte();
            cell.MapChangeData = reader.ReadByte();

            if (map.Version > 5)
            {
                cell.MoveZone = reader.ReadByte();
            }

            return cell;
        }
    }
}