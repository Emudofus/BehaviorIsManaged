#region License GNU GPL
// MessageReceiver.cs
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
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using BiM.Core.Extensions;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class MessageReceiver : IMessageBuilder
    {
        private readonly Dictionary<uint, Func<NetworkMessage>> m_constructors = new Dictionary<uint, Func<NetworkMessage>>(800);
        private readonly Dictionary<uint, Type> m_messages = new Dictionary<uint, Type>(800);

        #region IMessageBuilder Members

        /// <summary>
        ///   Gets instance of the message defined by id thanks to BigEndianReader.
        /// </summary>
        /// <param name = "id">id.</param>
        /// <returns></returns>
        public NetworkMessage BuildMessage(uint id, IDataReader reader)
        {
            if (!m_messages.ContainsKey(id))
                throw new MessageNotFoundException(string.Format("NetworkMessage <id:{0}> doesn't exist", id));

            NetworkMessage message = m_constructors[id]();

            if (message == null)
                throw new MessageNotFoundException(string.Format("Constructors[{0}] (delegate {1}) does not exist", id, m_messages[id]));

            message.Unpack(reader);

            return message;
        }

        #endregion

        /// <summary>
        ///   Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            Assembly asm = Assembly.GetAssembly(typeof (MessageReceiver));

            foreach (Type type in asm.GetTypes())
            {
                if (!type.IsSubclassOf(typeof (NetworkMessage)))
                    continue;

                FieldInfo fieldId = type.GetField("Id");

                if (fieldId != null)
                {
                    var id = (uint) fieldId.GetValue(type);
                    if (m_messages.ContainsKey(id))
                        throw new AmbiguousMatchException(
                            string.Format(
                                "MessageReceiver() => {0} item is already in the dictionary, old type is : {1}, new type is  {2}",
                                id, m_messages[id], type));

                    m_messages.Add(id, type);

                    ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);

                    if (ctor == null)
                        throw new Exception(
                            string.Format("'{0}' doesn't implemented a parameterless constructor",
                                          type));

                    m_constructors.Add(id, ctor.CreateDelegate<Func<NetworkMessage>>());
                }
            }
        }

        public Type GetMessageType(uint id)
        {
            if (!m_messages.ContainsKey(id))
                throw new MessageNotFoundException(string.Format("NetworkMessage <id:{0}> doesn't exist", id));

            return m_messages[id];
        }

        #region Nested type: MessageNotFoundException

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

        #endregion
    }
}