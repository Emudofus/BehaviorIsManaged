#region License GNU GPL
// Job.cs
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
using System.Linq;
using BiM.Behaviors.Data.D2O;
using BiM.Behaviors.Data.I18N;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Jobs
{
    public class Job
    {
        private string m_name;

        public SkillActionDescription[] Skills { get; private set; }

        SkillActionDescriptionCollect[] _collectSkills;
        public SkillActionDescriptionCollect[] CollectSkills { get { if (_collectSkills == null) { _collectSkills = Skills.OfType<SkillActionDescriptionCollect>().ToArray(); }; return _collectSkills; } }

        SkillActionDescriptionCraft[] _craftSkills;
        public SkillActionDescriptionCraft[] CraftSkills { get { if (_craftSkills == null) { _craftSkills = Skills.OfType<SkillActionDescriptionCraft>().ToArray(); }; return _craftSkills; } }

        /// <summary>
        /// Copy ctor
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="job"></param>
        public Job(Job job)
        {
            Owner = job.Owner;
            Skills = job.Skills;
            JobTemplate = job.JobTemplate;
            _craftSkills = null;
            _collectSkills = null;
            Level = job.Level;
            Experience = job.Experience;
        }

        public Job(PlayedCharacter owner, JobDescription job)
        {
            Owner = owner;
            SetJob(job);
        }

        public void SetJob(JobDescription job, int? level = null)
        {
            JobTemplate = ObjectDataManager.Instance.Get<Protocol.Data.Job>(job.jobId);
            Skills = job.skills;
            if (Skills == null)
                Skills = new SkillActionDescription[0]; // make sure we have a valid array in all cases
            _craftSkills = null;
            _collectSkills = null;
            if (level != null)
                Level = level.Value;
        }

        public PlayedCharacter Owner
        {
            get;
            private set;
        }

        public Protocol.Data.Job JobTemplate
        {
            get;
            private set;
        }

        public int Level
        {
            get;
            private set;
        }

        public double Experience
        {
            get;
            private set;
        }

        public double ExperienceLevelFloor
        {
            get;
            private set;
        }

        public double ExperienceNextLevelFloor
        {
            get;
            private set;
        }

        public string Name
        {
            get { return m_name ?? (m_name = I18NDataManager.Instance.ReadText(JobTemplate.nameId)); }
        }

        public void Update(JobExperience experience)
        {
            if (experience == null) throw new ArgumentNullException("experience");
            Level = experience.jobLevel;
            Experience = experience.jobXP;
            ExperienceLevelFloor = experience.jobXpLevelFloor;
            ExperienceNextLevelFloor = experience.jobXpNextLevelFloor;
        }
    }
}