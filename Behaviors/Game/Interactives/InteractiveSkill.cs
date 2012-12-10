#region License GNU GPL
// InteractiveSkill.cs
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
using System.ComponentModel;
using BiM.Behaviors.Data;
using BiM.Behaviors.Data.D2O;
using BiM.Behaviors.Data.I18N;
using BiM.Protocol.Data;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Interactives
{
    public class InteractiveSkill : INotifyPropertyChanged
    {
        private string m_name;
        private string m_templateName;

        public InteractiveSkill(InteractiveObject interactive, InteractiveElementSkill skill)
        {
            Id = skill.skillInstanceUid;
            Interactive = interactive;
            JobSkill = ObjectDataManager.Instance.Get<Skill>(skill.skillId);

            if (skill is InteractiveElementNamedSkill)
                NameId = (int?) ObjectDataManager.Instance.Get<SkillName>((skill as InteractiveElementNamedSkill).nameId).nameId;
        }

        public int Id
        {
            get;
            private set;
        }

        public string Name
        {
            get
            {
                if (NameId != null)
                    return m_name ?? (m_name = I18NDataManager.Instance.ReadText((int) NameId));

                else if (JobSkill != null)
                    return m_templateName ?? (m_templateName = I18NDataManager.Instance.ReadText(JobSkill.nameId));

                return string.Empty;
            }
        }

        public int? NameId
        {
            get;
            private set;
        }

        public InteractiveObject Interactive
        {
            get;
            private set;
        }

        public Skill JobSkill
        {
            get;
            private set;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
          if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        #endregion

        public bool IsEnabled()
        {
            return Interactive.EnabledSkills.Contains(this);
        }
    }
}