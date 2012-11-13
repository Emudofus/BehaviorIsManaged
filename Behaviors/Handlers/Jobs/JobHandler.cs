#region License GNU GPL
// JobHandler.cs
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