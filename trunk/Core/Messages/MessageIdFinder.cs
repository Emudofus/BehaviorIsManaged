using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BiM.Core.Extensions;

namespace BiM.Core.Messages
{
    public static class MessageIdFinder
    {
        private static readonly List<uint> s_usedIds = new List<uint>();

        public static void RegisterAssembly(Assembly assembly)
        {
            lock (s_usedIds)
            {

                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof (Message)) && type.HasInterface(typeof (IStaticId)))
                    {
                        var msg = Activator.CreateInstance(type) as Message;

                        s_usedIds.Add(msg.MessageId);
                    }
                }
            }
        }
    

        public static uint FindUniqueId()
        {
            lock (s_usedIds)
            {
                var id = s_usedIds.Max() + 1;
                s_usedIds.Add(id);

                return id;
            }
        }
    }
}