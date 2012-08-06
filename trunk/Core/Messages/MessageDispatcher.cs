using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using BiM.Core.Collections;
using BiM.Core.Extensions;
using NLog;

namespace BiM.Core.Messages
{
    /// <summary>
    /// Classic message dispatcher
    /// </summary>
    /// <typeparam name="T">Messsage dispatcher type</typeparam>
    public class MessageDispatcher
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected class MessageHandler
        {
            public MessageHandler(object container, MessageHandlerAttribute handlerAttribute, Action<object, object, Message> action, Type tokenType)
            {
                Container = container;
                Attribute = handlerAttribute;
                Action = action;
                TokenType = tokenType;
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

            public Action<object, object, Message> Action
            {
                get;
                private set;
            }

            public Type TokenType { get; private set; }
        }

        private readonly SortedDictionary<MessagePriority, Queue<Tuple<Message, object>>> m_messagesToDispatch = new SortedDictionary<MessagePriority, Queue<Tuple<Message, object>>>();
        private static readonly Dictionary<Type, List<MessageHandler>> m_handlers = new Dictionary<Type, List<MessageHandler>>();

        private int m_currentThreadId;
        private object m_currentProcessor;
        private ManualResetEventSlim m_resumeEvent = new ManualResetEventSlim(true);
        private ManualResetEventSlim m_messageEnqueuedEvent = new ManualResetEventSlim(false);

        private bool m_stopped;
        private bool m_dispatching;

        public MessageDispatcher()
        {
            foreach (var value in Enum.GetValues(typeof(MessagePriority)))
            {
                m_messagesToDispatch.Add((MessagePriority)value, new Queue<Tuple<Message, object>>());
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
                RegisterStaticContainer(type);
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
            if (container == null) throw new ArgumentNullException("container");
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

        public static void Register(MethodInfo method, object container)
        {
            var attributes = method.GetCustomAttributes(typeof(MessageHandlerAttribute), false) as MessageHandlerAttribute[];

            if (attributes == null || attributes.Length == 0)
                throw new Exception("A handler method must have a at least one MessageHandler attribute");

            Register(method, container, attributes);
        }

        public static void Register(MethodInfo method, object container, params MessageHandlerAttribute[] attributes)
        {
            if (method == null) throw new ArgumentNullException("method");
            if (attributes == null || attributes.Length == 0)
                return;


            var parameters = method.GetParameters();

            if (parameters.Length != 2 ||
                !parameters[1].ParameterType.IsSubclassOf(typeof(Message)))
            {
                throw new ArgumentException(string.Format("Method handler {0} has incorrect parameters. Right definition is Handler(object, Message)", method));
            }

            if (!method.IsStatic && container == null)
                throw new ArgumentException("You must give an object container if the method is static");

            Action<object, object, Message> handlerDelegate;
            try
            {
                handlerDelegate = (Action<object, object, Message>)method.CreateDelegate(typeof(object), typeof(Message));
            }
            catch (Exception)
            {
                throw new ArgumentException(string.Format("Method handler {0} has incorrect parameters. Right definition is Handler(object Message)", method));
            }

            foreach (var attribute in attributes)
            {
                Register(attribute.MessageType, attribute, handlerDelegate, parameters[0].ParameterType, method.IsStatic ? null : container);
            }
        }

        public static void Register(Type messageType, MessageHandlerAttribute attribute, Action<object, object, Message> action, Type tokenType, object container = null)
        {
            if (attribute == null) throw new ArgumentNullException("attribute");
            if (action == null) throw new ArgumentNullException("action");
            if (!m_handlers.ContainsKey(messageType))
                m_handlers.Add(messageType, new List<MessageHandler>());

            m_handlers[messageType].Add(new MessageHandler(container, attribute, action, tokenType));
        }

        public static void UnRegister()
        {
            throw new NotImplementedException();
        }

        public static bool IsRegistered(Type messageType)
        {
            return m_handlers.ContainsKey(messageType);
        }

        protected static IEnumerable<MessageHandler> GetHandlers(Type messageType, object token)
        {
            IEnumerable<MessageHandler> handlers = null;

            if (m_handlers.ContainsKey(messageType))
                handlers = m_handlers[messageType].Where(entry => token == null || entry.TokenType.IsAssignableFrom(token.GetType()));

            else
                if (messageType.BaseType != null && messageType.BaseType.IsSubclassOf(typeof(Message)))
                    return GetHandlers(messageType.BaseType, token);
                else
                    return Enumerable.Empty<MessageHandler>();

            if (messageType.BaseType != null && messageType.BaseType.IsSubclassOf(typeof(Message)))
                return handlers.Concat(GetHandlers(messageType.BaseType, token));

            return handlers;
        }

        public void Enqueue(Message message, bool executeIfCan = true)
        {
            Enqueue(message, null, executeIfCan);   
        }

        public virtual void Enqueue(Message message, object token, bool executeIfCan = true)
        {
            if (executeIfCan && IsInDispatchingContext())
                Dispatch(message, token);
            else
            {
                lock (m_messageEnqueuedEvent)
                {
                    m_messagesToDispatch[message.Priority].Enqueue(Tuple.Create(message, token));

                    if (!m_dispatching)
                        m_messageEnqueuedEvent.Set();
                }
            }
        }

        public bool IsInDispatchingContext()
        {
            return Thread.CurrentThread.ManagedThreadId == m_currentThreadId &&
                m_currentProcessor != null;
        }
                
        public void ProcessDispatching(object processor)
        {
            if (m_stopped)
                return;

            if (Interlocked.CompareExchange(ref m_currentThreadId, Thread.CurrentThread.ManagedThreadId, 0) == 0)
            {
                m_currentProcessor = processor;
                m_dispatching = true;
                
                foreach (var keyPair in m_messagesToDispatch)
                {
                    if (m_stopped)
                        break;

                    while (keyPair.Value.Count != 0)
                    {
                        if (m_stopped)
                            break;

                        var message = keyPair.Value.Dequeue();
                        Dispatch(message.Item1, message.Item2);
                    }
                }

                m_currentProcessor = null;
                m_dispatching = false;
                Interlocked.Exchange(ref m_currentThreadId, 0);
            }

            lock (m_messagesToDispatch)
            {
                if (m_messagesToDispatch.Sum(x => x.Value.Count) > 0)
                    m_messageEnqueuedEvent.Set();
                else
                    m_messageEnqueuedEvent.Reset();
            }
        }

        protected virtual void Dispatch(Message message, object token)
        {
            var handlers = GetHandlers(message.GetType(), token);

            try
            {
                foreach (var handler in handlers)
                {
                    handler.Action(handler.Container, token, message);

                    if (message.Canceled)
                        break;
                }

                message.OnDispatched();
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Exception on dispatching {0} : {1}", message, ex));
            }
        }

        /// <summary>
        /// Block the current thread until a message is enqueued
        /// </summary>
        public void Wait()
        {
            if (m_stopped)
                m_resumeEvent.Wait();

            if (m_messagesToDispatch.Sum(x => x.Value.Count) > 0)
                return;

            m_messageEnqueuedEvent.Wait();
        }

        public void Resume()
        {
            m_stopped = false;
            m_resumeEvent.Set();
        }

        public void Stop()
        {
            m_stopped = true;
            m_resumeEvent.Reset();
        }

        public void Dispose()
        {
            m_messagesToDispatch.Clear();
        }
    }
}