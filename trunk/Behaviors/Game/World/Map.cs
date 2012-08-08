using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using BiM.Behaviors.Data;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Core.Config;
using BiM.Protocol.Tools.Dlm;

namespace BiM.Behaviors.Game.World
{
    public class Map : IContext
    {
        public static string GenericDecryptionKey
        {
            get { return Config.GetStatic("MapDecryptionKey", "649ae451ca33ec53bbcbcc33becf15f4"); }
        }

        public const uint Width = 14;
        public const uint Height = 20;

        public const uint MapSize = Width*Height*2;
        private readonly DlmMap m_map;

        public Map(int id)
            : this(id, GenericDecryptionKey)
        {

        }

        public Map(int id, string decryptionKey)
        {
            m_map = DataProvider.Instance.Get<DlmMap>(id, decryptionKey);
            IEnumerable<Cell> cells = m_map.Cells.Select(entry => new Cell(this, entry));

            Cells = new CellList(cells.ToArray());
        }

        public CellList Cells
        {
            get;
            private set;
        }

        public ReadOnlyCollection<RolePlayActor> Actors
        {
            get;
            set;
        }

        #region Map Properties

        public int Id
        {
            get { return m_map.Id; }
        }

        public byte Version
        {
            get { return m_map.Version; }
        }

        public bool Encrypted
        {
            get { return m_map.Encrypted; }
        }

        public byte EncryptionVersion
        {
            get { return m_map.EncryptionVersion; }
        }

        public uint RelativeId
        {
            get { return m_map.RelativeId; }
        }

        public byte MapType
        {
            get { return m_map.MapType; }
        }

        public int SubAreaId
        {
            get { return m_map.SubAreaId; }
        }

        public int BottomNeighbourId
        {
            get { return m_map.BottomNeighbourId; }
        }

        public int LeftNeighbourId
        {
            get { return m_map.LeftNeighbourId; }
        }

        public int RightNeighbourId
        {
            get { return m_map.RightNeighbourId; }
        }

        public int ShadowBonusOnEntities
        {
            get { return m_map.ShadowBonusOnEntities; }
        }

        public Color BackgroundColor
        {
            get { return m_map.BackgroundColor; }
        }

        public ushort ZoomScale
        {
            get { return m_map.ZoomScale; }
        }

        public short ZoomOffsetX
        {
            get { return m_map.ZoomOffsetX; }
        }

        public short ZoomOffsetY
        {
            get { return m_map.ZoomOffsetY; }
        }

        public bool UseLowPassFilter
        {
            get { return m_map.UseLowPassFilter; }
        }

        public bool UseReverb
        {
            get { return m_map.UseReverb; }
        }

        public int PresetId
        {
            get { return m_map.PresetId; }
        }

        public DlmFixture[] BackgroudFixtures
        {
            get { return m_map.BackgroudFixtures; }
        }

        public int TopNeighbourId
        {
            get { return m_map.TopNeighbourId; }
        }

        public DlmFixture[] ForegroundFixtures
        {
            get { return m_map.ForegroundFixtures; }
        }


        public int GroundCRC
        {
            get { return m_map.GroundCRC; }
        }

        public DlmLayer[] Layers
        {
            get { return m_map.Layers; }
        }

        #endregion
    }
}