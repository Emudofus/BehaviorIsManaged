using BiM.Behaviors.Data;

namespace BiM.Behaviors.Game.World
{
    public class SuperArea
    {
        private readonly Protocol.Data.SuperArea m_superArea;

        public SuperArea(int id)
        {
            m_superArea = DataProvider.Instance.Get<Protocol.Data.SuperArea>(id);
        }

        public int Id
        {
            get { return m_superArea.id; }
        }

        public uint NameId
        {
            get { return m_superArea.nameId; }
        }

        public string Name
        {
            get { return DataProvider.Instance.Get<string>(NameId); }
        }

        public uint WorldmapId
        {
            get { return m_superArea.worldmapId; }
        }
    }
}