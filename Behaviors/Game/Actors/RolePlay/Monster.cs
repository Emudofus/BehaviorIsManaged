#region License GNU GPL
// Monster.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion
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
            m_monster = ObjectDataManager.Instance.Get<Protocol.Data.Monster>(informations.creatureGenericId);
            m_monsterGrade = m_monster.grades[informations.grade - 1];
            m_race = ObjectDataManager.Instance.Get<MonsterRace>(m_monster.race);
            m_superRace = ObjectDataManager.Instance.Get<MonsterSuperRace>(m_race.superRaceId);

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
            get { return I18NDataManager.Instance.ReadText(m_monster.nameId); }
        }

        public bool IsBoss
        {
            get { return m_monster.isBoss; }
        }

        public string RaceName
        {
            get { return I18NDataManager.Instance.ReadText(m_race.nameId); }
        }

        public bool IsArchMonster
        {
            get { return m_race.id == 78 && m_superRace.id == 20; }
        }

        public string SuperRaceName
        {
            get { return I18NDataManager.Instance.ReadText(m_superRace.nameId); }
        }
    }
}