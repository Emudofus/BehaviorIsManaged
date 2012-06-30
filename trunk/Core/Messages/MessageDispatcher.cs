using System;
using System.Collections.Generic;
using System.Reflection;
using BiM.Core.Extensions;

namespace BiM.Core.Messages
{
    public class MessageDispatcher<T>
    {
        protected class MessageHandler
        {
            public MessageHandler(object container, MessageHandlerAttribute handlerAttribute, Action<T, Message> action)
            {
                Container = container;
                Attribute = handlerAttribute;
                Action = action;
            }

            public object Container
            {
                get;
                private set;
            }

            public MessageHandlerAttribute Attribute
            {
                get;
                private set;
            }

            public Action<T, Message> Action
            {
                get;
                private set;
            }
        }

        private readonly Dictionary<uint, List<MessageHandler>> m_handlers = new Dictionary<uint, List<MessageHandler>>();

        public void RegisterAssembly(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (!type.IsAbstract)
                {
                    RegisterStaticContainer(type);
                }
            }
        }

        public void RegisterStaticContainer(Type type)
        {
            var methods = type.GetMethods(BindingFlags.Static |
                BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(MessageHandlerAttribute), false) as MessageHandlerAttribute[];

                if (attributes == null || attributes.Length == 0)
                    continue;

                var parameters = method.GetParameters();

                if (parameters.Length != 2 ||
                    parameters[0].ParameterType != typeof(T) ||
                    parameters[1].ParameterType != typeof(Message))
                {
                    throw new ArgumentException(string.Format("Method handler {0} has incorrect parameters. Right definition is Handler({1}, Mesage)", method, typeof(T).Name));
                }

                Register(method, null, attributes);
            }
        }

        public void RegisterContainer(object container)
        {
            var type = container.GetType();

            var methods = type.GetMethods(BindingFlags.Static |BindingFlags.Instance |
                BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(MessageHandlerAttribute), false) as MessageHandlerAttribute[];

                if (attributes == null || attributes.Length == 0)
                    continue;

                var parameters = method.GetParameters();

                if (parameters.Length != 2 ||
                    parameters[0].ParameterType != typeof(T) ||
                    parameters[1].ParameterType != typeof(Message))
                {
                    throw new ArgumentException(string.Format("Method handler {0} has incorrect parameters. Right definition is Handler({1}, Mesage)", method, typeof(T).Name));
                }

                Register(method, container, attributes);
            }
        }

        public void Register(MethodInfo method, object container, params MessageHandlerAttribute[] attributes)
        {
            if (attributes == null || attributes.Length == 0)
                return;

            if (!method.IsStatic && container == null)
                throw new ArgumentException("You must give an object container if the method is static");

            Action<T, Message> handlerDelegate;
            try
            {
                handlerDelegate = method.CreateDelegate(typeof(T), typeof(Message)) as Action<T, Message>;
            }
            catch (Exception)
            {
                throw new ArgumentException(string.Format("Method handler {0} has incorrect parameters. Right definition is Handler({1}, Mesage)", method, typeof(T).Name));
            }

            foreach (var attribute in attributes)
            {
                Register(attribute.MessageId, attribute, handlerDelegate, method.IsStatic ? null : container);
            }
        }

        public void Register(uint messageId, MessageHandlerAttribute attribute, Action<T, Message> action, object container = null)
        {
            if (!m_handlers.ContainsKey(messageId))
                m_handlers.Add(messageId, new List<MessageHandler>());

            m_handlers[messageId].Add(new MessageHandler(container, attribute, action));
        }

        public bool IsRegister(uint messageId)
        {
            return m_handlers.ContainsKey(messageId);
        }

        public void Dispatch(Message message)
        {
            List<MessageHandler> handlers;
            if (m_handlers.TryGetValue(message.MessageId, out handlers))
            {
                try
                {
                    foreach (var handler in handlers)
                    {
                        // send to task queue ?
                    }
                }
                catch (Exception)
                {
                    // todo : log
                    throw;
                }
            }
        }
    }
}