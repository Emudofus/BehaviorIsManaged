#region License GNU GPL
// MessageDispatcher.cs
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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
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
            public MessageHandler(object container, Type containerType, Type messageType, MessageHandlerAttribute handlerAttribute, Action<object, object, Message> action, Type tokenType)
            {
                Container = container;
                ContainerType = containerType;
                MessageType = messageType;
                Attribute = handlerAttribute;
                Action = action;
                TokenType = tokenType;
            }

            public object Container
            {
                get;
                private set;
            }

            public Type ContainerType
            {
                get;
                private set;
            }

            public Type MessageType
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

            public Type TokenType
            {
                get;
                private set;
            }
        }

        private SortedDictionary<MessagePriority, Queue<Tuple<Message, object>>> m_messagesToDispatch = new SortedDictionary<MessagePriority, Queue<Tuple<Message, object>>>();
        private static Dictionary<Assembly, Dictionary<Type, List<MessageHandler>>> m_handlers = new Dictionary<Assembly, Dictionary<Type, List<MessageHandler>>>();
        private Dictionary<Assembly, Dictionary<Type, List<MessageHandler>>> m_nonSharedHandlers = new Dictionary<Assembly, Dictionary<Type, List<MessageHandler>>>();

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

        #region Register Static

        public static void RegisterSharedAssembly(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            foreach (var type in assembly.GetTypes())
            {
                RegisterSharedStaticContainer(type);
            }
        }

        public static void RegisterSharedStaticContainer(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            var methods = type.GetMethods(BindingFlags.Static |
                BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(MessageHandlerAttribute), false) as MessageHandlerAttribute[];

                if (attributes == null || attributes.Length == 0)
                    continue;

                RegisterShared(method, null, attributes);
            }
        }

        public static void RegisterSharedContainer(object container)
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

                RegisterShared(method, container, attributes);
            }
        }

        private static void RegisterShared(MethodInfo method, object container)
        {
            var attributes = method.GetCustomAttributes(typeof(MessageHandlerAttribute), false) as MessageHandlerAttribute[];

            if (attributes == null || attributes.Length == 0)
                throw new Exception("A handler method must have a at least one MessageHandler attribute");

            RegisterShared(method, container, attributes);
        }

        private static void RegisterShared(MethodInfo method, object container, params MessageHandlerAttribute[] attributes)
        {
            if (method == null) throw new ArgumentNullException("method");
            if (attributes == null || attributes.Length == 0)
                return;


            var parameters = method.GetParameters();

            if ((parameters.Length != 2 || !parameters[1].ParameterType.IsSubclassOf(typeof(Message))))
            {
                throw new ArgumentException(string.Format("Method handler {0} has incorrect parameters. Right definition is Handler(object, Message)", method));
            }

            if (!method.IsStatic && container == null || method.IsStatic && container != null)
                return;

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
                RegisterShared(attribute.MessageType, method.DeclaringType, attribute, handlerDelegate, parameters[0].ParameterType, method.IsStatic ? null : container);
            }
        }

        private static void RegisterShared(Type messageType, Type containerType, MessageHandlerAttribute attribute, Action<object, object, Message> action, Type tokenType, object container = null)
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

            m_handlers[assembly][messageType].Add(new MessageHandler(container, containerType, messageType, attribute, action, tokenType));
        }


        protected static void UnRegisterShared(MessageHandler handler)
        {
            m_handlers[handler.ContainerType.Assembly][handler.MessageType].Remove(handler);
        }

        public static void UnRegisterShared(Type messageType)
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

        public static void UnRegisterSharedContainer(Type containerType)
        {
            foreach (var keyPair in m_handlers[containerType.Assembly])
            {
                keyPair.Value.RemoveAll(entry => entry.ContainerType == containerType);
            }
        }

        public static void UnRegisterSharedAssembly(Assembly assembly)
        {
            m_handlers.Remove(assembly);
        }

        #endregion

        #region Register Non-Shared
        public void RegisterNonShared(object handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            var type = handler.GetType();

            var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Instance |
                BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(MessageHandlerAttribute), false) as MessageHandlerAttribute[];

                if (attributes == null || attributes.Length == 0)
                    continue;

                RegisterNonShared(method, handler, attributes);
            }
        }

        private void RegisterNonShared(MethodInfo method, object container, params MessageHandlerAttribute[] attributes)
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

            if (!method.IsStatic && container == null || method.IsStatic && container != null)
                return;

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
                RegisterNonShared(attribute.MessageType, method.DeclaringType, attribute, handlerDelegate, parameters[0].ParameterType, method.IsStatic ? null : container);
            }
        }

        private void RegisterNonShared(Type messageType, Type containerType, MessageHandlerAttribute attribute, Action<object, object, Message> action, Type tokenType, object container = null)
        {
            if (attribute == null) throw new ArgumentNullException("attribute");
            if (action == null) throw new ArgumentNullException("action");

            var assembly = containerType.Assembly;

            // handlers are organized by assemblies to build an hierarchie
            // if the assembly is not registered yet we add it to the end
            if (!m_nonSharedHandlers.ContainsKey(assembly))
                m_nonSharedHandlers.Add(assembly, new Dictionary<Type, List<MessageHandler>>());

            if (!m_nonSharedHandlers[assembly].ContainsKey(messageType))
                m_nonSharedHandlers[assembly].Add(messageType, new List<MessageHandler>());

            m_nonSharedHandlers[assembly][messageType].Add(new MessageHandler(container, containerType, messageType, attribute, action, tokenType));
        }

        public bool HasNonSharedContainer(Type type)
        {
            return m_nonSharedHandlers.Any(assembly => assembly.Value.Any(x => x.Value.Any(handler => handler.ContainerType == type)));
        }

        public void UnRegisterNonShared(Type type)
        {
            foreach (var dict in m_nonSharedHandlers.Values)
            {
                var handlers = dict.Values; // copy
                foreach (var handler in handlers)
                {
                    handler.RemoveAll(entry => entry.ContainerType == type);
                }
            }
        }

        public void UnRegisterNonShared(object container)
        {
            foreach (var dict in m_nonSharedHandlers.Values)
            {
                var handlers = dict.Values; // copy
                foreach (var handler in handlers)
                {
                    handler.RemoveAll(entry => entry.Container == container);
                }
            }
        }

        #endregion

        public bool IsRegistered(Type messageType)
        {
            return m_handlers.Concat(m_nonSharedHandlers).Any(entry => entry.Value.ContainsKey(messageType));
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

        protected IEnumerable<MessageHandler> GetHandlers(Type messageType, object token)
        {
      foreach (var list in m_nonSharedHandlers.Values.Concat(m_handlers.Values).ToArray()) // ToArray : to avoid error if handler are added in the same time
                        {
        List<MessageHandler> handlersList;
        if (list.TryGetValue(messageType, out handlersList))
          foreach (var handler in handlersList)
            if (token == null || handler.TokenType.IsInstanceOfType(token))
                            yield return handler;
                        }

            // note : disabled yet.

            // recursivity to handle message from base class
            //if (messageType.BaseType != null && messageType.BaseType.IsSubclassOf(typeof(Message)))
            //    foreach (var handler in GetHandlers(messageType.BaseType, token))
            //    {
            //        if (handler.Attribute.HandleChildMessages)
            //            yield return handler;
            //    }
        }


        public void Enqueue(Message message, bool executeIfCan = true)
        {
            Enqueue(message, null, executeIfCan);   
        }

        public virtual void Enqueue(Message message, object token, bool executeIfCan = true)
        {
            if (executeIfCan && IsInDispatchingContext())
            {
                Dispatch(message, token);
            }
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
        foreach (var handler in GetHandlers(message.GetType(), token).ToArray()) // have to transform it into a collection if we want to add/remove handler
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

    private Stopwatch _spy;

    /// <summary>
    /// Says how many milliseconds elapsed since last message. 
    /// </summary>
    public long DelayFromLastMessage
    {
      get
      {
        if (_spy == null) _spy = Stopwatch.StartNew(); return _spy.ElapsedMilliseconds;
      }
    }

    /// <summary>
    /// Reset timer for last received message
    /// </summary>
    protected void ActivityUpdate()
    {
      if (_spy == null)
        _spy = Stopwatch.StartNew();
      else
        _spy.Restart();
    }

    }
}