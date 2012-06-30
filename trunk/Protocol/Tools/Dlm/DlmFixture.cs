using System.ComponentModel;
using BiM.Core.IO;

namespace BiM.Protocol.Tools.Dlm
{
    public class DlmFixture : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public DlmFixture(DlmMap map)
        {
            Map = map;
        }

        public DlmMap Map
        {
            get;
            set;
        }

        public int FixtureId
        {
            get;
            private set;
        }

        public System.Drawing.Point Offset
        {
            get;
            set;
        }

        public short Rotation
        {
            get;
            set;
        }

        public short ScaleX
        {
            get;
            set;
        }

        public short ScaleY
        {
            get;
            set;
        }

        public int Hue
        {
            get;
            set;
        }

        public byte Alpha
        {
            get;
            set;
        }

        public static DlmFixture ReadFromStream(DlmMap map, BigEndianReader reader)
        {
            var fixture = new DlmFixture(map);

            fixture.FixtureId = reader.ReadInt();
            fixture.Offset = new System.Drawing.Point(reader.ReadShort(), reader.ReadShort());
            fixture.Rotation = reader.ReadShort();
            fixture.ScaleX = reader.ReadShort();
            fixture.ScaleY = reader.ReadShort();
            fixture.Hue = reader.ReadByte() << 16 | reader.ReadByte() << 8 | reader.ReadByte();
            fixture.Alpha = reader.ReadByte();

            return fixture;
        }
    }
}