using BiM.Core.Messages;
using BiM.Protocol.Messages;
using NLog;

namespace BiM.Behaviors.Handlers.Jobs
{
    public class JobHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [MessageHandler(typeof (JobDescriptionMessage))]
        public static void HandleJobDescriptionMessage(Bot bot, JobDescriptionMessage message)
        {
            bot.Character.Update(message);
        }

        [MessageHandler(typeof (JobExperienceMultiUpdateMessage))]
        public static void HandleJobExperienceMultiUpdateMessage(Bot bot, JobExperienceMultiUpdateMessage message)
        {
            foreach (var update in message.experiencesUpdate)
            {
                var job = bot.Character.GetJob(update.jobId);

                if (job == null)
                    logger.Warn("Cannot update job {0} experience because it's not found", update.jobId);
                else
                    job.Update(update);
            }
        }

        [MessageHandler(typeof (JobExperienceUpdateMessage))]
        public static void HandleJobExperienceUpdateMessage(Bot bot, JobExperienceUpdateMessage message)
        {
            var job = bot.Character.GetJob(message.experiencesUpdate.jobId);

            if (job == null)
                logger.Warn("Cannot update job {0} experience because it's not found", message.experiencesUpdate.jobId);
            else
                job.Update(message.experiencesUpdate);

        }
    }
}