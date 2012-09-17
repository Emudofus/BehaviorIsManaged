using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BiM.Behaviors.Data;

namespace BiM.Behaviors.Game.Actors.RolePlay
{
    public class Monster
    {
        private readonly Protocol.Data.Monster m_monster;
        private readonly Protocol.Data.MonsterRace m_race;
        private readonly Protocol.Data.MonsterSuperRace m_superRace;

        public Monster(int id)
        {
            m_monster = DataProvider.Instance.Get<Protocol.Data.Monster>(id);
            m_race = DataProvider.Instance.Get<Protocol.Data.MonsterRace>(m_monster.race);
            m_superRace = DataProvider.Instance.Get<Protocol.Data.MonsterSuperRace>(m_race.superRaceId);
        }

        public int Id
        {
            get { return m_monster.id; }
        }

        public string Name
        {
            get { return DataProvider.Instance.Get<string>(m_monster.nameId); }
        }

        public bool IsBoss
        {
            get { return m_monster.isBoss; }
        }

        public string Race
        {
            get { return DataProvider.Instance.Get<string>(m_race.nameId); }
        }

        public bool IsArchMonster
        {
            get { return m_race.id == 78 && m_superRace.id == 20; }
        }

        public string SuperRace
        {
            get { return DataProvider.Instance.Get<string>(m_superRace.nameId); }
        }
    }
}
