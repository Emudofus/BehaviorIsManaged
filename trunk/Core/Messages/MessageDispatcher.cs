using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading;
using BiM.Core.Collections;
using BiM.Core.Extensions;
using NLog;

namespace BiM.Core.Messages
{
    public class MessageDispatcher<T> where T : class
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected class MessageHandler
        {
            public MessageHandler(object container, MessageHandlerAttribute handlerAttribute, Action<object, T, Message> action)
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

            public Action<object, T, Message> Action
            {
                get;
                private set;
            }
        }

        private readonly SortedDictionary<MessagePriority, Queue<Message>> m_messagesToDispatch = new SortedDictionary<MessagePriority, Queue<Message>>();
        private static readonly Dictionary<uint, List<MessageHandler>> m_handlers = new Dictionary<uint, List<MessageHandler>>();

        private int m_currentThreadId;
        private T m_currentProcessor;

        private bool m_stopped;

        public MessageDispatcher()
        {
            foreach (var value in Enum.GetValues(typeof(MessagePriority)))
            {
                m_messagesToDispatch.Add((MessagePriority)value, new Queue<Message>());
            }
        }

        public bool Stopped
        {
            get { return m_stopped; }
        }

        public static void RegisterAssembly(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            foreach (var type in assembly.GetTypes())
            {
                if (!type.IsAbstract)
                {
                    RegisterStaticContainer(type);
                }
            }
        }

        public static void RegisterStaticContainer(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            var methods = type.GetMethods(BindingFlags.Static |
                BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(MessageHandlerAttribute), false) as MessageHandlerAttribute[];

                if (attributes == null || attributes.Length == 0)
                    continue;

                Register(method, null, attributes);
            }
        }

        public static void RegisterContainer(object container)
        {
            var type = container.GetType();

            var methods = type.GetMethods(BindingFlags.Static |BindingFlags.Instance |
                BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(MessageHandlerAttribute), false) as MessageHandlerAttribute[];

                if (attributes == null || attributes.Length == 0)
                    continue;

                Register(method, container, attributes);
            }
        }

        public static void Register(MethodInfo method, object container, params MessageHandlerAttribute[] attributes)
        {
            if (method == null) throw new ArgumentNullException("method");
            if (attributes == null || attributes.Length == 0)
                return;


            var parameters = method.GetParameters();

            if (parameters.Length != 2 ||
                parameters[0].ParameterType != typeof(T) ||
                !parameters[1].ParameterType.IsSubclassOf(typeof(Message)))
            {
                throw new ArgumentException(string.Format("Method handler {0} has incorrect parameters. Right definition is Handler({1}, Mesage)", method, typeof(T).Name));
            }

            if (!method.IsStatic && container == null)
                throw new ArgumentException("You must give an object container if the method is static");

            Action<object, T, Message> handlerDelegate;
            try
            {
                handlerDelegate = (Action<object, T, Message>)method.CreateDelegate(typeof(T), typeof(Message));
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

        public static void Register(uint messageId, MessageHandlerAttribute attribute, Action<object, T, Message> action, object container = null)
        {
            if (attribute == null) throw new ArgumentNullException("attribute");
            if (action == null) throw new ArgumentNullException("action");
            if (!m_handlers.ContainsKey(messageId))
                m_handlers.Add(messageId, new List<MessageHandler>());

            m_handlers[messageId].Add(new MessageHandler(container, attribute, action));
        }

        public static void UnRegister()
        {
            throw new NotImplementedException();
        }

        public static bool IsRegistered(uint messageId)
        {
            return m_handlers.ContainsKey(messageId);
        }

        protected static ReadOnlyCollection<MessageHandler> GetHandlers(uint messageId)
        {
            List<MessageHandler> handlers;

            if (m_handlers.TryGetValue(messageId, out handlers))
            {
                return handlers.AsReadOnly();
            }

            return new List<MessageHandler>().AsReadOnly();
        }

        public virtual void Enqueue(Message message, bool executeIfCan = true)
        {
            if (executeIfCan && IsInDispatchingContext())
                Dispatch(m_currentProcessor, message);
            else
                m_messagesToDispatch[message.Priority].Enqueue(message);
        }

        public bool IsInDispatchingContext()
        {
            return Thread.CurrentThread.ManagedThreadId == m_currentThreadId &&
                m_currentProcessor != null;
        }

        public void ProcessDispatching(T processor)
        {
            if (Interlocked.CompareExchange(ref m_currentThreadId, Thread.CurrentThread.ManagedThreadId, 0) == 0)
            {
                m_currentProcessor = processor;

                foreach (var keyPair in m_messagesToDispatch)
                {
                    if (m_stopped)
                        break;

                    while (keyPair.Value.Count != 0)
                    {
                        if (m_stopped)
                            break;

                        var message = keyPair.Value.Dequeue();
                        Dispatch(processor, message);
                    }
                }

                Interlocked.Exchange(ref m_currentThreadId, 0);
                m_currentProcessor = null;
            }
        }

        protected virtual void Dispatch(T processor, Message message)
        {
            var handlers = GetHandlers(message.MessageId);

            try
            {
                foreach (var handler in handlers)
                {
                    handler.Action(handler.Container, processor, message);

                    if (message.Canceled)
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException(string.Format("Exception on dispatching {0}", message), ex);
            }
        }

        public void Start()
        {
            m_stopped = false;
        }

        public void Stop()
        {
            m_stopped = true;
        }

        public void Dispose()
        {
            m_messagesToDispatch.Clear();
        }
    }
}