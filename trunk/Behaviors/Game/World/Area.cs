using System;
using BiM.Behaviors.Data;
using BiM.Protocol.Data;

namespace BiM.Behaviors.Game.World
{
    public class Area
    {
        private readonly Protocol.Data.Area m_area;

        public Area(int id)
        {
            m_area = DataProvider.Instance.Get<Protocol.Data.Area>(id);
            SuperArea = new SuperArea(SuperAreaId);
        }

        public int Id
        {
            get { return m_area.id; }
        }

        public uint NameId
        {
            get { return m_area.nameId; }
        }

        public string Name
        {
            get { return DataProvider.Instance.Get<string>(NameId); }
        }

        public SuperArea SuperArea
        {
            get;
            private set;
        }

        public int SuperAreaId
        {
            get { return m_area.superAreaId; }
        }

        public bool ContainHouses
        {
            get { return m_area.containHouses; }
        }

        public Boolean ContainPaddocks
        {
            get { return m_area.containPaddocks; }
        }

        public Rectangle Bounds
        {
            get { return m_area.bounds; }
        }
    }
}