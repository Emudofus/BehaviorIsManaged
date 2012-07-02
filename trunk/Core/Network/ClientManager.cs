using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace BiM.Core.Network
{
    public class ClientManager<T>
        where T : class, IClient
    {
        public const int MaxConcurrentConnections = 1000;
        public const int BufferSize = 8192;

        private BufferManager m_bufferManager; // allocate memory dedicated to a client to avoid memory alloc on each send/recv

        private SemaphoreSlim m_semaphore; // limit the number of threads accessing to a ressource
        private SocketAsyncEventArgs m_acceptArgs = new SocketAsyncEventArgs(); // async arg used on client connection

        private readonly List<T> m_clients = new List<T>();

        private Socket m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                                                    ProtocolType.Tcp);

        private IPEndPoint m_endPoint;


        public delegate T ClientCreationHandler(Socket socket);
        private ClientCreationHandler m_clientCreationDelegate;

        #region Events
        public event Action<T> ClientConnected;

        private void OnClientConnected(T client)
        {
            Action<T> handler = ClientConnected;
            if (handler != null) handler(client);
        }

        public event Action<T> ClientDisconnected;

        private void OnClientDisconnected(T client)
        {
            Action<T> handler = ClientDisconnected;
            if (handler != null) handler(client);
        }
        #endregion

        /// <summary>
        /// List of connected Clients
        /// </summary>
        public ReadOnlyCollection<T> Clients
        {
            get
            {
                return m_clients.AsReadOnly();
            }
        }

        public int Count
        {
            get
            {
                return m_clients.Count;
            }
        }

        public ClientManager(IPEndPoint endPoint, ClientCreationHandler clientCreationDelegate)
        {
            m_endPoint = endPoint;
            m_clientCreationDelegate = clientCreationDelegate;

            Initialize();
        }

        private void Initialize()
        {
            // init buffer manager
            m_bufferManager = new BufferManager(MaxConcurrentConnections * BufferSize, BufferSize);
            m_bufferManager.InitializeBuffer();

            // init semaphore
            m_semaphore = new SemaphoreSlim(MaxConcurrentConnections, MaxConcurrentConnections);
            m_acceptArgs.Completed += (sender, e) => ProcessAccept(e);
        }

        public void Start()
        {
            m_socket.Bind(m_endPoint);
            m_socket.Listen(MaxConcurrentConnections);

            StartAccept();
        }

        public void Stop()
        {

        }

        private void StartAccept()
        {
            m_acceptArgs.AcceptSocket = null;

            // thread block if the max connections limit is reached
            m_semaphore.Wait();

            // raise or not the event depending on AcceptAsync return
            if (!m_socket.AcceptAsync(m_acceptArgs))
            {
                ProcessAccept(m_acceptArgs);
            }
        }

        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            // use a async arg from the pool avoid to re-allocate memory on each connection
            var args = new SocketAsyncEventArgs();
            args.Completed += OnReceiveCompleted;

            m_bufferManager.SetBuffer(args);
            
            // create the client instance
            var client = m_clientCreationDelegate(e.AcceptSocket);
            args.UserToken = client;

            lock (m_clients)
                m_clients.Add(client);

            OnClientConnected(client);

            // if the event is not raised we first check new connections before parsing message that can blocks the connection queue
            if (!e.AcceptSocket.ReceiveAsync(args))
            {
                StartAccept();
                ProcessReceive(args);
            }
            else
            {
                StartAccept();
            }
        }

        private void OnReceiveCompleted(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                switch (e.LastOperation)
                {
                    case SocketAsyncOperation.Receive:
                        ProcessReceive(e);
                        break;
                    case SocketAsyncOperation.Disconnect:
                        CloseClientSocket(e);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception exception)
            {
                // theoretically it shouldn't go up to there.
                //logger.Error("Last chance exception on receiving ! : " + exception);
            }
        }

        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred <= 0 || e.SocketError != SocketError.Success)
            {
                CloseClientSocket(e);
                return;
            }

            var client = e.UserToken as Client;

            if (client == null)
            {
                CloseClientSocket(e);
                return;
            }

            client.Receive(e.Buffer, e.Offset, e.BytesTransferred);

            if (client.Socket == null)
            {
                CloseClientSocket(e);
                return;
            }

            // just continue to receive
            bool willRaiseEvent = client.Socket.ReceiveAsync(e);

            if (!willRaiseEvent)
            {
                ProcessReceive(e);
            }
        }

        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            var client = e.UserToken as T;

            if (client == null)
                return;

            try
            {
                client.Disconnect();
            }
            finally
            {
                lock (m_clients)
                    m_clients.Remove(client);

                OnClientDisconnected(client);
                m_semaphore.Release();
                e.Dispose();
            }
        }
    }
}