using System.ComponentModel;
using BiM.Behaviors.Data;
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
            JobSkill = DataProvider.Instance.Get<Skill>(skill.skillId);

            if (skill is InteractiveElementNamedSkill)
                NameId = (int?) DataProvider.Instance.Get<SkillName>((skill as InteractiveElementNamedSkill).nameId).nameId;
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
                    return m_name ?? (m_name = DataProvider.Instance.Get<string>((int) NameId));

                else if (JobSkill != null)
                    return m_templateName ?? (m_templateName = DataProvider.Instance.Get<string>(JobSkill.nameId));

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

        #endregion

        public bool IsEnabled()
        {
            return Interactive.EnabledSkills.Contains(this);
        }
    }
}