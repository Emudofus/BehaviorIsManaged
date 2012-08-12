using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using BiM.Core.IO;
using BiM.Protocol.Data;

namespace BiM.Protocol.Tools.Dlm
{
    public class DlmMap : INotifyPropertyChanged, IDataObject
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public const int CellCount = 560;

        public DlmMap()
        {
            
        }

        public byte Version
        {
            get;
            set;
        }

        public int Id
        {
            get;
            set;
        }

        public bool Encrypted
        {
            get;
            set;
        }

        public byte EncryptionVersion
        {
            get;
            set;
        }

        public uint RelativeId
        {
            get;
            set;
        }

        public byte MapType
        {
            get;
            set;
        }

        public int SubAreaId
        {
            get;
            set;
        }

        public int BottomNeighbourId
        {
            get;
            set;
        }

        public int LeftNeighbourId
        {
            get;
            set;
        }

        public int RightNeighbourId
        {
            get;
            set;
        }

        public int ShadowBonusOnEntities
        {
            get;
            set;
        }

        public Color BackgroundColor
        {
            get;
            set;
        }

        public ushort ZoomScale
        {
            get;
            set;
        }

        public short ZoomOffsetX
        {
            get;
            set;
        }

        public short ZoomOffsetY
        {
            get;
            set;
        }

        public bool UseLowPassFilter
        {
            get;
            set;
        }

        public bool UseReverb
        {
            get;
            set;
        }

        public int PresetId
        {
            get;
            set;
        }

        public DlmFixture[] BackgroudFixtures
        {
            get;
            set;
        }

        public int TopNeighbourId
        {
            get;
            set;
        }

        public DlmFixture[] ForegroundFixtures
        {
            get;
            set;
        }

        public DlmCellData[] Cells
        {
            get;
            set;
        }

        public int GroundCRC
        {
            get;
            set;
        }

        public DlmLayer[] Layers
        {
            get;
            set;
        }

        public bool UsingNewMovementSystem
        {
            get;
            set;
        }

        public static DlmMap ReadFromStream(BigEndianReader givenReader, DlmReader dlmReader)
        {
            var reader = givenReader;
            var map = new DlmMap();

            map.Version = reader.ReadByte();
            map.Id = reader.ReadInt();

            if (map.Version >= 7)
            {
                map.Encrypted = reader.ReadBoolean();
                map.EncryptionVersion = reader.ReadByte();

                var len = reader.ReadInt();

                if (map.Encrypted)
                {
                    var key = dlmReader.DecryptionKey;

                    if (key == null && dlmReader.DecryptionKeyProvider != null)
                        key = dlmReader.DecryptionKeyProvider(map.Id);

                    if (key == null)
                    {
                        throw new InvalidOperationException(string.Format("Cannot decrypt the map {0} without decryption key", map.Id));
                    }

                    var data = reader.ReadBytes(len);
                    var encodedKey = Encoding.Default.GetBytes(key);

                    if (key.Length > 0)
                    {
                        for (int i = 0; i < data.Length; i++)
                        {
                            data[i] = (byte)( data[i] ^ encodedKey[i % key.Length] );
                        }

                        reader = new BigEndianReader(new MemoryStream(data));
                    }
                }
            }

            map.RelativeId = reader.ReadUInt();
            map.MapType = reader.ReadByte();
            map.SubAreaId = reader.ReadInt();
            map.TopNeighbourId = reader.ReadInt();
            map.BottomNeighbourId = reader.ReadInt();
            map.LeftNeighbourId = reader.ReadInt();
            map.RightNeighbourId = reader.ReadInt();
            map.ShadowBonusOnEntities = reader.ReadInt();

            if (map.Version >= 3)
            {
                map.BackgroundColor = Color.FromArgb(reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
            }

            if (map.Version >= 4)
            {
                map.ZoomScale = reader.ReadUShort();
                map.ZoomOffsetX = reader.ReadShort();
                map.ZoomOffsetY = reader.ReadShort();
            }

            map.UseLowPassFilter = reader.ReadByte() == 1;
            map.UseReverb = reader.ReadByte() == 1;

            if (map.UseReverb)
            {
                map.PresetId = reader.ReadInt();
            }
            {
                map.PresetId = -1;
            }

            map.BackgroudFixtures = new DlmFixture[reader.ReadByte()];
            for (int i = 0; i < map.BackgroudFixtures.Length; i++)
            {
                map.BackgroudFixtures[i] = DlmFixture.ReadFromStream(map, reader);
            }

            map.ForegroundFixtures = new DlmFixture[reader.ReadByte()];
            for (int i = 0; i < map.ForegroundFixtures.Length; i++)
            {
                map.ForegroundFixtures[i] = DlmFixture.ReadFromStream(map, reader);
            }

            reader.ReadInt();
            map.GroundCRC = reader.ReadInt();

            map.Layers = new DlmLayer[reader.ReadByte()];
            for (int i = 0; i < map.Layers.Length; i++)
            {
                map.Layers[i] = DlmLayer.ReadFromStream(map, reader);
            }

            map.Cells = new DlmCellData[CellCount];
            int? lastMoveZone = null;
            for (short i = 0; i < map.Cells.Length; i++)
            {
                map.Cells[i] = DlmCellData.ReadFromStream(map, i, reader);
                if (!lastMoveZone.HasValue)
                    lastMoveZone = map.Cells[i].MoveZone;
                else if (lastMoveZone != map.Cells[i].MoveZone) // if a cell is different the new system is used
                    map.UsingNewMovementSystem = true;
            }

            return map;
        }
    }
}