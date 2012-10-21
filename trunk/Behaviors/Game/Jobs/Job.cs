using System;
using BiM.Behaviors.Data;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Jobs
{
    public class Job
    {
        private string m_name;

        public Job(PlayedCharacter owner, JobDescription job)
        {
            Owner = owner;
            JobTemplate = DataProvider.Instance.Get<Protocol.Data.Job>(job.jobId);
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
            get { return m_name ?? (m_name = DataProvider.Instance.Get<string>(JobTemplate.nameId)); }
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