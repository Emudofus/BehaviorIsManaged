using System.Collections.Generic;
using BiM.Behaviors.Data;
using BiM.Protocol.Data;
using BiM.Protocol.Enums;

namespace BiM.Behaviors.Game.World
{
    public class SubArea
    {
        private readonly Protocol.Data.SubArea m_subArea;

        public SubArea(int id)
        {
            m_subArea = DataProvider.Instance.Get<Protocol.Data.SubArea>(id);
            Area = new Area(AreaId);
        }

        public int Id
        {
            get { return m_subArea.id; }
        }

        public uint NameId
        {
            get { return m_subArea.nameId; }
        }

        public string Name
        {
            get { return DataProvider.Instance.Get<string>(NameId); }
        }

        public int AreaId
        {
            get { return m_subArea.areaId; }
        }

        public Area Area
        {
            get;
            private set;
        }

        public AlignmentSideEnum AlignmentSide
        {
            get;
            set;
        }

        public List<AmbientSound> AmbientSounds
        {
            get { return m_subArea.ambientSounds; }
        }

        public List<uint> MapIds
        {
            get { return m_subArea.mapIds; }
        }

        public Rectangle Bounds
        {
            get { return m_subArea.bounds; }
        }

        public List<int> Shape
        {
            get { return m_subArea.shape; }
        }

        public List<uint> CustomWorldMap
        {
            get { return m_subArea.customWorldMap; }
        }

        public int PackId
        {
            get { return m_subArea.packId; }
        }
    }
}