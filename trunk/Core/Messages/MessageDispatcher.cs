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
            public MessageHandler(object container, Type containerType, Type messageType, MessageHandlerAttribute handlerAttribute, Action<object, object, Message> action, Type tokenType, IMessageFilter filter)
            {
                Container = container;
                ContainerType = containerType;
                MessageType = messageType;
                Attribute = handlerAttribute;
                Action = action;
                TokenType = tokenType;
                Filter = filter;
            }

            public object Container
            {
                get;
                set;
            }

            public Type ContainerType
            {
                get;
                set;
            }

            public Type MessageType
            {
                get;
                set;
            }

            public MessageHandlerAttribute Attribute
            {
                get;
                set;
            }

            public Action<object, object, Message> Action
            {
                get;
                set;
            }

            public Type TokenType { get; set; }

            public IMessageFilter Filter { get; set; }
        }

        private SortedDictionary<MessagePriority, Queue<Tuple<Message, object>>> m_messagesToDispatch = new SortedDictionary<MessagePriority, Queue<Tuple<Message, object>>>();
        private static Dictionary<Assembly, Dictionary<Type, List<MessageHandler>>> m_handlers = new Dictionary<Assembly, Dictionary<Type, List<MessageHandler>>>();

        private int m_currentThreadId;
        private object m_currentProcessor;

        public object CurrentProcessor
        {
            get { return m_currentProcessor; }
        }

        public int CurrentThreadId
        {
            get { return m_currentThreadId; }
        }

        private ManualResetEventSlim m_resumeEvent = new ManualResetEventSlim(true);
        private ManualResetEventSlim m_messageEnqueuedEvent = new ManualResetEventSlim(false);

        private bool m_stopped;
        private bool m_dispatching;

        public event Action<MessageDispatcher, Message> MessageDispatched;

        protected void OnMessageDispatched(Message message)
        {
            var evnt = MessageDispatched;
            if (evnt != null)
                MessageDispatched(this, message);
        }

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
                Register(attribute.MessageType, method.DeclaringType, attribute, handlerDelegate, parameters[0].ParameterType, method.IsStatic ? null : container);
            }
        }

        public static void Register(Type messageType, Type containerType, MessageHandlerAttribute attribute, Action<object, object, Message> action, Type tokenType, object container = null)
        {
            if (attribute == null) throw new ArgumentNullException("attribute");
            if (action == null) throw new ArgumentNullException("action");

            var assembly = containerType.Assembly;

            // handlers are organized by assemblies to build an hierarchie
            // if the assembly is not registered yet we add it to the end
            if (!m_handlers.ContainsKey(assembly))
                m_handlers.Add(assembly, new Dictionary<Type, List<MessageHandler>>());

            if (!m_handlers[assembly].ContainsKey(messageType))
                m_handlers[assembly].Add(messageType, new List<MessageHandler>());

            IMessageFilter filter = null;
            if (attribute.FilterType != null)
            {
                if (!attribute.FilterType.HasInterface(typeof(IMessageFilter)))
                    throw new Exception(string.Format("Cannot register handler {0} in {1}, the filter type {2} doesn't implement IMessageFilter", messageType, containerType, attribute.FilterType));

                ConstructorInfo ctor;
                if ((ctor = attribute.FilterType.GetConstructor(new Type[0])) == null)
                    throw new Exception(string.Format("Cannot register handler {0} in {1}, the filter type {2} hasn't a default constructor", messageType, containerType, attribute.FilterType));

                filter = (IMessageFilter)ctor.Invoke(new object[0]);
            }

            m_handlers[assembly][messageType].Add(new MessageHandler(container, containerType, messageType, attribute, action, tokenType, filter));
        }

        public static void DefineHierarchy(IEnumerable<Assembly> assemblies)
        {
            var handlers = m_handlers;
            m_handlers = assemblies.ToDictionary(entry => entry, entry => new Dictionary<Type, List<MessageHandler>>());
            
            // assembly that are first are the basic ones
            // the handlers in this assemblies are called first
            foreach (var handler in handlers)
            {
                if (!m_handlers.ContainsKey(handler.Key))
                    m_handlers.Add(handler.Key, new Dictionary<Type, List<MessageHandler>>());

                m_handlers[handler.Key] = handler.Value;
            }
        }

        protected static void UnRegister(MessageHandler handler)
        {
            m_handlers[handler.ContainerType.Assembly][handler.MessageType].Remove(handler);
        }

        public static void UnRegister(Type messageType)
        {
            foreach (var keyPair in m_handlers)
            {
                foreach (var handler in keyPair.Value)
                {
                    if (handler.Key == messageType)
                        handler.Value.Clear();
                }
            }
        }

        public static void UnRegisterContainer(Type containerType)
        {
            foreach (var keyPair in m_handlers[containerType.Assembly])
            {
                keyPair.Value.RemoveAll(entry => entry.ContainerType == containerType);
            }
        }

        public static void UnRegisterAssembly(Assembly assembly)
        {
            m_handlers.Remove(assembly);
        }

        public static bool IsRegistered(Type messageType)
        {
            return m_handlers.Any(entry => entry.Value.ContainsKey(messageType));
        }

        protected static IEnumerable<MessageHandler> GetHandlers(Type messageType, object token)
        {
            IEnumerable<MessageHandler> handlers = null;

            // navigate through the hierarchy ...
            foreach (var keyPair in m_handlers)
            {
                // if a handler can handle this type we return it
                if (keyPair.Value.ContainsKey(messageType))
                    foreach (var handler in keyPair.Value[messageType].Where(entry => token == null || entry.TokenType.IsInstanceOfType(token)))
                    {
                        yield return handler;
                    }

                // if a handler can handle the message subclass, we add it too
                if (messageType.BaseType != null && messageType.BaseType.IsSubclassOf(typeof(Message)))
                    foreach (var handler in GetHandlers(messageType.BaseType, token))
                    {
                        yield return handler;
                    }
                    
            }
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

                var copy = m_messagesToDispatch.ToArray();
                foreach (var keyPair in copy)
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
            try
            {
                var handlers = GetHandlers(message.GetType(), token);

                foreach (var handler in handlers)
                {
                    handler.Action(handler.Container, token, message);

                    if (message.Canceled)
                        break;
                }

                message.OnDispatched();
                OnMessageDispatched(message);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Cannot dispatch {0}", message), ex);
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
            if (!m_stopped)
                return;

            m_stopped = false;
            m_resumeEvent.Set();
        }

        public void Stop()
        {
            if (m_stopped)
                return;

            m_stopped = true;
            m_resumeEvent.Reset();
        }

        public void Dispose()
        {
            Stop();

            foreach (var messages in m_messagesToDispatch)
            {
                messages.Value.Clear();
            }
        }
    }
}