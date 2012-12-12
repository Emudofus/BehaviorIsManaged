#region License GNU GPL
// MonsterFighter.cs
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
using System;
using BiM.Behaviors.Data;
using BiM.Behaviors.Data.D2O;
using BiM.Behaviors.Data.I18N;
using BiM.Behaviors.Game.Fights;
using BiM.Behaviors.Game.Stats;
using BiM.Behaviors.Game.World;
using BiM.Protocol.Data;
using BiM.Protocol.Types;
using NLog;

namespace BiM.Behaviors.Game.Actors.Fighters
{
    public class MonsterFighter : Fighter
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private string m_name;

        public MonsterFighter(GameFightMonsterInformations msg, Fight fight) : base (msg, fight)
        {
            MonsterTemplate = ObjectDataManager.Instance.Get<Monster>(msg.creatureGenericId);
            MonsterGrade = MonsterTemplate.grades[msg.creatureGrade - 1];
            Level = (int) MonsterGrade.level;        
        }

        public override int Id
        {
            get;
            protected set;
        }

        public override string Name
        {
            get
            {
                return m_name ?? (m_name = I18NDataManager.Instance.ReadText(MonsterTemplate.nameId));
            }
            protected set
            {
            }
        }

        public MonsterGrade MonsterGrade
        {
            get;
            protected set;
        }

        public Monster MonsterTemplate
        {
            get;
            protected set;
        }

        public void Update(GameFightMonsterInformations msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            Id = msg.contextualId;
            Look = msg.look;
            Update(msg.disposition);
            IsAlive = msg.alive;
            MonsterTemplate = ObjectDataManager.Instance.Get<Monster>(msg.creatureGenericId);
            MonsterGrade = MonsterTemplate.grades[msg.creatureGrade - 1];
        }

        public override void Update(GameFightFighterInformations informations)
        {
            if (informations == null) throw new ArgumentNullException("informations");

            if (informations is GameFightMonsterInformations)
            {
                Update(informations as GameFightMonsterInformations);
            }
            else
            {
                logger.Error("Cannot update a {0} with a {1} instance", GetType(), informations.GetType());
                base.Update(informations);
            }
        }
    }
}