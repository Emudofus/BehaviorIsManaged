using BiM.Behaviors.Data;
using BiM.Protocol.Data;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Actors.RolePlay
{
    public class Monster
    {
        private readonly Protocol.Data.Monster m_monster;
        private readonly MonsterRace m_race;
        private readonly MonsterSuperRace m_superRace;
        private readonly MonsterGrade m_monsterGrade;

        public Monster(MonsterInGroupInformations informations)
            : this(informations, informations.look)
        {
        }

        public Monster(MonsterInGroupLightInformations informations, EntityLook look)
        {
            Look = look;
            m_monster = DataProvider.Instance.Get<Protocol.Data.Monster>(informations.creatureGenericId);
            m_monsterGrade = m_monster.grades[informations.grade - 1];
            m_race = DataProvider.Instance.Get<MonsterRace>(m_monster.race);
            m_superRace = DataProvider.Instance.Get<MonsterSuperRace>(m_race.superRaceId);

        }

        public int Id
        {
            get { return m_monster.id; }
        }

        public EntityLook Look
        {
            get;
            private set;
        }

        public Protocol.Data.Monster MonsterTemplate
        {
            get { return m_monster; }
        }

        public MonsterGrade Grade
        {
            get { return m_monsterGrade; }
        }

        public MonsterRace Race
        {
            get { return m_race; }
        }

        public MonsterSuperRace SuperRace
        {
            get { return m_superRace; }
        }

        public string Name
        {
            get { return DataProvider.Instance.Get<string>(m_monster.nameId); }
        }

        public bool IsBoss
        {
            get { return m_monster.isBoss; }
        }

        public string RaceName
        {
            get { return DataProvider.Instance.Get<string>(m_race.nameId); }
        }

        public bool IsArchMonster
        {
            get { return m_race.id == 78 && m_superRace.id == 20; }
        }

        public string SuperRaceName
        {
            get { return DataProvider.Instance.Get<string>(m_superRace.nameId); }
        }
    }
}