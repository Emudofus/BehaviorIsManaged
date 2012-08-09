using BiM.Behaviors.Data;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Jobs
{
    public class Job
    {
        public Job(PlayedCharacter owner, JobDescription job)
        {
            Owner = owner;
            JobTemplate = DataProvider.Instance.Get<Protocol.Data.Job>(job.jobId);
        }

        public PlayedCharacter Owner
        {
            get;
            set;
        }

        public Protocol.Data.Job JobTemplate
        {
            get;
            set;
        }
    }
}