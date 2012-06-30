using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using BiM.Core.Extensions;
using BiM.Core.IO;

namespace BiM.Protocol.Messages
{
    public static class MessageReceiver
    {
        private static readonly Dictionary<uint, Type> Messages = new Dictionary<uint, Type>(800);
        private static readonly Dictionary<uint, Func<NetworkMessage>> Constructors = new Dictionary<uint, Func<NetworkMessage>>(800);


        /// <summary>
        ///   Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            Assembly asm = Assembly.GetAssembly(typeof(MessageReceiver));

                foreach (Type type in asm.GetTypes())
                {
                    var fieldId = type.GetField("Id");

                    if (fieldId != null)
                    {
                        var id = (uint)fieldId.GetValue(type);
                        if (Messages.ContainsKey(id))
                            throw new AmbiguousMatchException(
                                string.Format(
                                    "MessageReceiver() => {0} item is already in the dictionary, old type is : {1}, new type is  {2}",
                                    id, Messages[id], type));

                        Messages.Add(id, type);

                        ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);

                        if (ctor == null)
                            throw new Exception(
                                string.Format("'{0}' doesn't implemented a parameterless constructor",
                                              type));

                        Constructors.Add(id, ctor.CreateDelegate<Func<NetworkMessage>>());
                    }
                }
        }

        /// <summary>
        ///   Gets instance of the message defined by id thanks to BigEndianReader.
        /// </summary>
        /// <param name = "id">id.</param>
        /// <returns></returns>
        public static NetworkMessage BuildMessage(uint id, BigEndianReader reader)
        {
            if (!Messages.ContainsKey(id))
                throw new MessageNotFoundException(string.Format("NetworkMessage <id:{0}> doesn't exist", id));

            NetworkMessage message = Constructors[id]();

            if (message == null)
                throw new MessageNotFoundException(string.Format("Constructors[{0}] (delegate {1}) does not exist", id, Messages[id]));

            message.Unpack(reader);

            return message;
        }

        public static Type GetMessageType(uint id)
        {
            if (!Messages.ContainsKey(id))
                throw new MessageNotFoundException(string.Format("NetworkMessage <id:{0}> doesn't exist", id));

            return Messages[id];
        }

        [Serializable]
        public class MessageNotFoundException : Exception
        {
            public MessageNotFoundException()
            {
            }

            public MessageNotFoundException(string message)
                : base(message)
            {
            }

            public MessageNotFoundException(string message, Exception inner)
                : base(message, inner)
            {
            }

            protected MessageNotFoundException(
                SerializationInfo info,
                StreamingContext context)
                : base(info, context)
            {
            }
        }
    }
}