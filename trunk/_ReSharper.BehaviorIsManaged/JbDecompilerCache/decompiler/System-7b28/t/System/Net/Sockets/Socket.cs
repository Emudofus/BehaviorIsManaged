// Type: System.Net.Sockets.Socket
// Assembly: System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Configuration;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Net.Sockets
{
  public class Socket : IDisposable
  {
    private static readonly int protocolInformationSize = Marshal.SizeOf(typeof (UnsafeNclNativeMethods.OSSOCK.WSAPROTOCOL_INFO));
    private bool willBlock = true;
    private bool willBlockInternal = true;
    private int m_CloseTimeout = -1;
    internal const int DefaultCloseTimeout = -1;
    private const int microcnv = 1000000;
    private object m_AcceptQueueOrConnectResult;
    private SafeCloseSocket m_Handle;
    internal EndPoint m_RightEndPoint;
    internal EndPoint m_RemoteEndPoint;
    private bool m_IsConnected;
    private bool m_IsDisconnected;
    private bool isListening;
    private bool m_NonBlockingConnectInProgress;
    private EndPoint m_NonBlockingConnectRightEndPoint;
    private AddressFamily addressFamily;
    private SocketType socketType;
    private ProtocolType protocolType;
    private Socket.CacheSet m_Caches;
    internal static bool UseOverlappedIO;
    private bool useOverlappedIO;
    private bool m_BoundToThreadPool;
    private bool m_ReceivingPacketInformation;
    private ManualResetEvent m_AsyncEvent;
    private RegisteredWaitHandle m_RegisteredWait;
    private AsyncEventBits m_BlockEventBits;
    private SocketAddress m_PermittedRemoteAddress;
    private DynamicWinsockMethods m_DynamicWinsockMethods;
    private static object s_InternalSyncObject;
    private int m_IntCleanedUp;
    internal static bool s_SupportsIPv4;
    internal static bool s_SupportsIPv6;
    internal static bool s_OSSupportsIPv6;
    internal static bool s_Initialized;
    private static WaitOrTimerCallback s_RegisteredWaitCallback;
    private static bool s_LoggingEnabled;
    internal static bool s_PerfCountersEnabled;

    [Obsolete("SupportsIPv4 is obsoleted for this type, please use OSSupportsIPv4 instead. http://go.microsoft.com/fwlink/?linkid=14202")]
    public static bool SupportsIPv4
    {
      get
      {
        Socket.InitializeSockets();
        return Socket.s_SupportsIPv4;
      }
    }

    public static bool OSSupportsIPv4
    {
      get
      {
        Socket.InitializeSockets();
        return Socket.s_SupportsIPv4;
      }
    }

    [Obsolete("SupportsIPv6 is obsoleted for this type, please use OSSupportsIPv6 instead. http://go.microsoft.com/fwlink/?linkid=14202")]
    public static bool SupportsIPv6
    {
      get
      {
        Socket.InitializeSockets();
        return Socket.s_SupportsIPv6;
      }
    }

    internal static bool LegacySupportsIPv6
    {
      get
      {
        Socket.InitializeSockets();
        return Socket.s_SupportsIPv6;
      }
    }

    public static bool OSSupportsIPv6
    {
      get
      {
        Socket.InitializeSockets();
        return Socket.s_OSSupportsIPv6;
      }
    }

    public int Available
    {
      get
      {
        if (this.CleanedUp)
          throw new ObjectDisposedException(this.GetType().FullName);
        int argp = 0;
        if (UnsafeNclNativeMethods.OSSOCK.ioctlsocket(this.m_Handle, 1074030207, out argp) != SocketError.SocketError)
          return argp;
        SocketException socketException = new SocketException();
        this.UpdateStatusAfterSocketError(socketException);
        if (Socket.s_LoggingEnabled)
          Logging.Exception(Logging.Sockets, (object) this, "Available", (Exception) socketException);
        throw socketException;
      }
    }

    public EndPoint LocalEndPoint
    {
      get
      {
        if (this.CleanedUp)
          throw new ObjectDisposedException(this.GetType().FullName);
        if (this.m_NonBlockingConnectInProgress && this.Poll(0, SelectMode.SelectWrite))
        {
          this.m_IsConnected = true;
          this.m_RightEndPoint = this.m_NonBlockingConnectRightEndPoint;
          this.m_NonBlockingConnectInProgress = false;
        }
        if (this.m_RightEndPoint == null)
          return (EndPoint) null;
        SocketAddress socketAddress = this.m_RightEndPoint.Serialize();
        if (UnsafeNclNativeMethods.OSSOCK.getsockname(this.m_Handle, socketAddress.m_Buffer, out socketAddress.m_Size) == SocketError.Success)
          return this.m_RightEndPoint.Create(socketAddress);
        SocketException socketException = new SocketException();
        this.UpdateStatusAfterSocketError(socketException);
        if (Socket.s_LoggingEnabled)
          Logging.Exception(Logging.Sockets, (object) this, "LocalEndPoint", (Exception) socketException);
        throw socketException;
      }
    }

    public EndPoint RemoteEndPoint
    {
      get
      {
        if (this.CleanedUp)
          throw new ObjectDisposedException(this.GetType().FullName);
        if (this.m_RemoteEndPoint == null)
        {
          if (this.m_NonBlockingConnectInProgress && this.Poll(0, SelectMode.SelectWrite))
          {
            this.m_IsConnected = true;
            this.m_RightEndPoint = this.m_NonBlockingConnectRightEndPoint;
            this.m_NonBlockingConnectInProgress = false;
          }
          if (this.m_RightEndPoint == null)
            return (EndPoint) null;
          SocketAddress socketAddress = this.m_RightEndPoint.Serialize();
          if (UnsafeNclNativeMethods.OSSOCK.getpeername(this.m_Handle, socketAddress.m_Buffer, out socketAddress.m_Size) != SocketError.Success)
          {
            SocketException socketException = new SocketException();
            this.UpdateStatusAfterSocketError(socketException);
            if (Socket.s_LoggingEnabled)
              Logging.Exception(Logging.Sockets, (object) this, "RemoteEndPoint", (Exception) socketException);
            throw socketException;
          }
          else
          {
            try
            {
              this.m_RemoteEndPoint = this.m_RightEndPoint.Create(socketAddress);
            }
            catch
            {
            }
          }
        }
        return this.m_RemoteEndPoint;
      }
    }

    public IntPtr Handle
    {
      get
      {
        ExceptionHelper.UnmanagedPermission.Demand();
        return this.m_Handle.DangerousGetHandle();
      }
    }

    internal SafeCloseSocket SafeHandle
    {
      get
      {
        return this.m_Handle;
      }
    }

    public bool Blocking
    {
      get
      {
        return this.willBlock;
      }
      set
      {
        if (this.CleanedUp)
          throw new ObjectDisposedException(this.GetType().FullName);
        bool current;
        SocketError socketError = this.InternalSetBlocking(value, out current);
        if (socketError != SocketError.Success)
        {
          SocketException socketException = new SocketException(socketError);
          this.UpdateStatusAfterSocketError(socketException);
          if (Socket.s_LoggingEnabled)
            Logging.Exception(Logging.Sockets, (object) this, "Blocking", (Exception) socketException);
          throw socketException;
        }
        else
          this.willBlock = current;
      }
    }

    public bool UseOnlyOverlappedIO
    {
      get
      {
        return this.useOverlappedIO;
      }
      set
      {
        if (this.m_BoundToThreadPool)
          throw new InvalidOperationException(SR.GetString("net_io_completionportwasbound"));
        this.useOverlappedIO = value;
      }
    }

    public bool Connected
    {
      get
      {
        if (this.m_NonBlockingConnectInProgress && this.Poll(0, SelectMode.SelectWrite))
        {
          this.m_IsConnected = true;
          this.m_RightEndPoint = this.m_NonBlockingConnectRightEndPoint;
          this.m_NonBlockingConnectInProgress = false;
        }
        return this.m_IsConnected;
      }
    }

    public AddressFamily AddressFamily
    {
      get
      {
        return this.addressFamily;
      }
    }

    public SocketType SocketType
    {
      get
      {
        return this.socketType;
      }
    }

    public ProtocolType ProtocolType
    {
      get
      {
        return this.protocolType;
      }
    }

    public bool IsBound
    {
      get
      {
        return this.m_RightEndPoint != null;
      }
    }

    public bool ExclusiveAddressUse
    {
      get
      {
        if ((int) this.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ExclusiveAddressUse) == 0)
          return false;
        else
          return true;
      }
      set
      {
        if (this.IsBound)
          throw new InvalidOperationException(SR.GetString("net_sockets_mustnotbebound"));
        this.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ExclusiveAddressUse, value ? 1 : 0);
      }
    }

    public int ReceiveBufferSize
    {
      get
      {
        return (int) this.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer);
      }
      set
      {
        if (value < 0)
          throw new ArgumentOutOfRangeException("value");
        this.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, value);
      }
    }

    public int SendBufferSize
    {
      get
      {
        return (int) this.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer);
      }
      set
      {
        if (value < 0)
          throw new ArgumentOutOfRangeException("value");
        this.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, value);
      }
    }

    public int ReceiveTimeout
    {
      get
      {
        return (int) this.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout);
      }
      set
      {
        if (value < -1)
          throw new ArgumentOutOfRangeException("value");
        if (value == -1)
          value = 0;
        this.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, value);
      }
    }

    public int SendTimeout
    {
      get
      {
        return (int) this.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout);
      }
      set
      {
        if (value < -1)
          throw new ArgumentOutOfRangeException("value");
        if (value == -1)
          value = 0;
        this.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, value);
      }
    }

    public LingerOption LingerState
    {
      get
      {
        return (LingerOption) this.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger);
      }
      set
      {
        this.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, (object) value);
      }
    }

    public bool NoDelay
    {
      get
      {
        if ((int) this.GetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug) == 0)
          return false;
        else
          return true;
      }
      set
      {
        this.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug, value ? 1 : 0);
      }
    }

    public short Ttl
    {
      get
      {
        if (this.addressFamily == AddressFamily.InterNetwork)
          return (short) (int) this.GetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress);
        if (this.addressFamily == AddressFamily.InterNetworkV6)
          return (short) (int) this.GetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.ReuseAddress);
        else
          throw new NotSupportedException(SR.GetString("net_invalidversion"));
      }
      set
      {
        if ((int) value < 0 || (int) value > (int) byte.MaxValue)
          throw new ArgumentOutOfRangeException("value");
        if (this.addressFamily == AddressFamily.InterNetwork)
        {
          this.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, (int) value);
        }
        else
        {
          if (this.addressFamily != AddressFamily.InterNetworkV6)
            throw new NotSupportedException(SR.GetString("net_invalidversion"));
          this.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.ReuseAddress, (int) value);
        }
      }
    }

    public bool DontFragment
    {
      get
      {
        if (this.addressFamily != AddressFamily.InterNetwork)
          throw new NotSupportedException(SR.GetString("net_invalidversion"));
        if ((int) this.GetSocketOption(SocketOptionLevel.IP, SocketOptionName.DontFragment) == 0)
          return false;
        else
          return true;
      }
      set
      {
        if (this.addressFamily != AddressFamily.InterNetwork)
          throw new NotSupportedException(SR.GetString("net_invalidversion"));
        this.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.DontFragment, value ? 1 : 0);
      }
    }

    public bool MulticastLoopback
    {
      get
      {
        if (this.addressFamily == AddressFamily.InterNetwork)
        {
          if ((int) this.GetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastLoopback) == 0)
            return false;
          else
            return true;
        }
        else
        {
          if (this.addressFamily != AddressFamily.InterNetworkV6)
            throw new NotSupportedException(SR.GetString("net_invalidversion"));
          if ((int) this.GetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.MulticastLoopback) == 0)
            return false;
          else
            return true;
        }
      }
      set
      {
        if (this.addressFamily == AddressFamily.InterNetwork)
        {
          this.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastLoopback, value ? 1 : 0);
        }
        else
        {
          if (this.addressFamily != AddressFamily.InterNetworkV6)
            throw new NotSupportedException(SR.GetString("net_invalidversion"));
          this.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.MulticastLoopback, value ? 1 : 0);
        }
      }
    }

    public bool EnableBroadcast
    {
      get
      {
        if ((int) this.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast) == 0)
          return false;
        else
          return true;
      }
      set
      {
        this.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, value ? 1 : 0);
      }
    }

    bool CanUseAcceptEx
    {
      private get
      {
        if (!ComNetOS.IsWinNt)
          return false;
        if (!Thread.CurrentThread.IsThreadPoolThread && !SettingsSectionInternal.Section.AlwaysUseCompletionPortsForAccept)
          return this.m_IsDisconnected;
        else
          return true;
      }
    }

    static object InternalSyncObject
    {
      private get
      {
        if (Socket.s_InternalSyncObject == null)
        {
          object obj = new object();
          Interlocked.CompareExchange(ref Socket.s_InternalSyncObject, obj, (object) null);
        }
        return Socket.s_InternalSyncObject;
      }
    }

    Socket.CacheSet Caches
    {
      private get
      {
        if (this.m_Caches == null)
          this.m_Caches = new Socket.CacheSet();
        return this.m_Caches;
      }
    }

    internal bool CleanedUp
    {
      get
      {
        return this.m_IntCleanedUp == 1;
      }
    }

    internal TransportType Transport
    {
      get
      {
        if (this.protocolType == ProtocolType.Tcp)
          return TransportType.Tcp;
        if (this.protocolType != ProtocolType.Udp)
          return TransportType.All;
        else
          return TransportType.Udp;
      }
    }

    static Socket()
    {
    }

    public Socket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
    {
      Socket.s_LoggingEnabled = Logging.On;
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "Socket", (object) addressFamily);
      Socket.InitializeSockets();
      this.m_Handle = SafeCloseSocket.CreateWSASocket(addressFamily, socketType, protocolType);
      if (this.m_Handle.IsInvalid)
        throw new SocketException();
      this.addressFamily = addressFamily;
      this.socketType = socketType;
      this.protocolType = protocolType;
      IPProtectionLevel ipProtectionLevel = SettingsSectionInternal.Section.IPProtectionLevel;
      if (ipProtectionLevel != IPProtectionLevel.Unspecified)
        this.SetIPProtectionLevel(ipProtectionLevel);
      if (!Socket.s_LoggingEnabled)
        return;
      Logging.Exit(Logging.Sockets, (object) this, "Socket", (string) null);
    }

    public Socket(SocketInformation socketInformation)
    {
      Socket.s_LoggingEnabled = Logging.On;
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "Socket", (object) this.addressFamily);
      ExceptionHelper.UnrestrictedSocketPermission.Demand();
      Socket.InitializeSockets();
      if (socketInformation.ProtocolInformation == null || socketInformation.ProtocolInformation.Length < Socket.protocolInformationSize)
        throw new ArgumentException(SR.GetString("net_sockets_invalid_socketinformation"), "socketInformation.ProtocolInformation");
      fixed (byte* pinnedBuffer = socketInformation.ProtocolInformation)
      {
        this.m_Handle = SafeCloseSocket.CreateWSASocket(pinnedBuffer);
        UnsafeNclNativeMethods.OSSOCK.WSAPROTOCOL_INFO wsaprotocolInfo = (UnsafeNclNativeMethods.OSSOCK.WSAPROTOCOL_INFO) Marshal.PtrToStructure((IntPtr) ((void*) pinnedBuffer), typeof (UnsafeNclNativeMethods.OSSOCK.WSAPROTOCOL_INFO));
        this.addressFamily = wsaprotocolInfo.iAddressFamily;
        this.socketType = (SocketType) wsaprotocolInfo.iSocketType;
        this.protocolType = (ProtocolType) wsaprotocolInfo.iProtocol;
      }
      if (this.m_Handle.IsInvalid)
      {
        SocketException socketException = new SocketException();
        if (socketException.ErrorCode == 10022)
          throw new ArgumentException(SR.GetString("net_sockets_invalid_socketinformation"), "socketInformation");
        else
          throw socketException;
      }
      else
      {
        if (this.addressFamily != AddressFamily.InterNetwork && this.addressFamily != AddressFamily.InterNetworkV6)
          throw new NotSupportedException(SR.GetString("net_invalidversion"));
        this.m_IsConnected = socketInformation.IsConnected;
        this.willBlock = !socketInformation.IsNonBlocking;
        this.InternalSetBlocking(this.willBlock);
        this.isListening = socketInformation.IsListening;
        this.UseOnlyOverlappedIO = socketInformation.UseOnlyOverlappedIO;
        if (socketInformation.RemoteEndPoint != null)
        {
          this.m_RightEndPoint = socketInformation.RemoteEndPoint;
          this.m_RemoteEndPoint = socketInformation.RemoteEndPoint;
        }
        else
        {
          EndPoint endPoint = (EndPoint) null;
          if (this.addressFamily == AddressFamily.InterNetwork)
            endPoint = (EndPoint) IPEndPoint.Any;
          else if (this.addressFamily == AddressFamily.InterNetworkV6)
            endPoint = (EndPoint) IPEndPoint.IPv6Any;
          SocketAddress socketAddress = endPoint.Serialize();
          SocketError socketError;
          try
          {
            socketError = UnsafeNclNativeMethods.OSSOCK.getsockname(this.m_Handle, socketAddress.m_Buffer, out socketAddress.m_Size);
          }
          catch (ObjectDisposedException ex)
          {
            socketError = SocketError.NotSocket;
          }
          if (socketError == SocketError.Success)
          {
            try
            {
              this.m_RightEndPoint = endPoint.Create(socketAddress);
            }
            catch
            {
            }
          }
        }
        if (!Socket.s_LoggingEnabled)
          return;
        Logging.Exit(Logging.Sockets, (object) this, "Socket", (string) null);
      }
    }

    private Socket(SafeCloseSocket fd)
    {
      Socket.s_LoggingEnabled = Logging.On;
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "Socket", (string) null);
      Socket.InitializeSockets();
      if (fd == null || fd.IsInvalid)
        throw new ArgumentException(SR.GetString("net_InvalidSocketHandle"));
      this.m_Handle = fd;
      this.addressFamily = AddressFamily.Unknown;
      this.socketType = SocketType.Unknown;
      this.protocolType = ProtocolType.Unknown;
      if (!Socket.s_LoggingEnabled)
        return;
      Logging.Exit(Logging.Sockets, (object) this, "Socket", (string) null);
    }

    ~Socket()
    {
      this.Dispose(false);
    }

    public void Bind(EndPoint localEP)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "Bind", (object) localEP);
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (localEP == null)
        throw new ArgumentNullException("localEP");
      EndPoint endPoint = localEP;
      IPEndPoint ipEndPoint1 = localEP as IPEndPoint;
      if (ipEndPoint1 != null)
      {
        IPEndPoint ipEndPoint2 = ipEndPoint1.Snapshot();
        endPoint = (EndPoint) ipEndPoint2;
        new SocketPermission(NetworkAccess.Accept, this.Transport, ipEndPoint2.Address.ToString(), ipEndPoint2.Port).Demand();
      }
      else
        ExceptionHelper.UnmanagedPermission.Demand();
      SocketAddress socketAddress = this.CallSerializeCheckDnsEndPoint(endPoint);
      this.DoBind(endPoint, socketAddress);
      if (!Socket.s_LoggingEnabled)
        return;
      Logging.Exit(Logging.Sockets, (object) this, "Bind", "");
    }

    internal void InternalBind(EndPoint localEP)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "InternalBind", (object) localEP);
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      EndPoint remoteEP = localEP;
      SocketAddress socketAddress = this.SnapshotAndSerialize(ref remoteEP);
      this.DoBind(remoteEP, socketAddress);
      if (!Socket.s_LoggingEnabled)
        return;
      Logging.Exit(Logging.Sockets, (object) this, "InternalBind", "");
    }

    private void DoBind(EndPoint endPointSnapshot, SocketAddress socketAddress)
    {
      if (UnsafeNclNativeMethods.OSSOCK.bind(this.m_Handle, socketAddress.m_Buffer, socketAddress.m_Size) != SocketError.Success)
      {
        SocketException socketException = new SocketException();
        this.UpdateStatusAfterSocketError(socketException);
        if (Socket.s_LoggingEnabled)
          Logging.Exception(Logging.Sockets, (object) this, "DoBind", (Exception) socketException);
        throw socketException;
      }
      else
      {
        if (this.m_RightEndPoint != null)
          return;
        this.m_RightEndPoint = endPointSnapshot;
      }
    }

    public void Connect(EndPoint remoteEP)
    {
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (remoteEP == null)
        throw new ArgumentNullException("remoteEP");
      if (this.m_IsDisconnected)
        throw new InvalidOperationException(SR.GetString("net_sockets_disconnectedConnect"));
      if (this.isListening)
        throw new InvalidOperationException(SR.GetString("net_sockets_mustnotlisten"));
      this.ValidateBlockingMode();
      DnsEndPoint dnsEndPoint = remoteEP as DnsEndPoint;
      if (dnsEndPoint != null)
      {
        if (dnsEndPoint.AddressFamily != AddressFamily.Unspecified && dnsEndPoint.AddressFamily != this.addressFamily)
          throw new NotSupportedException(SR.GetString("net_invalidversion"));
        this.Connect(dnsEndPoint.Host, dnsEndPoint.Port);
      }
      else
      {
        EndPoint remoteEP1 = remoteEP;
        SocketAddress socketAddress = this.CheckCacheRemote(ref remoteEP1, true);
        if (!this.Blocking)
        {
          this.m_NonBlockingConnectRightEndPoint = remoteEP1;
          this.m_NonBlockingConnectInProgress = true;
        }
        this.DoConnect(remoteEP1, socketAddress);
      }
    }

    public void Connect(IPAddress address, int port)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "Connect", (object) address);
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (address == null)
        throw new ArgumentNullException("address");
      if (!ValidationHelper.ValidateTcpPort(port))
        throw new ArgumentOutOfRangeException("port");
      if (this.addressFamily != address.AddressFamily)
        throw new NotSupportedException(SR.GetString("net_invalidversion"));
      this.Connect((EndPoint) new IPEndPoint(address, port));
      if (!Socket.s_LoggingEnabled)
        return;
      Logging.Exit(Logging.Sockets, (object) this, "Connect", (string) null);
    }

    public void Connect(string host, int port)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "Connect", host);
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (host == null)
        throw new ArgumentNullException("host");
      if (!ValidationHelper.ValidateTcpPort(port))
        throw new ArgumentOutOfRangeException("port");
      if (this.addressFamily != AddressFamily.InterNetwork && this.addressFamily != AddressFamily.InterNetworkV6)
        throw new NotSupportedException(SR.GetString("net_invalidversion"));
      this.Connect(Dns.GetHostAddresses(host), port);
      if (!Socket.s_LoggingEnabled)
        return;
      Logging.Exit(Logging.Sockets, (object) this, "Connect", (string) null);
    }

    public void Connect(IPAddress[] addresses, int port)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "Connect", (object) addresses);
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (addresses == null)
        throw new ArgumentNullException("addresses");
      if (addresses.Length == 0)
        throw new ArgumentException(SR.GetString("net_sockets_invalid_ipaddress_length"), "addresses");
      if (!ValidationHelper.ValidateTcpPort(port))
        throw new ArgumentOutOfRangeException("port");
      if (this.addressFamily != AddressFamily.InterNetwork && this.addressFamily != AddressFamily.InterNetworkV6)
        throw new NotSupportedException(SR.GetString("net_invalidversion"));
      Exception exception = (Exception) null;
      foreach (IPAddress address in addresses)
      {
        if (address.AddressFamily == this.addressFamily)
        {
          try
          {
            this.Connect((EndPoint) new IPEndPoint(address, port));
            exception = (Exception) null;
            break;
          }
          catch (Exception ex)
          {
            if (NclUtilities.IsFatal(ex))
              throw;
            else
              exception = ex;
          }
        }
      }
      if (exception != null)
        throw exception;
      if (!this.Connected)
        throw new ArgumentException(SR.GetString("net_invalidAddressList"), "addresses");
      if (!Socket.s_LoggingEnabled)
        return;
      Logging.Exit(Logging.Sockets, (object) this, "Connect", (string) null);
    }

    public void Close()
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "Close", (string) null);
      this.Dispose();
      if (!Socket.s_LoggingEnabled)
        return;
      Logging.Exit(Logging.Sockets, (object) this, "Close", (string) null);
    }

    public void Close(int timeout)
    {
      if (timeout < -1)
        throw new ArgumentOutOfRangeException("timeout");
      this.m_CloseTimeout = timeout;
      this.Dispose();
    }

    public void Listen(int backlog)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "Listen", (object) backlog);
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (UnsafeNclNativeMethods.OSSOCK.listen(this.m_Handle, backlog) != SocketError.Success)
      {
        SocketException socketException = new SocketException();
        this.UpdateStatusAfterSocketError(socketException);
        if (Socket.s_LoggingEnabled)
          Logging.Exception(Logging.Sockets, (object) this, "Listen", (Exception) socketException);
        throw socketException;
      }
      else
      {
        this.isListening = true;
        if (!Socket.s_LoggingEnabled)
          return;
        Logging.Exit(Logging.Sockets, (object) this, "Listen", "");
      }
    }

    public Socket Accept()
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "Accept", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (this.m_RightEndPoint == null)
        throw new InvalidOperationException(SR.GetString("net_sockets_mustbind"));
      if (!this.isListening)
        throw new InvalidOperationException(SR.GetString("net_sockets_mustlisten"));
      if (this.m_IsDisconnected)
        throw new InvalidOperationException(SR.GetString("net_sockets_disconnectedAccept"));
      this.ValidateBlockingMode();
      SocketAddress socketAddress = this.m_RightEndPoint.Serialize();
      SafeCloseSocket fd = SafeCloseSocket.Accept(this.m_Handle, socketAddress.m_Buffer, ref socketAddress.m_Size);
      if (fd.IsInvalid)
      {
        SocketException socketException = new SocketException();
        this.UpdateStatusAfterSocketError(socketException);
        if (Socket.s_LoggingEnabled)
          Logging.Exception(Logging.Sockets, (object) this, "Accept", (Exception) socketException);
        throw socketException;
      }
      else
      {
        Socket acceptSocket = this.CreateAcceptSocket(fd, this.m_RightEndPoint.Create(socketAddress), false);
        if (Socket.s_LoggingEnabled)
        {
          Logging.PrintInfo(Logging.Sockets, (object) acceptSocket, SR.GetString("net_log_socket_accepted", (object) acceptSocket.RemoteEndPoint, (object) acceptSocket.LocalEndPoint));
          Logging.Exit(Logging.Sockets, (object) this, "Accept", (object) acceptSocket);
        }
        return acceptSocket;
      }
    }

    public int Send(byte[] buffer, int size, SocketFlags socketFlags)
    {
      return this.Send(buffer, 0, size, socketFlags);
    }

    public int Send(byte[] buffer, SocketFlags socketFlags)
    {
      return this.Send(buffer, 0, buffer != null ? buffer.Length : 0, socketFlags);
    }

    public int Send(byte[] buffer)
    {
      return this.Send(buffer, 0, buffer != null ? buffer.Length : 0, SocketFlags.None);
    }

    public int Send(IList<ArraySegment<byte>> buffers)
    {
      return this.Send(buffers, SocketFlags.None);
    }

    public int Send(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
    {
      SocketError errorCode;
      int num = this.Send(buffers, socketFlags, out errorCode);
      if (errorCode != SocketError.Success)
        throw new SocketException(errorCode);
      else
        return num;
    }

    public int Send(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "Send", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (buffers == null)
        throw new ArgumentNullException("buffers");
      if (buffers.Count == 0)
      {
        throw new ArgumentException(SR.GetString("net_sockets_zerolist", new object[1]
        {
          (object) "buffers"
        }), "buffers");
      }
      else
      {
        this.ValidateBlockingMode();
        errorCode = SocketError.Success;
        int count = buffers.Count;
        WSABuffer[] buffersArray = new WSABuffer[count];
        GCHandle[] gcHandleArray = (GCHandle[]) null;
        int bytesTransferred;
        try
        {
          gcHandleArray = new GCHandle[count];
          for (int index = 0; index < count; ++index)
          {
            ArraySegment<byte> segment = buffers[index];
            ValidationHelper.ValidateSegment(segment);
            gcHandleArray[index] = GCHandle.Alloc((object) segment.Array, GCHandleType.Pinned);
            buffersArray[index].Length = segment.Count;
            buffersArray[index].Pointer = Marshal.UnsafeAddrOfPinnedArrayElement((Array) segment.Array, segment.Offset);
          }
          errorCode = UnsafeNclNativeMethods.OSSOCK.WSASend_Blocking(this.m_Handle.DangerousGetHandle(), buffersArray, count, out bytesTransferred, socketFlags, (SafeHandle) SafeNativeOverlapped.Zero, IntPtr.Zero);
          if (errorCode == SocketError.SocketError)
            errorCode = (SocketError) Marshal.GetLastWin32Error();
        }
        finally
        {
          if (gcHandleArray != null)
          {
            for (int index = 0; index < gcHandleArray.Length; ++index)
            {
              if (gcHandleArray[index].IsAllocated)
                gcHandleArray[index].Free();
            }
          }
        }
        if (errorCode != SocketError.Success)
        {
          this.UpdateStatusAfterSocketError(errorCode);
          if (Socket.s_LoggingEnabled)
          {
            Logging.Exception(Logging.Sockets, (object) this, "Send", (Exception) new SocketException(errorCode));
            Logging.Exit(Logging.Sockets, (object) this, "Send", (object) 0);
          }
          return 0;
        }
        else
        {
          if (Socket.s_PerfCountersEnabled && bytesTransferred > 0)
          {
            NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketBytesSent, (long) bytesTransferred);
            if (this.Transport == TransportType.Udp)
              NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketDatagramsSent);
          }
          if (Socket.s_LoggingEnabled)
            Logging.Exit(Logging.Sockets, (object) this, "Send", (object) bytesTransferred);
          return bytesTransferred;
        }
      }
    }

    public void SendFile(string fileName)
    {
      if (!ComNetOS.IsWinNt)
        this.DownLevelSendFile(fileName);
      else
        this.SendFile(fileName, (byte[]) null, (byte[]) null, TransmitFileOptions.UseDefaultWorkerThread);
    }

    public void SendFile(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "SendFile", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (!ComNetOS.IsWinNt)
        throw new PlatformNotSupportedException(SR.GetString("WinNTRequired"));
      if (!this.Connected)
        throw new NotSupportedException(SR.GetString("net_notconnected"));
      this.ValidateBlockingMode();
      TransmitFileOverlappedAsyncResult overlappedAsyncResult = new TransmitFileOverlappedAsyncResult(this);
      FileStream fileStream = (FileStream) null;
      if (fileName != null && fileName.Length > 0)
        fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
      SafeHandle fileHandle = (SafeHandle) null;
      if (fileStream != null)
      {
        ExceptionHelper.UnmanagedPermission.Assert();
        try
        {
          fileHandle = (SafeHandle) fileStream.SafeFileHandle;
        }
        finally
        {
          CodeAccessPermission.RevertAssert();
        }
      }
      SocketError socketError = SocketError.Success;
      try
      {
        overlappedAsyncResult.SetUnmanagedStructures(preBuffer, postBuffer, fileStream, TransmitFileOptions.UseDefaultWorkerThread, true);
        if ((fileHandle != null ? (!UnsafeNclNativeMethods.OSSOCK.TransmitFile_Blocking(this.m_Handle.DangerousGetHandle(), fileHandle, 0, 0, (SafeHandle) SafeNativeOverlapped.Zero, overlappedAsyncResult.TransmitFileBuffers, flags) ? 1 : 0) : (!UnsafeNclNativeMethods.OSSOCK.TransmitFile_Blocking2(this.m_Handle.DangerousGetHandle(), IntPtr.Zero, 0, 0, (SafeHandle) SafeNativeOverlapped.Zero, overlappedAsyncResult.TransmitFileBuffers, flags) ? 1 : 0)) != 0)
          socketError = (SocketError) Marshal.GetLastWin32Error();
      }
      finally
      {
        overlappedAsyncResult.SyncReleaseUnmanagedStructures();
      }
      if (socketError != SocketError.Success)
      {
        SocketException socketException = new SocketException(socketError);
        this.UpdateStatusAfterSocketError(socketException);
        if (Socket.s_LoggingEnabled)
          Logging.Exception(Logging.Sockets, (object) this, "SendFile", (Exception) socketException);
        throw socketException;
      }
      else
      {
        if ((overlappedAsyncResult.Flags & (TransmitFileOptions.Disconnect | TransmitFileOptions.ReuseSocket)) != TransmitFileOptions.UseDefaultWorkerThread)
        {
          this.SetToDisconnected();
          this.m_RemoteEndPoint = (EndPoint) null;
        }
        if (!Socket.s_LoggingEnabled)
          return;
        Logging.Exit(Logging.Sockets, (object) this, "SendFile", (object) socketError);
      }
    }

    public int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags)
    {
      SocketError errorCode;
      int num = this.Send(buffer, offset, size, socketFlags, out errorCode);
      if (errorCode != SocketError.Success)
        throw new SocketException(errorCode);
      else
        return num;
    }

    public unsafe int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "Send", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (buffer == null)
        throw new ArgumentNullException("buffer");
      if (offset < 0 || offset > buffer.Length)
        throw new ArgumentOutOfRangeException("offset");
      if (size < 0 || size > buffer.Length - offset)
        throw new ArgumentOutOfRangeException("size");
      errorCode = SocketError.Success;
      this.ValidateBlockingMode();
      int num;
      if (buffer.Length == 0)
      {
        num = UnsafeNclNativeMethods.OSSOCK.send(this.m_Handle.DangerousGetHandle(), (byte*) null, 0, socketFlags);
      }
      else
      {
        fixed (byte* numPtr = buffer)
          num = UnsafeNclNativeMethods.OSSOCK.send(this.m_Handle.DangerousGetHandle(), numPtr + offset, size, socketFlags);
      }
      if (num == -1)
      {
        errorCode = (SocketError) Marshal.GetLastWin32Error();
        this.UpdateStatusAfterSocketError(errorCode);
        if (Socket.s_LoggingEnabled)
        {
          Logging.Exception(Logging.Sockets, (object) this, "Send", (Exception) new SocketException(errorCode));
          Logging.Exit(Logging.Sockets, (object) this, "Send", (object) 0);
        }
        return 0;
      }
      else
      {
        if (Socket.s_PerfCountersEnabled && num > 0)
        {
          NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketBytesSent, (long) num);
          if (this.Transport == TransportType.Udp)
            NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketDatagramsSent);
        }
        if (Socket.s_LoggingEnabled)
          Logging.Dump(Logging.Sockets, (object) this, "Send", buffer, offset, size);
        if (Socket.s_LoggingEnabled)
          Logging.Exit(Logging.Sockets, (object) this, "Send", (object) num);
        return num;
      }
    }

    public unsafe int SendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEP)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "SendTo", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (buffer == null)
        throw new ArgumentNullException("buffer");
      if (remoteEP == null)
        throw new ArgumentNullException("remoteEP");
      if (offset < 0 || offset > buffer.Length)
        throw new ArgumentOutOfRangeException("offset");
      if (size < 0 || size > buffer.Length - offset)
        throw new ArgumentOutOfRangeException("size");
      this.ValidateBlockingMode();
      EndPoint remoteEP1 = remoteEP;
      SocketAddress socketAddress = this.CheckCacheRemote(ref remoteEP1, false);
      int num;
      if (buffer.Length == 0)
      {
        num = UnsafeNclNativeMethods.OSSOCK.sendto(this.m_Handle.DangerousGetHandle(), (byte*) null, 0, socketFlags, socketAddress.m_Buffer, socketAddress.m_Size);
      }
      else
      {
        fixed (byte* numPtr = buffer)
          num = UnsafeNclNativeMethods.OSSOCK.sendto(this.m_Handle.DangerousGetHandle(), numPtr + offset, size, socketFlags, socketAddress.m_Buffer, socketAddress.m_Size);
      }
      if (num == -1)
      {
        SocketException socketException = new SocketException();
        this.UpdateStatusAfterSocketError(socketException);
        if (Socket.s_LoggingEnabled)
          Logging.Exception(Logging.Sockets, (object) this, "SendTo", (Exception) socketException);
        throw socketException;
      }
      else
      {
        if (this.m_RightEndPoint == null)
          this.m_RightEndPoint = remoteEP1;
        if (Socket.s_PerfCountersEnabled && num > 0)
        {
          NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketBytesSent, (long) num);
          if (this.Transport == TransportType.Udp)
            NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketDatagramsSent);
        }
        if (Socket.s_LoggingEnabled)
          Logging.Dump(Logging.Sockets, (object) this, "SendTo", buffer, offset, size);
        if (Socket.s_LoggingEnabled)
          Logging.Exit(Logging.Sockets, (object) this, "SendTo", (object) num);
        return num;
      }
    }

    public int SendTo(byte[] buffer, int size, SocketFlags socketFlags, EndPoint remoteEP)
    {
      return this.SendTo(buffer, 0, size, socketFlags, remoteEP);
    }

    public int SendTo(byte[] buffer, SocketFlags socketFlags, EndPoint remoteEP)
    {
      return this.SendTo(buffer, 0, buffer != null ? buffer.Length : 0, socketFlags, remoteEP);
    }

    public int SendTo(byte[] buffer, EndPoint remoteEP)
    {
      return this.SendTo(buffer, 0, buffer != null ? buffer.Length : 0, SocketFlags.None, remoteEP);
    }

    public int Receive(byte[] buffer, int size, SocketFlags socketFlags)
    {
      return this.Receive(buffer, 0, size, socketFlags);
    }

    public int Receive(byte[] buffer, SocketFlags socketFlags)
    {
      return this.Receive(buffer, 0, buffer != null ? buffer.Length : 0, socketFlags);
    }

    public int Receive(byte[] buffer)
    {
      return this.Receive(buffer, 0, buffer != null ? buffer.Length : 0, SocketFlags.None);
    }

    public int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags)
    {
      SocketError errorCode;
      int num = this.Receive(buffer, offset, size, socketFlags, out errorCode);
      if (errorCode != SocketError.Success)
        throw new SocketException(errorCode);
      else
        return num;
    }

    public unsafe int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "Receive", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (buffer == null)
        throw new ArgumentNullException("buffer");
      if (offset < 0 || offset > buffer.Length)
        throw new ArgumentOutOfRangeException("offset");
      if (size < 0 || size > buffer.Length - offset)
        throw new ArgumentOutOfRangeException("size");
      this.ValidateBlockingMode();
      errorCode = SocketError.Success;
      int length;
      if (buffer.Length == 0)
      {
        length = UnsafeNclNativeMethods.OSSOCK.recv(this.m_Handle.DangerousGetHandle(), (byte*) null, 0, socketFlags);
      }
      else
      {
        fixed (byte* numPtr = buffer)
          length = UnsafeNclNativeMethods.OSSOCK.recv(this.m_Handle.DangerousGetHandle(), numPtr + offset, size, socketFlags);
      }
      if (length == -1)
      {
        errorCode = (SocketError) Marshal.GetLastWin32Error();
        this.UpdateStatusAfterSocketError(errorCode);
        if (Socket.s_LoggingEnabled)
        {
          Logging.Exception(Logging.Sockets, (object) this, "Receive", (Exception) new SocketException(errorCode));
          Logging.Exit(Logging.Sockets, (object) this, "Receive", (object) 0);
        }
        return 0;
      }
      else
      {
        if (Socket.s_PerfCountersEnabled)
        {
          bool flag = (socketFlags & SocketFlags.Peek) != SocketFlags.None;
          if (length > 0 && !flag)
          {
            NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketBytesReceived, (long) length);
            if (this.Transport == TransportType.Udp)
              NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketDatagramsReceived);
          }
        }
        if (Socket.s_LoggingEnabled)
          Logging.Dump(Logging.Sockets, (object) this, "Receive", buffer, offset, length);
        if (Socket.s_LoggingEnabled)
          Logging.Exit(Logging.Sockets, (object) this, "Receive", (object) length);
        return length;
      }
    }

    public int Receive(IList<ArraySegment<byte>> buffers)
    {
      return this.Receive(buffers, SocketFlags.None);
    }

    public int Receive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
    {
      SocketError errorCode;
      int num = this.Receive(buffers, socketFlags, out errorCode);
      if (errorCode != SocketError.Success)
        throw new SocketException(errorCode);
      else
        return num;
    }

    public int Receive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "Receive", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (buffers == null)
        throw new ArgumentNullException("buffers");
      if (buffers.Count == 0)
      {
        throw new ArgumentException(SR.GetString("net_sockets_zerolist", new object[1]
        {
          (object) "buffers"
        }), "buffers");
      }
      else
      {
        this.ValidateBlockingMode();
        int count = buffers.Count;
        WSABuffer[] buffers1 = new WSABuffer[count];
        GCHandle[] gcHandleArray = (GCHandle[]) null;
        errorCode = SocketError.Success;
        int bytesTransferred;
        try
        {
          gcHandleArray = new GCHandle[count];
          for (int index = 0; index < count; ++index)
          {
            ArraySegment<byte> segment = buffers[index];
            ValidationHelper.ValidateSegment(segment);
            gcHandleArray[index] = GCHandle.Alloc((object) segment.Array, GCHandleType.Pinned);
            buffers1[index].Length = segment.Count;
            buffers1[index].Pointer = Marshal.UnsafeAddrOfPinnedArrayElement((Array) segment.Array, segment.Offset);
          }
          errorCode = UnsafeNclNativeMethods.OSSOCK.WSARecv_Blocking(this.m_Handle.DangerousGetHandle(), buffers1, count, out bytesTransferred, out socketFlags, (SafeHandle) SafeNativeOverlapped.Zero, IntPtr.Zero);
          if (errorCode == SocketError.SocketError)
            errorCode = (SocketError) Marshal.GetLastWin32Error();
        }
        finally
        {
          if (gcHandleArray != null)
          {
            for (int index = 0; index < gcHandleArray.Length; ++index)
            {
              if (gcHandleArray[index].IsAllocated)
                gcHandleArray[index].Free();
            }
          }
        }
        if (errorCode != SocketError.Success)
        {
          this.UpdateStatusAfterSocketError(errorCode);
          if (Socket.s_LoggingEnabled)
          {
            Logging.Exception(Logging.Sockets, (object) this, "Receive", (Exception) new SocketException(errorCode));
            Logging.Exit(Logging.Sockets, (object) this, "Receive", (object) 0);
          }
          return 0;
        }
        else
        {
          if (Socket.s_PerfCountersEnabled)
          {
            bool flag = (socketFlags & SocketFlags.Peek) != SocketFlags.None;
            if (bytesTransferred > 0 && !flag)
            {
              NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketBytesReceived, (long) bytesTransferred);
              if (this.Transport == TransportType.Udp)
                NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketDatagramsReceived);
            }
          }
          if (Socket.s_LoggingEnabled)
            Logging.Exit(Logging.Sockets, (object) this, "Receive", (object) bytesTransferred);
          return bytesTransferred;
        }
      }
    }

    public int ReceiveMessageFrom(byte[] buffer, int offset, int size, ref SocketFlags socketFlags, ref EndPoint remoteEP, out IPPacketInformation ipPacketInformation)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "ReceiveMessageFrom", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (!ComNetOS.IsPostWin2K)
        throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
      if (buffer == null)
        throw new ArgumentNullException("buffer");
      if (remoteEP == null)
        throw new ArgumentNullException("remoteEP");
      if (remoteEP.AddressFamily != this.addressFamily)
      {
        throw new ArgumentException(SR.GetString("net_InvalidEndPointAddressFamily", (object) remoteEP.AddressFamily, (object) this.addressFamily), "remoteEP");
      }
      else
      {
        if (offset < 0 || offset > buffer.Length)
          throw new ArgumentOutOfRangeException("offset");
        if (size < 0 || size > buffer.Length - offset)
          throw new ArgumentOutOfRangeException("size");
        if (this.m_RightEndPoint == null)
          throw new InvalidOperationException(SR.GetString("net_sockets_mustbind"));
        this.ValidateBlockingMode();
        EndPoint remoteEP1 = remoteEP;
        SocketAddress socketAddress1 = this.SnapshotAndSerialize(ref remoteEP1);
        ReceiveMessageOverlappedAsyncResult overlappedAsyncResult = new ReceiveMessageOverlappedAsyncResult(this, (object) null, (AsyncCallback) null);
        overlappedAsyncResult.SetUnmanagedStructures(buffer, offset, size, socketAddress1, socketFlags);
        SocketAddress socketAddress2 = remoteEP1.Serialize();
        int bytesTransferred = 0;
        SocketError socketError = SocketError.Success;
        this.SetReceivingPacketInformation();
        try
        {
          if (this.WSARecvMsg_Blocking(this.m_Handle.DangerousGetHandle(), Marshal.UnsafeAddrOfPinnedArrayElement((Array) overlappedAsyncResult.m_MessageBuffer, 0), out bytesTransferred, IntPtr.Zero, IntPtr.Zero) == SocketError.SocketError)
            socketError = (SocketError) Marshal.GetLastWin32Error();
        }
        finally
        {
          overlappedAsyncResult.SyncReleaseUnmanagedStructures();
        }
        if (socketError != SocketError.Success && socketError != SocketError.MessageSize)
        {
          SocketException socketException = new SocketException(socketError);
          this.UpdateStatusAfterSocketError(socketException);
          if (Socket.s_LoggingEnabled)
            Logging.Exception(Logging.Sockets, (object) this, "ReceiveMessageFrom", (Exception) socketException);
          throw socketException;
        }
        else
        {
          if (!socketAddress2.Equals((object) overlappedAsyncResult.m_SocketAddress))
          {
            try
            {
              remoteEP = remoteEP1.Create(overlappedAsyncResult.m_SocketAddress);
            }
            catch
            {
            }
            if (this.m_RightEndPoint == null)
              this.m_RightEndPoint = remoteEP1;
          }
          socketFlags = overlappedAsyncResult.m_flags;
          ipPacketInformation = overlappedAsyncResult.m_IPPacketInformation;
          if (Socket.s_LoggingEnabled)
            Logging.Exit(Logging.Sockets, (object) this, "ReceiveMessageFrom", (object) socketError);
          return bytesTransferred;
        }
      }
    }

    public unsafe int ReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "ReceiveFrom", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (buffer == null)
        throw new ArgumentNullException("buffer");
      if (remoteEP == null)
        throw new ArgumentNullException("remoteEP");
      if (remoteEP.AddressFamily != this.addressFamily)
      {
        throw new ArgumentException(SR.GetString("net_InvalidEndPointAddressFamily", (object) remoteEP.AddressFamily, (object) this.addressFamily), "remoteEP");
      }
      else
      {
        if (offset < 0 || offset > buffer.Length)
          throw new ArgumentOutOfRangeException("offset");
        if (size < 0 || size > buffer.Length - offset)
          throw new ArgumentOutOfRangeException("size");
        if (this.m_RightEndPoint == null)
          throw new InvalidOperationException(SR.GetString("net_sockets_mustbind"));
        this.ValidateBlockingMode();
        EndPoint remoteEP1 = remoteEP;
        SocketAddress socketAddress1 = this.SnapshotAndSerialize(ref remoteEP1);
        SocketAddress socketAddress2 = remoteEP1.Serialize();
        int num;
        if (buffer.Length == 0)
        {
          num = UnsafeNclNativeMethods.OSSOCK.recvfrom(this.m_Handle.DangerousGetHandle(), (byte*) null, 0, socketFlags, socketAddress1.m_Buffer, out socketAddress1.m_Size);
        }
        else
        {
          fixed (byte* numPtr = buffer)
            num = UnsafeNclNativeMethods.OSSOCK.recvfrom(this.m_Handle.DangerousGetHandle(), numPtr + offset, size, socketFlags, socketAddress1.m_Buffer, out socketAddress1.m_Size);
        }
        SocketException socketException = (SocketException) null;
        if (num == -1)
        {
          socketException = new SocketException();
          this.UpdateStatusAfterSocketError(socketException);
          if (Socket.s_LoggingEnabled)
            Logging.Exception(Logging.Sockets, (object) this, "ReceiveFrom", (Exception) socketException);
          if (socketException.ErrorCode != 10040)
            throw socketException;
        }
        if (!socketAddress2.Equals((object) socketAddress1))
        {
          try
          {
            remoteEP = remoteEP1.Create(socketAddress1);
          }
          catch
          {
          }
          if (this.m_RightEndPoint == null)
            this.m_RightEndPoint = remoteEP1;
        }
        if (socketException != null)
          throw socketException;
        if (Socket.s_PerfCountersEnabled && num > 0)
        {
          NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketBytesReceived, (long) num);
          if (this.Transport == TransportType.Udp)
            NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketDatagramsReceived);
        }
        if (Socket.s_LoggingEnabled)
          Logging.Dump(Logging.Sockets, (object) this, "ReceiveFrom", buffer, offset, size);
        if (Socket.s_LoggingEnabled)
          Logging.Exit(Logging.Sockets, (object) this, "ReceiveFrom", (object) num);
        return num;
      }
    }

    public int ReceiveFrom(byte[] buffer, int size, SocketFlags socketFlags, ref EndPoint remoteEP)
    {
      return this.ReceiveFrom(buffer, 0, size, socketFlags, ref remoteEP);
    }

    public int ReceiveFrom(byte[] buffer, SocketFlags socketFlags, ref EndPoint remoteEP)
    {
      return this.ReceiveFrom(buffer, 0, buffer != null ? buffer.Length : 0, socketFlags, ref remoteEP);
    }

    public int ReceiveFrom(byte[] buffer, ref EndPoint remoteEP)
    {
      return this.ReceiveFrom(buffer, 0, buffer != null ? buffer.Length : 0, SocketFlags.None, ref remoteEP);
    }

    public int IOControl(int ioControlCode, byte[] optionInValue, byte[] optionOutValue)
    {
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (ioControlCode == -2147195266)
        throw new InvalidOperationException(SR.GetString("net_sockets_useblocking"));
      ExceptionHelper.UnmanagedPermission.Demand();
      int bytesTransferred = 0;
      if (UnsafeNclNativeMethods.OSSOCK.WSAIoctl_Blocking(this.m_Handle.DangerousGetHandle(), ioControlCode, optionInValue, optionInValue != null ? optionInValue.Length : 0, optionOutValue, optionOutValue != null ? optionOutValue.Length : 0, out bytesTransferred, (SafeHandle) SafeNativeOverlapped.Zero, IntPtr.Zero) != SocketError.SocketError)
        return bytesTransferred;
      SocketException socketException = new SocketException();
      this.UpdateStatusAfterSocketError(socketException);
      if (Socket.s_LoggingEnabled)
        Logging.Exception(Logging.Sockets, (object) this, "IOControl", (Exception) socketException);
      throw socketException;
    }

    public int IOControl(IOControlCode ioControlCode, byte[] optionInValue, byte[] optionOutValue)
    {
      return this.IOControl((int) ioControlCode, optionInValue, optionOutValue);
    }

    internal int IOControl(IOControlCode ioControlCode, IntPtr optionInValue, int inValueSize, IntPtr optionOutValue, int outValueSize)
    {
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if ((int) ioControlCode == -2147195266)
        throw new InvalidOperationException(SR.GetString("net_sockets_useblocking"));
      int bytesTransferred = 0;
      if (UnsafeNclNativeMethods.OSSOCK.WSAIoctl_Blocking_Internal(this.m_Handle.DangerousGetHandle(), (uint) ioControlCode, optionInValue, inValueSize, optionOutValue, outValueSize, out bytesTransferred, (SafeHandle) SafeNativeOverlapped.Zero, IntPtr.Zero) != SocketError.SocketError)
        return bytesTransferred;
      SocketException socketException = new SocketException();
      this.UpdateStatusAfterSocketError(socketException);
      if (Socket.s_LoggingEnabled)
        Logging.Exception(Logging.Sockets, (object) this, "IOControl", (Exception) socketException);
      throw socketException;
    }

    public void SetIPProtectionLevel(IPProtectionLevel level)
    {
      if (level == IPProtectionLevel.Unspecified)
        throw new ArgumentException(SR.GetString("net_sockets_invalid_optionValue_all"), "level");
      if (this.addressFamily == AddressFamily.InterNetworkV6)
      {
        this.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPProtectionLevel, (int) level);
      }
      else
      {
        if (this.addressFamily != AddressFamily.InterNetwork)
          throw new NotSupportedException(SR.GetString("net_invalidversion"));
        this.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.IPProtectionLevel, (int) level);
      }
    }

    public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionValue)
    {
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      this.CheckSetOptionPermissions(optionLevel, optionName);
      this.SetSocketOption(optionLevel, optionName, optionValue, false);
    }

    public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue)
    {
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      this.CheckSetOptionPermissions(optionLevel, optionName);
      if (UnsafeNclNativeMethods.OSSOCK.setsockopt(this.m_Handle, optionLevel, optionName, optionValue, optionValue != null ? optionValue.Length : 0) != SocketError.SocketError)
        return;
      SocketException socketException = new SocketException();
      this.UpdateStatusAfterSocketError(socketException);
      if (Socket.s_LoggingEnabled)
        Logging.Exception(Logging.Sockets, (object) this, "SetSocketOption", (Exception) socketException);
      throw socketException;
    }

    public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, bool optionValue)
    {
      this.SetSocketOption(optionLevel, optionName, optionValue ? 1 : 0);
    }

    public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, object optionValue)
    {
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (optionValue == null)
        throw new ArgumentNullException("optionValue");
      this.CheckSetOptionPermissions(optionLevel, optionName);
      if (optionLevel == SocketOptionLevel.Socket && optionName == SocketOptionName.Linger)
      {
        LingerOption lref = optionValue as LingerOption;
        if (lref == null)
          throw new ArgumentException(SR.GetString("net_sockets_invalid_optionValue", new object[1]
          {
            (object) "LingerOption"
          }), "optionValue");
        else if (lref.LingerTime < 0 || lref.LingerTime > (int) ushort.MaxValue)
          throw new ArgumentException(SR.GetString("ArgumentOutOfRange_Bounds_Lower_Upper", (object) 0, (object) (int) ushort.MaxValue), "optionValue.LingerTime");
        else
          this.setLingerOption(lref);
      }
      else if (optionLevel == SocketOptionLevel.IP && (optionName == SocketOptionName.AddMembership || optionName == SocketOptionName.DropMembership))
      {
        MulticastOption MR = optionValue as MulticastOption;
        if (MR == null)
          throw new ArgumentException(SR.GetString("net_sockets_invalid_optionValue", new object[1]
          {
            (object) "MulticastOption"
          }), "optionValue");
        else
          this.setMulticastOption(optionName, MR);
      }
      else
      {
        if (optionLevel != SocketOptionLevel.IPv6 || optionName != SocketOptionName.AddMembership && optionName != SocketOptionName.DropMembership)
          throw new ArgumentException(SR.GetString("net_sockets_invalid_optionValue_all"), "optionValue");
        IPv6MulticastOption MR = optionValue as IPv6MulticastOption;
        if (MR == null)
          throw new ArgumentException(SR.GetString("net_sockets_invalid_optionValue", new object[1]
          {
            (object) "IPv6MulticastOption"
          }), "optionValue");
        else
          this.setIPv6MulticastOption(optionName, MR);
      }
    }

    public object GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName)
    {
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (optionLevel == SocketOptionLevel.Socket && optionName == SocketOptionName.Linger)
        return (object) this.getLingerOpt();
      if (optionLevel == SocketOptionLevel.IP && (optionName == SocketOptionName.AddMembership || optionName == SocketOptionName.DropMembership))
        return (object) this.getMulticastOpt(optionName);
      if (optionLevel == SocketOptionLevel.IPv6 && (optionName == SocketOptionName.AddMembership || optionName == SocketOptionName.DropMembership))
        return (object) this.getIPv6MulticastOpt(optionName);
      int optionValue = 0;
      int optionLength = 4;
      if (UnsafeNclNativeMethods.OSSOCK.getsockopt(this.m_Handle, optionLevel, optionName, out optionValue, out optionLength) != SocketError.SocketError)
        return (object) optionValue;
      SocketException socketException = new SocketException();
      this.UpdateStatusAfterSocketError(socketException);
      if (Socket.s_LoggingEnabled)
        Logging.Exception(Logging.Sockets, (object) this, "GetSocketOption", (Exception) socketException);
      throw socketException;
    }

    public void GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue)
    {
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      int optionLength = optionValue != null ? optionValue.Length : 0;
      if (UnsafeNclNativeMethods.OSSOCK.getsockopt(this.m_Handle, optionLevel, optionName, optionValue, out optionLength) != SocketError.SocketError)
        return;
      SocketException socketException = new SocketException();
      this.UpdateStatusAfterSocketError(socketException);
      if (Socket.s_LoggingEnabled)
        Logging.Exception(Logging.Sockets, (object) this, "GetSocketOption", (Exception) socketException);
      throw socketException;
    }

    public byte[] GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionLength)
    {
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      byte[] optionValue = new byte[optionLength];
      int optionLength1 = optionLength;
      if (UnsafeNclNativeMethods.OSSOCK.getsockopt(this.m_Handle, optionLevel, optionName, optionValue, out optionLength1) == SocketError.SocketError)
      {
        SocketException socketException = new SocketException();
        this.UpdateStatusAfterSocketError(socketException);
        if (Socket.s_LoggingEnabled)
          Logging.Exception(Logging.Sockets, (object) this, "GetSocketOption", (Exception) socketException);
        throw socketException;
      }
      else
      {
        if (optionLength != optionLength1)
        {
          byte[] numArray = new byte[optionLength1];
          Buffer.BlockCopy((Array) optionValue, 0, (Array) numArray, 0, optionLength1);
          optionValue = numArray;
        }
        return optionValue;
      }
    }

    public bool Poll(int microSeconds, SelectMode mode)
    {
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      IntPtr handle = this.m_Handle.DangerousGetHandle();
      IntPtr[] numArray = new IntPtr[2]
      {
        (IntPtr) 1,
        handle
      };
      TimeValue timeValue = new TimeValue();
      int num;
      if (microSeconds != -1)
      {
        Socket.MicrosecondsToTimeValue((long) (uint) microSeconds, ref timeValue);
        num = UnsafeNclNativeMethods.OSSOCK.select(0, mode == SelectMode.SelectRead ? numArray : (IntPtr[]) null, mode == SelectMode.SelectWrite ? numArray : (IntPtr[]) null, mode == SelectMode.SelectError ? numArray : (IntPtr[]) null, ref timeValue);
      }
      else
        num = UnsafeNclNativeMethods.OSSOCK.select(0, mode == SelectMode.SelectRead ? numArray : (IntPtr[]) null, mode == SelectMode.SelectWrite ? numArray : (IntPtr[]) null, mode == SelectMode.SelectError ? numArray : (IntPtr[]) null, IntPtr.Zero);
      if (num == -1)
      {
        SocketException socketException = new SocketException();
        this.UpdateStatusAfterSocketError(socketException);
        if (Socket.s_LoggingEnabled)
          Logging.Exception(Logging.Sockets, (object) this, "Poll", (Exception) socketException);
        throw socketException;
      }
      else if ((int) numArray[0] == 0)
        return false;
      else
        return numArray[1] == handle;
    }

    public static void Select(IList checkRead, IList checkWrite, IList checkError, int microSeconds)
    {
      if ((checkRead == null || checkRead.Count == 0) && (checkWrite == null || checkWrite.Count == 0) && (checkError == null || checkError.Count == 0))
        throw new ArgumentNullException(SR.GetString("net_sockets_empty_select"));
      if (checkRead != null && checkRead.Count > 65536)
        throw new ArgumentOutOfRangeException("checkRead", SR.GetString("net_sockets_toolarge_select", (object) "checkRead", (object) 65536.ToString((IFormatProvider) NumberFormatInfo.CurrentInfo)));
      else if (checkWrite != null && checkWrite.Count > 65536)
        throw new ArgumentOutOfRangeException("checkWrite", SR.GetString("net_sockets_toolarge_select", (object) "checkWrite", (object) 65536.ToString((IFormatProvider) NumberFormatInfo.CurrentInfo)));
      else if (checkError != null && checkError.Count > 65536)
      {
        throw new ArgumentOutOfRangeException("checkError", SR.GetString("net_sockets_toolarge_select", (object) "checkError", (object) 65536.ToString((IFormatProvider) NumberFormatInfo.CurrentInfo)));
      }
      else
      {
        IntPtr[] numArray1 = Socket.SocketListToFileDescriptorSet(checkRead);
        IntPtr[] numArray2 = Socket.SocketListToFileDescriptorSet(checkWrite);
        IntPtr[] numArray3 = Socket.SocketListToFileDescriptorSet(checkError);
        int num;
        if (microSeconds != -1)
        {
          TimeValue timeValue = new TimeValue();
          Socket.MicrosecondsToTimeValue((long) (uint) microSeconds, ref timeValue);
          num = UnsafeNclNativeMethods.OSSOCK.select(0, numArray1, numArray2, numArray3, ref timeValue);
        }
        else
          num = UnsafeNclNativeMethods.OSSOCK.select(0, numArray1, numArray2, numArray3, IntPtr.Zero);
        if (num == -1)
          throw new SocketException();
        Socket.SelectFileDescriptor(checkRead, numArray1);
        Socket.SelectFileDescriptor(checkWrite, numArray2);
        Socket.SelectFileDescriptor(checkError, numArray3);
      }
    }

    [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
    public IAsyncResult BeginSendFile(string fileName, AsyncCallback callback, object state)
    {
      if (!ComNetOS.IsWinNt)
        return this.BeginDownLevelSendFile(fileName, true, callback, state);
      else
        return this.BeginSendFile(fileName, (byte[]) null, (byte[]) null, TransmitFileOptions.UseDefaultWorkerThread, callback, state);
    }

    [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
    public IAsyncResult BeginConnect(EndPoint remoteEP, AsyncCallback callback, object state)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "BeginConnect", (object) remoteEP);
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (remoteEP == null)
        throw new ArgumentNullException("remoteEP");
      if (this.isListening)
        throw new InvalidOperationException(SR.GetString("net_sockets_mustnotlisten"));
      DnsEndPoint dnsEndPoint = remoteEP as DnsEndPoint;
      if (dnsEndPoint != null)
      {
        if (dnsEndPoint.AddressFamily != AddressFamily.Unspecified && dnsEndPoint.AddressFamily != this.addressFamily)
          throw new NotSupportedException(SR.GetString("net_invalidversion"));
        else
          return this.BeginConnect(dnsEndPoint.Host, dnsEndPoint.Port, callback, state);
      }
      else
      {
        if (this.CanUseConnectEx(remoteEP))
          return this.BeginConnectEx(remoteEP, true, callback, state);
        EndPoint remoteEP1 = remoteEP;
        SocketAddress socketAddress = this.CheckCacheRemote(ref remoteEP1, true);
        ConnectAsyncResult connectAsyncResult = new ConnectAsyncResult((object) this, remoteEP1, state, callback);
        connectAsyncResult.StartPostingAsyncOp(false);
        this.DoBeginConnect(remoteEP1, socketAddress, (LazyAsyncResult) connectAsyncResult);
        connectAsyncResult.FinishPostingAsyncOp(ref this.Caches.ConnectClosureCache);
        if (Socket.s_LoggingEnabled)
          Logging.Exit(Logging.Sockets, (object) this, "BeginConnect", (object) connectAsyncResult);
        return (IAsyncResult) connectAsyncResult;
      }
    }

    public unsafe SocketInformation DuplicateAndClose(int targetProcessId)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "DuplicateAndClose", (string) null);
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      ExceptionHelper.UnrestrictedSocketPermission.Demand();
      SocketInformation socketInformation = new SocketInformation();
      socketInformation.ProtocolInformation = new byte[Socket.protocolInformationSize];
      SocketError socketError;
      fixed (byte* pinnedBuffer = socketInformation.ProtocolInformation)
        socketError = (SocketError) UnsafeNclNativeMethods.OSSOCK.WSADuplicateSocket(this.m_Handle, (uint) targetProcessId, pinnedBuffer);
      if (socketError != SocketError.Success)
      {
        SocketException socketException = new SocketException();
        if (Socket.s_LoggingEnabled)
          Logging.Exception(Logging.Sockets, (object) this, "DuplicateAndClose", (Exception) socketException);
        throw socketException;
      }
      else
      {
        socketInformation.IsConnected = this.Connected;
        socketInformation.IsNonBlocking = !this.Blocking;
        socketInformation.IsListening = this.isListening;
        socketInformation.UseOnlyOverlappedIO = this.UseOnlyOverlappedIO;
        socketInformation.RemoteEndPoint = this.m_RemoteEndPoint;
        this.Close(-1);
        if (Socket.s_LoggingEnabled)
          Logging.Exit(Logging.Sockets, (object) this, "DuplicateAndClose", (string) null);
        return socketInformation;
      }
    }

    internal IAsyncResult UnsafeBeginConnect(EndPoint remoteEP, AsyncCallback callback, object state)
    {
      if (this.CanUseConnectEx(remoteEP))
        return this.BeginConnectEx(remoteEP, false, callback, state);
      EndPoint remoteEP1 = remoteEP;
      SocketAddress socketAddress = this.SnapshotAndSerialize(ref remoteEP1);
      ConnectAsyncResult connectAsyncResult = new ConnectAsyncResult((object) this, remoteEP1, state, callback);
      this.DoBeginConnect(remoteEP1, socketAddress, (LazyAsyncResult) connectAsyncResult);
      return (IAsyncResult) connectAsyncResult;
    }

    private void DoBeginConnect(EndPoint endPointSnapshot, SocketAddress socketAddress, LazyAsyncResult asyncResult)
    {
      EndPoint endPoint = this.m_RightEndPoint;
      if (this.m_AcceptQueueOrConnectResult != null)
        throw new InvalidOperationException(SR.GetString("net_sockets_no_duplicate_async"));
      this.m_AcceptQueueOrConnectResult = (object) asyncResult;
      if (!this.SetAsyncEventSelect(AsyncEventBits.FdConnect))
      {
        this.m_AcceptQueueOrConnectResult = (object) null;
        throw new ObjectDisposedException(this.GetType().FullName);
      }
      else
      {
        IntPtr handle = this.m_Handle.DangerousGetHandle();
        if (this.m_RightEndPoint == null)
          this.m_RightEndPoint = endPointSnapshot;
        SocketError socketError = UnsafeNclNativeMethods.OSSOCK.WSAConnect(handle, socketAddress.m_Buffer, socketAddress.m_Size, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
        if (socketError != SocketError.Success)
          socketError = (SocketError) Marshal.GetLastWin32Error();
        if (socketError == SocketError.WouldBlock)
          return;
        bool flag = true;
        if (socketError == SocketError.Success)
          this.SetToConnected();
        else
          asyncResult.ErrorCode = (int) socketError;
        if (Interlocked.Exchange<RegisteredWaitHandle>(ref this.m_RegisteredWait, (RegisteredWaitHandle) null) == null)
          flag = false;
        this.UnsetAsyncEventSelect();
        if (socketError == SocketError.Success)
        {
          if (!flag)
            return;
          asyncResult.InvokeCallback();
        }
        else
        {
          this.m_RightEndPoint = endPoint;
          SocketException socketException = new SocketException(socketError);
          this.UpdateStatusAfterSocketError(socketException);
          this.m_AcceptQueueOrConnectResult = (object) null;
          if (Socket.s_LoggingEnabled)
            Logging.Exception(Logging.Sockets, (object) this, "BeginConnect", (Exception) socketException);
          throw socketException;
        }
      }
    }

    private bool CanUseConnectEx(EndPoint remoteEP)
    {
      if (!ComNetOS.IsPostWin2K || this.socketType != SocketType.Stream || this.m_RightEndPoint == null && !(remoteEP.GetType() == typeof (IPEndPoint)))
        return false;
      if (!Thread.CurrentThread.IsThreadPoolThread && !SettingsSectionInternal.Section.AlwaysUseCompletionPortsForConnect)
        return this.m_IsDisconnected;
      else
        return true;
    }

    private void ConnectCallback()
    {
      LazyAsyncResult lazyAsyncResult = (LazyAsyncResult) this.m_AcceptQueueOrConnectResult;
      if (lazyAsyncResult.InternalPeekCompleted)
        return;
      NetworkEvents networkEvents = new NetworkEvents();
      networkEvents.Events = AsyncEventBits.FdConnect;
      SocketError socketError = SocketError.OperationAborted;
      object result = (object) null;
      try
      {
        if (!this.CleanedUp)
        {
          try
          {
            socketError = UnsafeNclNativeMethods.OSSOCK.WSAEnumNetworkEvents(this.m_Handle, this.m_AsyncEvent.SafeWaitHandle, out networkEvents);
            socketError = socketError == SocketError.Success ? (SocketError) networkEvents.ErrorCodes[4] : (SocketError) Marshal.GetLastWin32Error();
            this.UnsetAsyncEventSelect();
          }
          catch (ObjectDisposedException ex)
          {
            socketError = SocketError.OperationAborted;
          }
        }
        if (socketError == SocketError.Success)
          this.SetToConnected();
      }
      catch (Exception ex)
      {
        if (NclUtilities.IsFatal(ex))
          throw;
        else
          result = (object) ex;
      }
      if (lazyAsyncResult.InternalPeekCompleted)
        return;
      lazyAsyncResult.ErrorCode = (int) socketError;
      lazyAsyncResult.InvokeCallback(result);
    }

    [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
    public IAsyncResult BeginConnect(string host, int port, AsyncCallback requestCallback, object state)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "BeginConnect", host);
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (host == null)
        throw new ArgumentNullException("host");
      if (!ValidationHelper.ValidateTcpPort(port))
        throw new ArgumentOutOfRangeException("port");
      if (this.addressFamily != AddressFamily.InterNetwork && this.addressFamily != AddressFamily.InterNetworkV6)
        throw new NotSupportedException(SR.GetString("net_invalidversion"));
      if (this.isListening)
        throw new InvalidOperationException(SR.GetString("net_sockets_mustnotlisten"));
      Socket.MultipleAddressConnectAsyncResult context = new Socket.MultipleAddressConnectAsyncResult((IPAddress[]) null, port, this, state, requestCallback);
      context.StartPostingAsyncOp(false);
      IAsyncResult hostAddresses = Dns.UnsafeBeginGetHostAddresses(host, new AsyncCallback(Socket.DnsCallback), (object) context);
      if (hostAddresses.CompletedSynchronously && Socket.DoDnsCallback(hostAddresses, context))
        context.InvokeCallback();
      context.FinishPostingAsyncOp(ref this.Caches.ConnectClosureCache);
      if (Socket.s_LoggingEnabled)
        Logging.Exit(Logging.Sockets, (object) this, "BeginConnect", (object) context);
      return (IAsyncResult) context;
    }

    [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
    public IAsyncResult BeginConnect(IPAddress address, int port, AsyncCallback requestCallback, object state)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "BeginConnect", (object) address);
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (address == null)
        throw new ArgumentNullException("address");
      if (!ValidationHelper.ValidateTcpPort(port))
        throw new ArgumentOutOfRangeException("port");
      if (this.addressFamily != address.AddressFamily)
        throw new NotSupportedException(SR.GetString("net_invalidversion"));
      IAsyncResult asyncResult = this.BeginConnect((EndPoint) new IPEndPoint(address, port), requestCallback, state);
      if (Socket.s_LoggingEnabled)
        Logging.Exit(Logging.Sockets, (object) this, "BeginConnect", (object) asyncResult);
      return asyncResult;
    }

    [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
    public IAsyncResult BeginConnect(IPAddress[] addresses, int port, AsyncCallback requestCallback, object state)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "BeginConnect", (object) addresses);
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (addresses == null)
        throw new ArgumentNullException("addresses");
      if (addresses.Length == 0)
        throw new ArgumentException(SR.GetString("net_invalidAddressList"), "addresses");
      if (!ValidationHelper.ValidateTcpPort(port))
        throw new ArgumentOutOfRangeException("port");
      if (this.addressFamily != AddressFamily.InterNetwork && this.addressFamily != AddressFamily.InterNetworkV6)
        throw new NotSupportedException(SR.GetString("net_invalidversion"));
      if (this.isListening)
        throw new InvalidOperationException(SR.GetString("net_sockets_mustnotlisten"));
      Socket.MultipleAddressConnectAsyncResult context = new Socket.MultipleAddressConnectAsyncResult(addresses, port, this, state, requestCallback);
      context.StartPostingAsyncOp(false);
      if (Socket.DoMultipleAddressConnectCallback(Socket.PostOneBeginConnect(context), context))
        context.InvokeCallback();
      context.FinishPostingAsyncOp(ref this.Caches.ConnectClosureCache);
      if (Socket.s_LoggingEnabled)
        Logging.Exit(Logging.Sockets, (object) this, "BeginConnect", (object) context);
      return (IAsyncResult) context;
    }

    [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
    public IAsyncResult BeginDisconnect(bool reuseSocket, AsyncCallback callback, object state)
    {
      DisconnectOverlappedAsyncResult asyncResult = new DisconnectOverlappedAsyncResult(this, state, callback);
      asyncResult.StartPostingAsyncOp(false);
      this.DoBeginDisconnect(reuseSocket, asyncResult);
      asyncResult.FinishPostingAsyncOp();
      return (IAsyncResult) asyncResult;
    }

    private void DoBeginDisconnect(bool reuseSocket, DisconnectOverlappedAsyncResult asyncResult)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "BeginDisconnect", (string) null);
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (!ComNetOS.IsPostWin2K)
        throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
      asyncResult.SetUnmanagedStructures((object) null);
      SocketError errorCode = SocketError.Success;
      if (!this.DisconnectEx(this.m_Handle, asyncResult.OverlappedHandle, reuseSocket ? 2 : 0, 0))
        errorCode = (SocketError) Marshal.GetLastWin32Error();
      if (errorCode == SocketError.Success)
      {
        this.SetToDisconnected();
        this.m_RemoteEndPoint = (EndPoint) null;
      }
      SocketError socketError = asyncResult.CheckAsyncCallOverlappedResult(errorCode);
      if (socketError != SocketError.Success)
      {
        SocketException socketException = new SocketException(socketError);
        this.UpdateStatusAfterSocketError(socketException);
        if (Socket.s_LoggingEnabled)
          Logging.Exception(Logging.Sockets, (object) this, "BeginDisconnect", (Exception) socketException);
        throw socketException;
      }
      else
      {
        if (!Socket.s_LoggingEnabled)
          return;
        Logging.Exit(Logging.Sockets, (object) this, "BeginDisconnect", (object) asyncResult);
      }
    }

    public void Disconnect(bool reuseSocket)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "Disconnect", (string) null);
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (!ComNetOS.IsPostWin2K)
        throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
      SocketError socketError = SocketError.Success;
      if (!this.DisconnectEx_Blocking(this.m_Handle.DangerousGetHandle(), IntPtr.Zero, reuseSocket ? 2 : 0, 0))
        socketError = (SocketError) Marshal.GetLastWin32Error();
      if (socketError != SocketError.Success)
      {
        SocketException socketException = new SocketException(socketError);
        this.UpdateStatusAfterSocketError(socketException);
        if (Socket.s_LoggingEnabled)
          Logging.Exception(Logging.Sockets, (object) this, "Disconnect", (Exception) socketException);
        throw socketException;
      }
      else
      {
        this.SetToDisconnected();
        this.m_RemoteEndPoint = (EndPoint) null;
        if (!Socket.s_LoggingEnabled)
          return;
        Logging.Exit(Logging.Sockets, (object) this, "Disconnect", (string) null);
      }
    }

    public void EndConnect(IAsyncResult asyncResult)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "EndConnect", (object) asyncResult);
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (asyncResult == null)
        throw new ArgumentNullException("asyncResult");
      LazyAsyncResult lazyAsyncResult = (LazyAsyncResult) null;
      EndPoint endPoint = (EndPoint) null;
      ConnectOverlappedAsyncResult overlappedAsyncResult = asyncResult as ConnectOverlappedAsyncResult;
      if (overlappedAsyncResult == null)
      {
        Socket.MultipleAddressConnectAsyncResult connectAsyncResult1 = asyncResult as Socket.MultipleAddressConnectAsyncResult;
        if (connectAsyncResult1 == null)
        {
          ConnectAsyncResult connectAsyncResult2 = asyncResult as ConnectAsyncResult;
          if (connectAsyncResult2 != null)
          {
            endPoint = connectAsyncResult2.RemoteEndPoint;
            lazyAsyncResult = (LazyAsyncResult) connectAsyncResult2;
          }
        }
        else
        {
          endPoint = connectAsyncResult1.RemoteEndPoint;
          lazyAsyncResult = (LazyAsyncResult) connectAsyncResult1;
        }
      }
      else
      {
        endPoint = overlappedAsyncResult.RemoteEndPoint;
        lazyAsyncResult = (LazyAsyncResult) overlappedAsyncResult;
      }
      if (lazyAsyncResult == null || lazyAsyncResult.AsyncObject != this)
        throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
      if (lazyAsyncResult.EndCalled)
      {
        throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[1]
        {
          (object) "EndConnect"
        }));
      }
      else
      {
        lazyAsyncResult.InternalWaitForCompletion();
        lazyAsyncResult.EndCalled = true;
        this.m_AcceptQueueOrConnectResult = (object) null;
        if (lazyAsyncResult.Result is Exception)
        {
          if (Socket.s_LoggingEnabled)
            Logging.Exception(Logging.Sockets, (object) this, "EndConnect", (Exception) lazyAsyncResult.Result);
          throw (Exception) lazyAsyncResult.Result;
        }
        else if (lazyAsyncResult.ErrorCode != 0)
        {
          SocketException socketException = new SocketException(lazyAsyncResult.ErrorCode, endPoint);
          this.UpdateStatusAfterSocketError(socketException);
          if (Socket.s_LoggingEnabled)
            Logging.Exception(Logging.Sockets, (object) this, "EndConnect", (Exception) socketException);
          throw socketException;
        }
        else
        {
          if (!Socket.s_LoggingEnabled)
            return;
          Logging.PrintInfo(Logging.Sockets, (object) this, SR.GetString("net_log_socket_connected", (object) this.LocalEndPoint, (object) this.RemoteEndPoint));
          Logging.Exit(Logging.Sockets, (object) this, "EndConnect", "");
        }
      }
    }

    public void EndDisconnect(IAsyncResult asyncResult)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "EndDisconnect", (object) asyncResult);
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (!ComNetOS.IsPostWin2K)
        throw new PlatformNotSupportedException(SR.GetString("WinNTRequired"));
      if (asyncResult == null)
        throw new ArgumentNullException("asyncResult");
      LazyAsyncResult lazyAsyncResult = asyncResult as LazyAsyncResult;
      if (lazyAsyncResult == null || lazyAsyncResult.AsyncObject != this)
        throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
      if (lazyAsyncResult.EndCalled)
      {
        throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[1]
        {
          (object) "EndDisconnect"
        }));
      }
      else
      {
        lazyAsyncResult.InternalWaitForCompletion();
        lazyAsyncResult.EndCalled = true;
        if (lazyAsyncResult.ErrorCode != 0)
        {
          SocketException socketException = new SocketException(lazyAsyncResult.ErrorCode);
          this.UpdateStatusAfterSocketError(socketException);
          if (Socket.s_LoggingEnabled)
            Logging.Exception(Logging.Sockets, (object) this, "EndDisconnect", (Exception) socketException);
          throw socketException;
        }
        else
        {
          if (!Socket.s_LoggingEnabled)
            return;
          Logging.Exit(Logging.Sockets, (object) this, "EndDisconnect", (string) null);
        }
      }
    }

    [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
    public IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
    {
      SocketError errorCode;
      IAsyncResult asyncResult = this.BeginSend(buffer, offset, size, socketFlags, out errorCode, callback, state);
      if (errorCode != SocketError.Success && errorCode != SocketError.IOPending)
        throw new SocketException(errorCode);
      else
        return asyncResult;
    }

    [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
    public IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "BeginSend", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (buffer == null)
        throw new ArgumentNullException("buffer");
      if (offset < 0 || offset > buffer.Length)
        throw new ArgumentOutOfRangeException("offset");
      if (size < 0 || size > buffer.Length - offset)
        throw new ArgumentOutOfRangeException("size");
      OverlappedAsyncResult asyncResult = new OverlappedAsyncResult(this, state, callback);
      asyncResult.StartPostingAsyncOp(false);
      errorCode = this.DoBeginSend(buffer, offset, size, socketFlags, asyncResult);
      if (errorCode != SocketError.Success && errorCode != SocketError.IOPending)
        asyncResult = (OverlappedAsyncResult) null;
      else
        asyncResult.FinishPostingAsyncOp(ref this.Caches.SendClosureCache);
      if (Socket.s_LoggingEnabled)
        Logging.Exit(Logging.Sockets, (object) this, "BeginSend", (object) asyncResult);
      return (IAsyncResult) asyncResult;
    }

    internal IAsyncResult UnsafeBeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "UnsafeBeginSend", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      OverlappedAsyncResult asyncResult = new OverlappedAsyncResult(this, state, callback);
      SocketError socketError = this.DoBeginSend(buffer, offset, size, socketFlags, asyncResult);
      switch (socketError)
      {
        case SocketError.Success:
        case SocketError.IOPending:
          if (Socket.s_LoggingEnabled)
            Logging.Exit(Logging.Sockets, (object) this, "UnsafeBeginSend", (object) asyncResult);
          return (IAsyncResult) asyncResult;
        default:
          throw new SocketException(socketError);
      }
    }

    private SocketError DoBeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, OverlappedAsyncResult asyncResult)
    {
      SocketError socketError = SocketError.SocketError;
      try
      {
        asyncResult.SetUnmanagedStructures(buffer, offset, size, (SocketAddress) null, false, ref this.Caches.SendOverlappedCache);
        int bytesTransferred;
        socketError = UnsafeNclNativeMethods.OSSOCK.WSASend(this.m_Handle, ref asyncResult.m_SingleBuffer, 1, out bytesTransferred, socketFlags, asyncResult.OverlappedHandle, IntPtr.Zero);
        if (socketError != SocketError.Success)
          socketError = (SocketError) Marshal.GetLastWin32Error();
      }
      finally
      {
        socketError = asyncResult.CheckAsyncCallOverlappedResult(socketError);
      }
      if (socketError != SocketError.Success)
      {
        asyncResult.ExtractCache(ref this.Caches.SendOverlappedCache);
        this.UpdateStatusAfterSocketError(socketError);
        if (Socket.s_LoggingEnabled)
          Logging.Exception(Logging.Sockets, (object) this, "BeginSend", (Exception) new SocketException(socketError));
      }
      return socketError;
    }

    [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
    public IAsyncResult BeginSendFile(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags, AsyncCallback callback, object state)
    {
      TransmitFileOverlappedAsyncResult asyncResult = new TransmitFileOverlappedAsyncResult(this, state, callback);
      asyncResult.StartPostingAsyncOp(false);
      this.DoBeginSendFile(fileName, preBuffer, postBuffer, flags, asyncResult);
      asyncResult.FinishPostingAsyncOp(ref this.Caches.SendClosureCache);
      return (IAsyncResult) asyncResult;
    }

    private void DoBeginSendFile(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags, TransmitFileOverlappedAsyncResult asyncResult)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "BeginSendFile", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (!ComNetOS.IsWinNt)
        throw new PlatformNotSupportedException(SR.GetString("WinNTRequired"));
      if (!this.Connected)
        throw new NotSupportedException(SR.GetString("net_notconnected"));
      FileStream fileStream = (FileStream) null;
      if (fileName != null && fileName.Length > 0)
        fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
      SafeHandle fileHandle = (SafeHandle) null;
      if (fileStream != null)
      {
        ExceptionHelper.UnmanagedPermission.Assert();
        try
        {
          fileHandle = (SafeHandle) fileStream.SafeFileHandle;
        }
        finally
        {
          CodeAccessPermission.RevertAssert();
        }
      }
      SocketError socketError = SocketError.SocketError;
      try
      {
        asyncResult.SetUnmanagedStructures(preBuffer, postBuffer, fileStream, flags, ref this.Caches.SendOverlappedCache);
        socketError = (fileHandle == null ? UnsafeNclNativeMethods.OSSOCK.TransmitFile2(this.m_Handle, IntPtr.Zero, 0, 0, asyncResult.OverlappedHandle, asyncResult.TransmitFileBuffers, flags) : UnsafeNclNativeMethods.OSSOCK.TransmitFile(this.m_Handle, fileHandle, 0, 0, asyncResult.OverlappedHandle, asyncResult.TransmitFileBuffers, flags)) ? SocketError.Success : (SocketError) Marshal.GetLastWin32Error();
      }
      finally
      {
        socketError = asyncResult.CheckAsyncCallOverlappedResult(socketError);
      }
      if (socketError != SocketError.Success)
      {
        asyncResult.ExtractCache(ref this.Caches.SendOverlappedCache);
        SocketException socketException = new SocketException(socketError);
        this.UpdateStatusAfterSocketError(socketException);
        if (Socket.s_LoggingEnabled)
          Logging.Exception(Logging.Sockets, (object) this, "BeginSendFile", (Exception) socketException);
        throw socketException;
      }
      else
      {
        if (!Socket.s_LoggingEnabled)
          return;
        Logging.Exit(Logging.Sockets, (object) this, "BeginSendFile", (object) socketError);
      }
    }

    [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
    public IAsyncResult BeginSend(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, AsyncCallback callback, object state)
    {
      SocketError errorCode;
      IAsyncResult asyncResult = this.BeginSend(buffers, socketFlags, out errorCode, callback, state);
      if (errorCode != SocketError.Success && errorCode != SocketError.IOPending)
        throw new SocketException(errorCode);
      else
        return asyncResult;
    }

    [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
    public IAsyncResult BeginSend(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "BeginSend", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (buffers == null)
        throw new ArgumentNullException("buffers");
      if (buffers.Count == 0)
      {
        throw new ArgumentException(SR.GetString("net_sockets_zerolist", new object[1]
        {
          (object) "buffers"
        }), "buffers");
      }
      else
      {
        OverlappedAsyncResult asyncResult = new OverlappedAsyncResult(this, state, callback);
        asyncResult.StartPostingAsyncOp(false);
        errorCode = this.DoBeginSend(buffers, socketFlags, asyncResult);
        asyncResult.FinishPostingAsyncOp(ref this.Caches.SendClosureCache);
        if (errorCode != SocketError.Success && errorCode != SocketError.IOPending)
          asyncResult = (OverlappedAsyncResult) null;
        if (Socket.s_LoggingEnabled)
          Logging.Exit(Logging.Sockets, (object) this, "BeginSend", (object) asyncResult);
        return (IAsyncResult) asyncResult;
      }
    }

    private SocketError DoBeginSend(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, OverlappedAsyncResult asyncResult)
    {
      SocketError socketError = SocketError.SocketError;
      try
      {
        asyncResult.SetUnmanagedStructures(buffers, ref this.Caches.SendOverlappedCache);
        int bytesTransferred;
        socketError = UnsafeNclNativeMethods.OSSOCK.WSASend(this.m_Handle, asyncResult.m_WSABuffers, asyncResult.m_WSABuffers.Length, out bytesTransferred, socketFlags, asyncResult.OverlappedHandle, IntPtr.Zero);
        if (socketError != SocketError.Success)
          socketError = (SocketError) Marshal.GetLastWin32Error();
      }
      finally
      {
        socketError = asyncResult.CheckAsyncCallOverlappedResult(socketError);
      }
      if (socketError != SocketError.Success)
      {
        asyncResult.ExtractCache(ref this.Caches.SendOverlappedCache);
        this.UpdateStatusAfterSocketError(socketError);
        if (Socket.s_LoggingEnabled)
          Logging.Exception(Logging.Sockets, (object) this, "BeginSend", (Exception) new SocketException(socketError));
      }
      return socketError;
    }

    public int EndSend(IAsyncResult asyncResult)
    {
      SocketError errorCode;
      int num = this.EndSend(asyncResult, out errorCode);
      if (errorCode != SocketError.Success)
        throw new SocketException(errorCode);
      else
        return num;
    }

    public int EndSend(IAsyncResult asyncResult, out SocketError errorCode)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "EndSend", (object) asyncResult);
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (asyncResult == null)
        throw new ArgumentNullException("asyncResult");
      OverlappedAsyncResult overlappedAsyncResult = asyncResult as OverlappedAsyncResult;
      if (overlappedAsyncResult == null || overlappedAsyncResult.AsyncObject != this)
        throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
      if (overlappedAsyncResult.EndCalled)
      {
        throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[1]
        {
          (object) "EndSend"
        }));
      }
      else
      {
        int num = (int) overlappedAsyncResult.InternalWaitForCompletion();
        overlappedAsyncResult.EndCalled = true;
        overlappedAsyncResult.ExtractCache(ref this.Caches.SendOverlappedCache);
        if (Socket.s_PerfCountersEnabled && num > 0)
        {
          NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketBytesSent, (long) num);
          if (this.Transport == TransportType.Udp)
            NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketDatagramsSent);
        }
        errorCode = (SocketError) overlappedAsyncResult.ErrorCode;
        if (errorCode != SocketError.Success)
        {
          this.UpdateStatusAfterSocketError(errorCode);
          if (Socket.s_LoggingEnabled)
          {
            Logging.Exception(Logging.Sockets, (object) this, "EndSend", (Exception) new SocketException(errorCode));
            Logging.Exit(Logging.Sockets, (object) this, "EndSend", (object) 0);
          }
          return 0;
        }
        else
        {
          if (Socket.s_LoggingEnabled)
            Logging.Exit(Logging.Sockets, (object) this, "EndSend", (object) num);
          return num;
        }
      }
    }

    public void EndSendFile(IAsyncResult asyncResult)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "EndSendFile", (object) asyncResult);
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (!ComNetOS.IsWinNt)
      {
        this.EndDownLevelSendFile(asyncResult);
      }
      else
      {
        if (!ComNetOS.IsWinNt)
          throw new PlatformNotSupportedException(SR.GetString("WinNTRequired"));
        if (asyncResult == null)
          throw new ArgumentNullException("asyncResult");
        TransmitFileOverlappedAsyncResult overlappedAsyncResult = asyncResult as TransmitFileOverlappedAsyncResult;
        if (overlappedAsyncResult == null || overlappedAsyncResult.AsyncObject != this)
          throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
        if (overlappedAsyncResult.EndCalled)
        {
          throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[1]
          {
            (object) "EndSendFile"
          }));
        }
        else
        {
          overlappedAsyncResult.InternalWaitForCompletion();
          overlappedAsyncResult.EndCalled = true;
          overlappedAsyncResult.ExtractCache(ref this.Caches.SendOverlappedCache);
          if ((overlappedAsyncResult.Flags & (TransmitFileOptions.Disconnect | TransmitFileOptions.ReuseSocket)) != TransmitFileOptions.UseDefaultWorkerThread)
          {
            this.SetToDisconnected();
            this.m_RemoteEndPoint = (EndPoint) null;
          }
          if (overlappedAsyncResult.ErrorCode != 0)
          {
            SocketException socketException = new SocketException(overlappedAsyncResult.ErrorCode);
            this.UpdateStatusAfterSocketError(socketException);
            if (Socket.s_LoggingEnabled)
              Logging.Exception(Logging.Sockets, (object) this, "EndSendFile", (Exception) socketException);
            throw socketException;
          }
          else
          {
            if (!Socket.s_LoggingEnabled)
              return;
            Logging.Exit(Logging.Sockets, (object) this, "EndSendFile", "");
          }
        }
      }
    }

    [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
    public IAsyncResult BeginSendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEP, AsyncCallback callback, object state)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "BeginSendTo", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (buffer == null)
        throw new ArgumentNullException("buffer");
      if (remoteEP == null)
        throw new ArgumentNullException("remoteEP");
      if (offset < 0 || offset > buffer.Length)
        throw new ArgumentOutOfRangeException("offset");
      if (size < 0 || size > buffer.Length - offset)
        throw new ArgumentOutOfRangeException("size");
      EndPoint remoteEP1 = remoteEP;
      SocketAddress socketAddress = this.CheckCacheRemote(ref remoteEP1, false);
      OverlappedAsyncResult asyncResult = new OverlappedAsyncResult(this, state, callback);
      asyncResult.StartPostingAsyncOp(false);
      this.DoBeginSendTo(buffer, offset, size, socketFlags, remoteEP1, socketAddress, asyncResult);
      asyncResult.FinishPostingAsyncOp(ref this.Caches.SendClosureCache);
      if (Socket.s_LoggingEnabled)
        Logging.Exit(Logging.Sockets, (object) this, "BeginSendTo", (object) asyncResult);
      return (IAsyncResult) asyncResult;
    }

    private void DoBeginSendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint endPointSnapshot, SocketAddress socketAddress, OverlappedAsyncResult asyncResult)
    {
      EndPoint endPoint = this.m_RightEndPoint;
      SocketError socketError = SocketError.SocketError;
      try
      {
        asyncResult.SetUnmanagedStructures(buffer, offset, size, socketAddress, false, ref this.Caches.SendOverlappedCache);
        if (this.m_RightEndPoint == null)
          this.m_RightEndPoint = endPointSnapshot;
        int bytesTransferred;
        socketError = UnsafeNclNativeMethods.OSSOCK.WSASendTo(this.m_Handle, ref asyncResult.m_SingleBuffer, 1, out bytesTransferred, socketFlags, asyncResult.GetSocketAddressPtr(), asyncResult.SocketAddress.Size, asyncResult.OverlappedHandle, IntPtr.Zero);
        if (socketError != SocketError.Success)
          socketError = (SocketError) Marshal.GetLastWin32Error();
      }
      catch (ObjectDisposedException ex)
      {
        this.m_RightEndPoint = endPoint;
        throw;
      }
      finally
      {
        socketError = asyncResult.CheckAsyncCallOverlappedResult(socketError);
      }
      if (socketError == SocketError.Success)
        return;
      this.m_RightEndPoint = endPoint;
      asyncResult.ExtractCache(ref this.Caches.SendOverlappedCache);
      SocketException socketException = new SocketException(socketError);
      this.UpdateStatusAfterSocketError(socketException);
      if (Socket.s_LoggingEnabled)
        Logging.Exception(Logging.Sockets, (object) this, "BeginSendTo", (Exception) socketException);
      throw socketException;
    }

    public int EndSendTo(IAsyncResult asyncResult)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "EndSendTo", (object) asyncResult);
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (asyncResult == null)
        throw new ArgumentNullException("asyncResult");
      OverlappedAsyncResult overlappedAsyncResult = asyncResult as OverlappedAsyncResult;
      if (overlappedAsyncResult == null || overlappedAsyncResult.AsyncObject != this)
        throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
      if (overlappedAsyncResult.EndCalled)
      {
        throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[1]
        {
          (object) "EndSendTo"
        }));
      }
      else
      {
        int num = (int) overlappedAsyncResult.InternalWaitForCompletion();
        overlappedAsyncResult.EndCalled = true;
        overlappedAsyncResult.ExtractCache(ref this.Caches.SendOverlappedCache);
        if (Socket.s_PerfCountersEnabled && num > 0)
        {
          NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketBytesSent, (long) num);
          if (this.Transport == TransportType.Udp)
            NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketDatagramsSent);
        }
        if (overlappedAsyncResult.ErrorCode != 0)
        {
          SocketException socketException = new SocketException(overlappedAsyncResult.ErrorCode);
          this.UpdateStatusAfterSocketError(socketException);
          if (Socket.s_LoggingEnabled)
            Logging.Exception(Logging.Sockets, (object) this, "EndSendTo", (Exception) socketException);
          throw socketException;
        }
        else
        {
          if (Socket.s_LoggingEnabled)
            Logging.Exit(Logging.Sockets, (object) this, "EndSendTo", (object) num);
          return num;
        }
      }
    }

    [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
    public IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
    {
      SocketError errorCode;
      IAsyncResult asyncResult = this.BeginReceive(buffer, offset, size, socketFlags, out errorCode, callback, state);
      if (errorCode != SocketError.Success && errorCode != SocketError.IOPending)
        throw new SocketException(errorCode);
      else
        return asyncResult;
    }

    [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
    public IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "BeginReceive", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (buffer == null)
        throw new ArgumentNullException("buffer");
      if (offset < 0 || offset > buffer.Length)
        throw new ArgumentOutOfRangeException("offset");
      if (size < 0 || size > buffer.Length - offset)
        throw new ArgumentOutOfRangeException("size");
      OverlappedAsyncResult asyncResult = new OverlappedAsyncResult(this, state, callback);
      asyncResult.StartPostingAsyncOp(false);
      errorCode = this.DoBeginReceive(buffer, offset, size, socketFlags, asyncResult);
      if (errorCode != SocketError.Success && errorCode != SocketError.IOPending)
        asyncResult = (OverlappedAsyncResult) null;
      else
        asyncResult.FinishPostingAsyncOp(ref this.Caches.ReceiveClosureCache);
      if (Socket.s_LoggingEnabled)
        Logging.Exit(Logging.Sockets, (object) this, "BeginReceive", (object) asyncResult);
      return (IAsyncResult) asyncResult;
    }

    internal IAsyncResult UnsafeBeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "UnsafeBeginReceive", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      OverlappedAsyncResult asyncResult = new OverlappedAsyncResult(this, state, callback);
      int num = (int) this.DoBeginReceive(buffer, offset, size, socketFlags, asyncResult);
      if (Socket.s_LoggingEnabled)
        Logging.Exit(Logging.Sockets, (object) this, "UnsafeBeginReceive", (object) asyncResult);
      return (IAsyncResult) asyncResult;
    }

    private SocketError DoBeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, OverlappedAsyncResult asyncResult)
    {
      SocketError socketError = SocketError.SocketError;
      try
      {
        asyncResult.SetUnmanagedStructures(buffer, offset, size, (SocketAddress) null, false, ref this.Caches.ReceiveOverlappedCache);
        int bytesTransferred;
        socketError = UnsafeNclNativeMethods.OSSOCK.WSARecv(this.m_Handle, out asyncResult.m_SingleBuffer, 1, out bytesTransferred, out socketFlags, asyncResult.OverlappedHandle, IntPtr.Zero);
        if (socketError != SocketError.Success)
          socketError = (SocketError) Marshal.GetLastWin32Error();
      }
      finally
      {
        socketError = asyncResult.CheckAsyncCallOverlappedResult(socketError);
      }
      if (socketError != SocketError.Success)
      {
        asyncResult.ExtractCache(ref this.Caches.ReceiveOverlappedCache);
        this.UpdateStatusAfterSocketError(socketError);
        if (Socket.s_LoggingEnabled)
          Logging.Exception(Logging.Sockets, (object) this, "BeginReceive", (Exception) new SocketException(socketError));
        asyncResult.InvokeCallback((object) new SocketException(socketError));
      }
      return socketError;
    }

    [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
    public IAsyncResult BeginReceive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, AsyncCallback callback, object state)
    {
      SocketError errorCode;
      IAsyncResult asyncResult = this.BeginReceive(buffers, socketFlags, out errorCode, callback, state);
      if (errorCode != SocketError.Success && errorCode != SocketError.IOPending)
        throw new SocketException(errorCode);
      else
        return asyncResult;
    }

    [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
    public IAsyncResult BeginReceive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "BeginReceive", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (buffers == null)
        throw new ArgumentNullException("buffers");
      if (buffers.Count == 0)
      {
        throw new ArgumentException(SR.GetString("net_sockets_zerolist", new object[1]
        {
          (object) "buffers"
        }), "buffers");
      }
      else
      {
        OverlappedAsyncResult asyncResult = new OverlappedAsyncResult(this, state, callback);
        asyncResult.StartPostingAsyncOp(false);
        errorCode = this.DoBeginReceive(buffers, socketFlags, asyncResult);
        if (errorCode != SocketError.Success && errorCode != SocketError.IOPending)
          asyncResult = (OverlappedAsyncResult) null;
        else
          asyncResult.FinishPostingAsyncOp(ref this.Caches.ReceiveClosureCache);
        if (Socket.s_LoggingEnabled)
          Logging.Exit(Logging.Sockets, (object) this, "BeginReceive", (object) asyncResult);
        return (IAsyncResult) asyncResult;
      }
    }

    private SocketError DoBeginReceive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, OverlappedAsyncResult asyncResult)
    {
      SocketError socketError = SocketError.SocketError;
      try
      {
        asyncResult.SetUnmanagedStructures(buffers, ref this.Caches.ReceiveOverlappedCache);
        int bytesTransferred;
        socketError = UnsafeNclNativeMethods.OSSOCK.WSARecv(this.m_Handle, asyncResult.m_WSABuffers, asyncResult.m_WSABuffers.Length, out bytesTransferred, out socketFlags, asyncResult.OverlappedHandle, IntPtr.Zero);
        if (socketError != SocketError.Success)
          socketError = (SocketError) Marshal.GetLastWin32Error();
      }
      finally
      {
        socketError = asyncResult.CheckAsyncCallOverlappedResult(socketError);
      }
      if (socketError != SocketError.Success)
      {
        asyncResult.ExtractCache(ref this.Caches.ReceiveOverlappedCache);
        this.UpdateStatusAfterSocketError(socketError);
        if (Socket.s_LoggingEnabled)
          Logging.Exception(Logging.Sockets, (object) this, "BeginReceive", (Exception) new SocketException(socketError));
      }
      return socketError;
    }

    public int EndReceive(IAsyncResult asyncResult)
    {
      SocketError errorCode;
      int num = this.EndReceive(asyncResult, out errorCode);
      if (errorCode != SocketError.Success)
        throw new SocketException(errorCode);
      else
        return num;
    }

    public int EndReceive(IAsyncResult asyncResult, out SocketError errorCode)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "EndReceive", (object) asyncResult);
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (asyncResult == null)
        throw new ArgumentNullException("asyncResult");
      OverlappedAsyncResult overlappedAsyncResult = asyncResult as OverlappedAsyncResult;
      if (overlappedAsyncResult == null || overlappedAsyncResult.AsyncObject != this)
        throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
      if (overlappedAsyncResult.EndCalled)
      {
        throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[1]
        {
          (object) "EndReceive"
        }));
      }
      else
      {
        int num = (int) overlappedAsyncResult.InternalWaitForCompletion();
        overlappedAsyncResult.EndCalled = true;
        overlappedAsyncResult.ExtractCache(ref this.Caches.ReceiveOverlappedCache);
        if (Socket.s_PerfCountersEnabled && num > 0)
        {
          NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketBytesReceived, (long) num);
          if (this.Transport == TransportType.Udp)
            NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketDatagramsReceived);
        }
        errorCode = (SocketError) overlappedAsyncResult.ErrorCode;
        if (errorCode != SocketError.Success)
        {
          this.UpdateStatusAfterSocketError(errorCode);
          if (Socket.s_LoggingEnabled)
          {
            Logging.Exception(Logging.Sockets, (object) this, "EndReceive", (Exception) new SocketException(errorCode));
            Logging.Exit(Logging.Sockets, (object) this, "EndReceive", (object) 0);
          }
          return 0;
        }
        else
        {
          if (Socket.s_LoggingEnabled)
            Logging.Exit(Logging.Sockets, (object) this, "EndReceive", (object) num);
          return num;
        }
      }
    }

    public IAsyncResult BeginReceiveMessageFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP, AsyncCallback callback, object state)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "BeginReceiveMessageFrom", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (!ComNetOS.IsPostWin2K)
        throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
      if (buffer == null)
        throw new ArgumentNullException("buffer");
      if (remoteEP == null)
        throw new ArgumentNullException("remoteEP");
      if (remoteEP.AddressFamily != this.addressFamily)
      {
        throw new ArgumentException(SR.GetString("net_InvalidEndPointAddressFamily", (object) remoteEP.AddressFamily, (object) this.addressFamily), "remoteEP");
      }
      else
      {
        if (offset < 0 || offset > buffer.Length)
          throw new ArgumentOutOfRangeException("offset");
        if (size < 0 || size > buffer.Length - offset)
          throw new ArgumentOutOfRangeException("size");
        if (this.m_RightEndPoint == null)
          throw new InvalidOperationException(SR.GetString("net_sockets_mustbind"));
        ReceiveMessageOverlappedAsyncResult overlappedAsyncResult = new ReceiveMessageOverlappedAsyncResult(this, state, callback);
        overlappedAsyncResult.StartPostingAsyncOp(false);
        EndPoint endPoint = this.m_RightEndPoint;
        EndPoint remoteEP1 = remoteEP;
        SocketAddress socketAddress = this.SnapshotAndSerialize(ref remoteEP1);
        SocketError socketError = SocketError.SocketError;
        try
        {
          overlappedAsyncResult.SetUnmanagedStructures(buffer, offset, size, socketAddress, socketFlags, ref this.Caches.ReceiveOverlappedCache);
          overlappedAsyncResult.SocketAddressOriginal = remoteEP1.Serialize();
          this.SetReceivingPacketInformation();
          if (this.m_RightEndPoint == null)
            this.m_RightEndPoint = remoteEP1;
          int bytesTransferred;
          socketError = this.WSARecvMsg(this.m_Handle, Marshal.UnsafeAddrOfPinnedArrayElement((Array) overlappedAsyncResult.m_MessageBuffer, 0), out bytesTransferred, overlappedAsyncResult.OverlappedHandle, IntPtr.Zero);
          if (socketError != SocketError.Success)
          {
            socketError = (SocketError) Marshal.GetLastWin32Error();
            if (socketError == SocketError.MessageSize)
              socketError = SocketError.IOPending;
          }
        }
        catch (ObjectDisposedException ex)
        {
          this.m_RightEndPoint = endPoint;
          throw;
        }
        finally
        {
          socketError = overlappedAsyncResult.CheckAsyncCallOverlappedResult(socketError);
        }
        if (socketError != SocketError.Success)
        {
          this.m_RightEndPoint = endPoint;
          overlappedAsyncResult.ExtractCache(ref this.Caches.ReceiveOverlappedCache);
          SocketException socketException = new SocketException(socketError);
          this.UpdateStatusAfterSocketError(socketException);
          if (Socket.s_LoggingEnabled)
            Logging.Exception(Logging.Sockets, (object) this, "BeginReceiveMessageFrom", (Exception) socketException);
          throw socketException;
        }
        else
        {
          overlappedAsyncResult.FinishPostingAsyncOp(ref this.Caches.ReceiveClosureCache);
          if (overlappedAsyncResult.CompletedSynchronously)
          {
            if (!overlappedAsyncResult.SocketAddressOriginal.Equals((object) overlappedAsyncResult.SocketAddress))
            {
              try
              {
                remoteEP = remoteEP1.Create(overlappedAsyncResult.SocketAddress);
              }
              catch
              {
              }
            }
          }
          if (Socket.s_LoggingEnabled)
            Logging.Exit(Logging.Sockets, (object) this, "BeginReceiveMessageFrom", (object) overlappedAsyncResult);
          return (IAsyncResult) overlappedAsyncResult;
        }
      }
    }

    public int EndReceiveMessageFrom(IAsyncResult asyncResult, ref SocketFlags socketFlags, ref EndPoint endPoint, out IPPacketInformation ipPacketInformation)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "EndReceiveMessageFrom", (object) asyncResult);
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (endPoint == null)
        throw new ArgumentNullException("endPoint");
      if (endPoint.AddressFamily != this.addressFamily)
      {
        throw new ArgumentException(SR.GetString("net_InvalidEndPointAddressFamily", (object) endPoint.AddressFamily, (object) this.addressFamily), "endPoint");
      }
      else
      {
        if (asyncResult == null)
          throw new ArgumentNullException("asyncResult");
        ReceiveMessageOverlappedAsyncResult overlappedAsyncResult = asyncResult as ReceiveMessageOverlappedAsyncResult;
        if (overlappedAsyncResult == null || overlappedAsyncResult.AsyncObject != this)
          throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
        if (overlappedAsyncResult.EndCalled)
        {
          throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[1]
          {
            (object) "EndReceiveMessageFrom"
          }));
        }
        else
        {
          SocketAddress socketAddress = this.CallSerializeCheckDnsEndPoint(endPoint);
          int num = (int) overlappedAsyncResult.InternalWaitForCompletion();
          overlappedAsyncResult.EndCalled = true;
          overlappedAsyncResult.ExtractCache(ref this.Caches.ReceiveOverlappedCache);
          overlappedAsyncResult.SocketAddress.SetSize(overlappedAsyncResult.GetSocketAddressSizePtr());
          if (!socketAddress.Equals((object) overlappedAsyncResult.SocketAddress))
          {
            try
            {
              endPoint = endPoint.Create(overlappedAsyncResult.SocketAddress);
            }
            catch
            {
            }
          }
          if (Socket.s_PerfCountersEnabled && num > 0)
          {
            NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketBytesReceived, (long) num);
            if (this.Transport == TransportType.Udp)
              NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketDatagramsReceived);
          }
          if (overlappedAsyncResult.ErrorCode != 0 && overlappedAsyncResult.ErrorCode != 10040)
          {
            SocketException socketException = new SocketException(overlappedAsyncResult.ErrorCode);
            this.UpdateStatusAfterSocketError(socketException);
            if (Socket.s_LoggingEnabled)
              Logging.Exception(Logging.Sockets, (object) this, "EndReceiveMessageFrom", (Exception) socketException);
            throw socketException;
          }
          else
          {
            socketFlags = overlappedAsyncResult.m_flags;
            ipPacketInformation = overlappedAsyncResult.m_IPPacketInformation;
            if (Socket.s_LoggingEnabled)
              Logging.Exit(Logging.Sockets, (object) this, "EndReceiveMessageFrom", (object) num);
            return num;
          }
        }
      }
    }

    [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
    public IAsyncResult BeginReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP, AsyncCallback callback, object state)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "BeginReceiveFrom", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (buffer == null)
        throw new ArgumentNullException("buffer");
      if (remoteEP == null)
        throw new ArgumentNullException("remoteEP");
      if (remoteEP.AddressFamily != this.addressFamily)
      {
        throw new ArgumentException(SR.GetString("net_InvalidEndPointAddressFamily", (object) remoteEP.AddressFamily, (object) this.addressFamily), "remoteEP");
      }
      else
      {
        if (offset < 0 || offset > buffer.Length)
          throw new ArgumentOutOfRangeException("offset");
        if (size < 0 || size > buffer.Length - offset)
          throw new ArgumentOutOfRangeException("size");
        if (this.m_RightEndPoint == null)
          throw new InvalidOperationException(SR.GetString("net_sockets_mustbind"));
        EndPoint remoteEP1 = remoteEP;
        SocketAddress socketAddress = this.SnapshotAndSerialize(ref remoteEP1);
        OverlappedAsyncResult asyncResult = new OverlappedAsyncResult(this, state, callback);
        asyncResult.StartPostingAsyncOp(false);
        this.DoBeginReceiveFrom(buffer, offset, size, socketFlags, remoteEP1, socketAddress, asyncResult);
        asyncResult.FinishPostingAsyncOp(ref this.Caches.ReceiveClosureCache);
        if (asyncResult.CompletedSynchronously)
        {
          if (!asyncResult.SocketAddressOriginal.Equals((object) asyncResult.SocketAddress))
          {
            try
            {
              remoteEP = remoteEP1.Create(asyncResult.SocketAddress);
            }
            catch
            {
            }
          }
        }
        if (Socket.s_LoggingEnabled)
          Logging.Exit(Logging.Sockets, (object) this, "BeginReceiveFrom", (object) asyncResult);
        return (IAsyncResult) asyncResult;
      }
    }

    private void DoBeginReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint endPointSnapshot, SocketAddress socketAddress, OverlappedAsyncResult asyncResult)
    {
      EndPoint endPoint = this.m_RightEndPoint;
      SocketError socketError = SocketError.SocketError;
      try
      {
        asyncResult.SetUnmanagedStructures(buffer, offset, size, socketAddress, true, ref this.Caches.ReceiveOverlappedCache);
        asyncResult.SocketAddressOriginal = endPointSnapshot.Serialize();
        if (this.m_RightEndPoint == null)
          this.m_RightEndPoint = endPointSnapshot;
        int bytesTransferred;
        socketError = UnsafeNclNativeMethods.OSSOCK.WSARecvFrom(this.m_Handle, out asyncResult.m_SingleBuffer, 1, out bytesTransferred, out socketFlags, asyncResult.GetSocketAddressPtr(), asyncResult.GetSocketAddressSizePtr(), asyncResult.OverlappedHandle, IntPtr.Zero);
        if (socketError != SocketError.Success)
          socketError = (SocketError) Marshal.GetLastWin32Error();
      }
      catch (ObjectDisposedException ex)
      {
        this.m_RightEndPoint = endPoint;
        throw;
      }
      finally
      {
        socketError = asyncResult.CheckAsyncCallOverlappedResult(socketError);
      }
      if (socketError == SocketError.Success)
        return;
      this.m_RightEndPoint = endPoint;
      asyncResult.ExtractCache(ref this.Caches.ReceiveOverlappedCache);
      SocketException socketException = new SocketException(socketError);
      this.UpdateStatusAfterSocketError(socketException);
      if (Socket.s_LoggingEnabled)
        Logging.Exception(Logging.Sockets, (object) this, "BeginReceiveFrom", (Exception) socketException);
      throw socketException;
    }

    public int EndReceiveFrom(IAsyncResult asyncResult, ref EndPoint endPoint)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "EndReceiveFrom", (object) asyncResult);
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (endPoint == null)
        throw new ArgumentNullException("endPoint");
      if (endPoint.AddressFamily != this.addressFamily)
      {
        throw new ArgumentException(SR.GetString("net_InvalidEndPointAddressFamily", (object) endPoint.AddressFamily, (object) this.addressFamily), "endPoint");
      }
      else
      {
        if (asyncResult == null)
          throw new ArgumentNullException("asyncResult");
        OverlappedAsyncResult overlappedAsyncResult = asyncResult as OverlappedAsyncResult;
        if (overlappedAsyncResult == null || overlappedAsyncResult.AsyncObject != this)
          throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
        if (overlappedAsyncResult.EndCalled)
        {
          throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[1]
          {
            (object) "EndReceiveFrom"
          }));
        }
        else
        {
          SocketAddress socketAddress = this.CallSerializeCheckDnsEndPoint(endPoint);
          int num = (int) overlappedAsyncResult.InternalWaitForCompletion();
          overlappedAsyncResult.EndCalled = true;
          overlappedAsyncResult.ExtractCache(ref this.Caches.ReceiveOverlappedCache);
          overlappedAsyncResult.SocketAddress.SetSize(overlappedAsyncResult.GetSocketAddressSizePtr());
          if (!socketAddress.Equals((object) overlappedAsyncResult.SocketAddress))
          {
            try
            {
              endPoint = endPoint.Create(overlappedAsyncResult.SocketAddress);
            }
            catch
            {
            }
          }
          if (Socket.s_PerfCountersEnabled && num > 0)
          {
            NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketBytesReceived, (long) num);
            if (this.Transport == TransportType.Udp)
              NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketDatagramsReceived);
          }
          if (overlappedAsyncResult.ErrorCode != 0)
          {
            SocketException socketException = new SocketException(overlappedAsyncResult.ErrorCode);
            this.UpdateStatusAfterSocketError(socketException);
            if (Socket.s_LoggingEnabled)
              Logging.Exception(Logging.Sockets, (object) this, "EndReceiveFrom", (Exception) socketException);
            throw socketException;
          }
          else
          {
            if (Socket.s_LoggingEnabled)
              Logging.Exit(Logging.Sockets, (object) this, "EndReceiveFrom", (object) num);
            return num;
          }
        }
      }
    }

    [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
    public IAsyncResult BeginAccept(AsyncCallback callback, object state)
    {
      if (this.CanUseAcceptEx)
        return this.BeginAccept(0, callback, state);
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "BeginAccept", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      AcceptAsyncResult acceptAsyncResult = new AcceptAsyncResult((object) this, state, callback);
      acceptAsyncResult.StartPostingAsyncOp(false);
      this.DoBeginAccept((LazyAsyncResult) acceptAsyncResult);
      acceptAsyncResult.FinishPostingAsyncOp(ref this.Caches.AcceptClosureCache);
      if (Socket.s_LoggingEnabled)
        Logging.Exit(Logging.Sockets, (object) this, "BeginAccept", (object) acceptAsyncResult);
      return (IAsyncResult) acceptAsyncResult;
    }

    private void DoBeginAccept(LazyAsyncResult asyncResult)
    {
      if (this.m_RightEndPoint == null)
        throw new InvalidOperationException(SR.GetString("net_sockets_mustbind"));
      if (!this.isListening)
        throw new InvalidOperationException(SR.GetString("net_sockets_mustlisten"));
      bool flag = false;
      SocketError socketError = SocketError.Success;
      System.Collections.Queue acceptQueue = this.GetAcceptQueue();
      lock (this)
      {
        if (acceptQueue.Count == 0)
        {
          SocketAddress local_3 = this.m_RightEndPoint.Serialize();
          this.InternalSetBlocking(false);
          SafeCloseSocket local_4 = (SafeCloseSocket) null;
          try
          {
            local_4 = SafeCloseSocket.Accept(this.m_Handle, local_3.m_Buffer, ref local_3.m_Size);
            socketError = local_4.IsInvalid ? (SocketError) Marshal.GetLastWin32Error() : SocketError.Success;
          }
          catch (ObjectDisposedException exception_0)
          {
            socketError = SocketError.NotSocket;
          }
          if (socketError != SocketError.WouldBlock)
          {
            if (socketError == SocketError.Success)
              asyncResult.Result = (object) this.CreateAcceptSocket(local_4, this.m_RightEndPoint.Create(local_3), false);
            else
              asyncResult.ErrorCode = (int) socketError;
            this.InternalSetBlocking(true);
            flag = true;
          }
          else
          {
            acceptQueue.Enqueue((object) asyncResult);
            if (!this.SetAsyncEventSelect(AsyncEventBits.FdAccept))
            {
              acceptQueue.Dequeue();
              throw new ObjectDisposedException(this.GetType().FullName);
            }
          }
        }
        else
          acceptQueue.Enqueue((object) asyncResult);
      }
      if (!flag)
        return;
      if (socketError == SocketError.Success)
      {
        asyncResult.InvokeCallback();
      }
      else
      {
        SocketException socketException = new SocketException(socketError);
        this.UpdateStatusAfterSocketError(socketException);
        if (Socket.s_LoggingEnabled)
          Logging.Exception(Logging.Sockets, (object) this, "BeginAccept", (Exception) socketException);
        throw socketException;
      }
    }

    private void CompleteAcceptResults(object nullState)
    {
      System.Collections.Queue acceptQueue = this.GetAcceptQueue();
      bool flag = true;
      while (flag)
      {
        LazyAsyncResult lazyAsyncResult = (LazyAsyncResult) null;
        lock (this)
        {
          if (acceptQueue.Count == 0)
            break;
          lazyAsyncResult = (LazyAsyncResult) acceptQueue.Dequeue();
          if (acceptQueue.Count == 0)
            flag = false;
        }
        try
        {
          lazyAsyncResult.InvokeCallback((object) new SocketException(SocketError.OperationAborted));
        }
        catch
        {
          if (flag)
            ThreadPool.UnsafeQueueUserWorkItem(new WaitCallback(this.CompleteAcceptResults), (object) null);
          throw;
        }
      }
    }

    private void AcceptCallback(object nullState)
    {
      bool flag = true;
      System.Collections.Queue acceptQueue = this.GetAcceptQueue();
      while (flag)
      {
        LazyAsyncResult lazyAsyncResult = (LazyAsyncResult) null;
        SocketError socketError = SocketError.OperationAborted;
        SocketAddress socketAddress = (SocketAddress) null;
        SafeCloseSocket fd = (SafeCloseSocket) null;
        Exception exception = (Exception) null;
        object result = (object) null;
        lock (this)
        {
          if (acceptQueue.Count == 0)
            break;
          lazyAsyncResult = (LazyAsyncResult) acceptQueue.Peek();
          if (!this.CleanedUp)
          {
            socketAddress = this.m_RightEndPoint.Serialize();
            try
            {
              fd = SafeCloseSocket.Accept(this.m_Handle, socketAddress.m_Buffer, ref socketAddress.m_Size);
              socketError = fd.IsInvalid ? (SocketError) Marshal.GetLastWin32Error() : SocketError.Success;
            }
            catch (ObjectDisposedException exception_1)
            {
              socketError = SocketError.OperationAborted;
            }
            catch (Exception exception_2)
            {
              if (NclUtilities.IsFatal(exception_2))
                throw;
              else
                exception = exception_2;
            }
          }
          if (socketError == SocketError.WouldBlock && exception == null)
          {
            if (this.SetAsyncEventSelect(AsyncEventBits.FdAccept))
              break;
            exception = (Exception) new ObjectDisposedException(this.GetType().FullName);
          }
          if (exception != null)
            result = (object) exception;
          else if (socketError == SocketError.Success)
            result = (object) this.CreateAcceptSocket(fd, this.m_RightEndPoint.Create(socketAddress), true);
          else
            lazyAsyncResult.ErrorCode = (int) socketError;
          acceptQueue.Dequeue();
          if (acceptQueue.Count == 0)
          {
            if (!this.CleanedUp)
              this.UnsetAsyncEventSelect();
            flag = false;
          }
        }
        try
        {
          lazyAsyncResult.InvokeCallback(result);
        }
        catch
        {
          if (flag)
            ThreadPool.UnsafeQueueUserWorkItem(new WaitCallback(this.AcceptCallback), nullState);
          throw;
        }
      }
    }

    [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
    public IAsyncResult BeginAccept(int receiveSize, AsyncCallback callback, object state)
    {
      return this.BeginAccept((Socket) null, receiveSize, callback, state);
    }

    [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
    public IAsyncResult BeginAccept(Socket acceptSocket, int receiveSize, AsyncCallback callback, object state)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "BeginAccept", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (receiveSize < 0)
        throw new ArgumentOutOfRangeException("size");
      AcceptOverlappedAsyncResult asyncResult = new AcceptOverlappedAsyncResult(this, state, callback);
      asyncResult.StartPostingAsyncOp(false);
      this.DoBeginAccept(acceptSocket, receiveSize, asyncResult);
      asyncResult.FinishPostingAsyncOp(ref this.Caches.AcceptClosureCache);
      if (Socket.s_LoggingEnabled)
        Logging.Exit(Logging.Sockets, (object) this, "BeginAccept", (object) asyncResult);
      return (IAsyncResult) asyncResult;
    }

    private void DoBeginAccept(Socket acceptSocket, int receiveSize, AcceptOverlappedAsyncResult asyncResult)
    {
      if (!ComNetOS.IsWinNt)
        throw new PlatformNotSupportedException(SR.GetString("WinNTRequired"));
      if (this.m_RightEndPoint == null)
        throw new InvalidOperationException(SR.GetString("net_sockets_mustbind"));
      if (!this.isListening)
        throw new InvalidOperationException(SR.GetString("net_sockets_mustlisten"));
      if (acceptSocket == null)
        acceptSocket = new Socket(this.addressFamily, this.socketType, this.protocolType);
      else if (acceptSocket.m_RightEndPoint != null)
        throw new InvalidOperationException(SR.GetString("net_sockets_namedmustnotbebound", new object[1]
        {
          (object) "acceptSocket"
        }));
      asyncResult.AcceptSocket = acceptSocket;
      int num = this.m_RightEndPoint.Serialize().Size + 16;
      byte[] buffer = new byte[receiveSize + num * 2];
      asyncResult.SetUnmanagedStructures(buffer, num);
      SocketError errorCode = SocketError.Success;
      int bytesReceived;
      if (!this.AcceptEx(this.m_Handle, acceptSocket.m_Handle, Marshal.UnsafeAddrOfPinnedArrayElement((Array) asyncResult.Buffer, 0), receiveSize, num, num, out bytesReceived, asyncResult.OverlappedHandle))
        errorCode = (SocketError) Marshal.GetLastWin32Error();
      SocketError socketError = asyncResult.CheckAsyncCallOverlappedResult(errorCode);
      if (socketError == SocketError.Success)
        return;
      SocketException socketException = new SocketException(socketError);
      this.UpdateStatusAfterSocketError(socketException);
      if (Socket.s_LoggingEnabled)
        Logging.Exception(Logging.Sockets, (object) this, "BeginAccept", (Exception) socketException);
      throw socketException;
    }

    public Socket EndAccept(IAsyncResult asyncResult)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "EndAccept", (object) asyncResult);
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (ComNetOS.IsWinNt && asyncResult != null && asyncResult is AcceptOverlappedAsyncResult)
      {
        byte[] buffer;
        int bytesTransferred;
        return this.EndAccept(out buffer, out bytesTransferred, asyncResult);
      }
      if (asyncResult == null)
        throw new ArgumentNullException("asyncResult");
      AcceptAsyncResult acceptAsyncResult = asyncResult as AcceptAsyncResult;
      if (acceptAsyncResult == null || acceptAsyncResult.AsyncObject != this)
        throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
      if (acceptAsyncResult.EndCalled)
      {
        throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[1]
        {
          (object) "EndAccept"
        }));
      }
      else
      {
        object retObject = acceptAsyncResult.InternalWaitForCompletion();
        acceptAsyncResult.EndCalled = true;
        Exception exception = retObject as Exception;
        if (exception != null)
          throw exception;
        if (acceptAsyncResult.ErrorCode != 0)
        {
          SocketException socketException = new SocketException(acceptAsyncResult.ErrorCode);
          this.UpdateStatusAfterSocketError(socketException);
          if (Socket.s_LoggingEnabled)
            Logging.Exception(Logging.Sockets, (object) this, "EndAccept", (Exception) socketException);
          throw socketException;
        }
        else
        {
          Socket socket = (Socket) retObject;
          if (Socket.s_LoggingEnabled)
          {
            Logging.PrintInfo(Logging.Sockets, (object) socket, SR.GetString("net_log_socket_accepted", (object) socket.RemoteEndPoint, (object) socket.LocalEndPoint));
            Logging.Exit(Logging.Sockets, (object) this, "EndAccept", retObject);
          }
          return socket;
        }
      }
    }

    public Socket EndAccept(out byte[] buffer, IAsyncResult asyncResult)
    {
      byte[] buffer1;
      int bytesTransferred;
      Socket socket = this.EndAccept(out buffer1, out bytesTransferred, asyncResult);
      buffer = new byte[bytesTransferred];
      Array.Copy((Array) buffer1, (Array) buffer, bytesTransferred);
      return socket;
    }

    public Socket EndAccept(out byte[] buffer, out int bytesTransferred, IAsyncResult asyncResult)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "EndAccept", (object) asyncResult);
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (!ComNetOS.IsWinNt)
        throw new PlatformNotSupportedException(SR.GetString("WinNTRequired"));
      if (asyncResult == null)
        throw new ArgumentNullException("asyncResult");
      AcceptOverlappedAsyncResult overlappedAsyncResult = asyncResult as AcceptOverlappedAsyncResult;
      if (overlappedAsyncResult == null || overlappedAsyncResult.AsyncObject != this)
        throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
      if (overlappedAsyncResult.EndCalled)
      {
        throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[1]
        {
          (object) "EndAccept"
        }));
      }
      else
      {
        Socket socket = (Socket) overlappedAsyncResult.InternalWaitForCompletion();
        bytesTransferred = overlappedAsyncResult.BytesTransferred;
        buffer = overlappedAsyncResult.Buffer;
        overlappedAsyncResult.EndCalled = true;
        if (Socket.s_PerfCountersEnabled && bytesTransferred > 0)
          NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketBytesReceived, (long) bytesTransferred);
        if (overlappedAsyncResult.ErrorCode != 0)
        {
          SocketException socketException = new SocketException(overlappedAsyncResult.ErrorCode);
          this.UpdateStatusAfterSocketError(socketException);
          if (Socket.s_LoggingEnabled)
            Logging.Exception(Logging.Sockets, (object) this, "EndAccept", (Exception) socketException);
          throw socketException;
        }
        else
        {
          if (Socket.s_LoggingEnabled)
          {
            Logging.PrintInfo(Logging.Sockets, (object) socket, SR.GetString("net_log_socket_accepted", (object) socket.RemoteEndPoint, (object) socket.LocalEndPoint));
            Logging.Exit(Logging.Sockets, (object) this, "EndAccept", (object) socket);
          }
          return socket;
        }
      }
    }

    public void Shutdown(SocketShutdown how)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "Shutdown", (object) how);
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      SocketError socketError = UnsafeNclNativeMethods.OSSOCK.shutdown(this.m_Handle, (int) how) != SocketError.SocketError ? SocketError.Success : (SocketError) Marshal.GetLastWin32Error();
      switch (socketError)
      {
        case SocketError.Success:
        case SocketError.NotSocket:
          this.SetToDisconnected();
          this.InternalSetBlocking(this.willBlockInternal);
          if (!Socket.s_LoggingEnabled)
            break;
          Logging.Exit(Logging.Sockets, (object) this, "Shutdown", "");
          break;
        default:
          SocketException socketException = new SocketException(socketError);
          this.UpdateStatusAfterSocketError(socketException);
          if (Socket.s_LoggingEnabled)
            Logging.Exception(Logging.Sockets, (object) this, "Shutdown", (Exception) socketException);
          throw socketException;
      }
    }

    private void EnsureDynamicWinsockMethods()
    {
      if (this.m_DynamicWinsockMethods != null)
        return;
      this.m_DynamicWinsockMethods = DynamicWinsockMethods.GetMethods(this.addressFamily, this.socketType, this.protocolType);
    }

    private bool AcceptEx(SafeCloseSocket listenSocketHandle, SafeCloseSocket acceptSocketHandle, IntPtr buffer, int len, int localAddressLength, int remoteAddressLength, out int bytesReceived, SafeHandle overlapped)
    {
      this.EnsureDynamicWinsockMethods();
      return this.m_DynamicWinsockMethods.GetDelegate<AcceptExDelegate>(listenSocketHandle)(listenSocketHandle, acceptSocketHandle, buffer, len, localAddressLength, remoteAddressLength, out bytesReceived, overlapped);
    }

    internal void GetAcceptExSockaddrs(IntPtr buffer, int receiveDataLength, int localAddressLength, int remoteAddressLength, out IntPtr localSocketAddress, out int localSocketAddressLength, out IntPtr remoteSocketAddress, out int remoteSocketAddressLength)
    {
      this.EnsureDynamicWinsockMethods();
      this.m_DynamicWinsockMethods.GetDelegate<GetAcceptExSockaddrsDelegate>(this.m_Handle)(buffer, receiveDataLength, localAddressLength, remoteAddressLength, out localSocketAddress, out localSocketAddressLength, out remoteSocketAddress, out remoteSocketAddressLength);
    }

    private bool DisconnectEx(SafeCloseSocket socketHandle, SafeHandle overlapped, int flags, int reserved)
    {
      this.EnsureDynamicWinsockMethods();
      return this.m_DynamicWinsockMethods.GetDelegate<DisconnectExDelegate>(socketHandle)(socketHandle, overlapped, flags, reserved);
    }

    private bool DisconnectEx_Blocking(IntPtr socketHandle, IntPtr overlapped, int flags, int reserved)
    {
      this.EnsureDynamicWinsockMethods();
      return this.m_DynamicWinsockMethods.GetDelegate<DisconnectExDelegate_Blocking>(this.m_Handle)(socketHandle, overlapped, flags, reserved);
    }

    private bool ConnectEx(SafeCloseSocket socketHandle, IntPtr socketAddress, int socketAddressSize, IntPtr buffer, int dataLength, out int bytesSent, SafeHandle overlapped)
    {
      this.EnsureDynamicWinsockMethods();
      return this.m_DynamicWinsockMethods.GetDelegate<ConnectExDelegate>(socketHandle)(socketHandle, socketAddress, socketAddressSize, buffer, dataLength, out bytesSent, overlapped);
    }

    private SocketError WSARecvMsg(SafeCloseSocket socketHandle, IntPtr msg, out int bytesTransferred, SafeHandle overlapped, IntPtr completionRoutine)
    {
      this.EnsureDynamicWinsockMethods();
      return this.m_DynamicWinsockMethods.GetDelegate<WSARecvMsgDelegate>(socketHandle)(socketHandle, msg, out bytesTransferred, overlapped, completionRoutine);
    }

    private SocketError WSARecvMsg_Blocking(IntPtr socketHandle, IntPtr msg, out int bytesTransferred, IntPtr overlapped, IntPtr completionRoutine)
    {
      this.EnsureDynamicWinsockMethods();
      return this.m_DynamicWinsockMethods.GetDelegate<WSARecvMsgDelegate_Blocking>(this.m_Handle)(socketHandle, msg, out bytesTransferred, overlapped, completionRoutine);
    }

    private bool TransmitPackets(SafeCloseSocket socketHandle, IntPtr packetArray, int elementCount, int sendSize, SafeNativeOverlapped overlapped, TransmitFileOptions flags)
    {
      this.EnsureDynamicWinsockMethods();
      return this.m_DynamicWinsockMethods.GetDelegate<TransmitPacketsDelegate>(socketHandle)(socketHandle, packetArray, elementCount, sendSize, (SafeHandle) overlapped, flags);
    }

    private System.Collections.Queue GetAcceptQueue()
    {
      if (this.m_AcceptQueueOrConnectResult == null)
        Interlocked.CompareExchange(ref this.m_AcceptQueueOrConnectResult, (object) new System.Collections.Queue(16), (object) null);
      return (System.Collections.Queue) this.m_AcceptQueueOrConnectResult;
    }

    private void CheckSetOptionPermissions(SocketOptionLevel optionLevel, SocketOptionName optionName)
    {
      if (optionLevel == SocketOptionLevel.Tcp && (optionName == SocketOptionName.Debug || optionName == SocketOptionName.AcceptConnection || optionName == SocketOptionName.AcceptConnection) || optionLevel == SocketOptionLevel.Udp && (optionName == SocketOptionName.Debug || optionName == SocketOptionName.ChecksumCoverage) || optionLevel == SocketOptionLevel.Socket && (optionName == SocketOptionName.KeepAlive || optionName == SocketOptionName.Linger || (optionName == SocketOptionName.DontLinger || optionName == SocketOptionName.SendBuffer) || (optionName == SocketOptionName.ReceiveBuffer || optionName == SocketOptionName.SendTimeout || (optionName == SocketOptionName.ExclusiveAddressUse || optionName == SocketOptionName.ReceiveTimeout))) || optionLevel == SocketOptionLevel.IPv6 && optionName == SocketOptionName.IPProtectionLevel)
        return;
      ExceptionHelper.UnmanagedPermission.Demand();
    }

    private SocketAddress SnapshotAndSerialize(ref EndPoint remoteEP)
    {
      IPEndPoint ipEndPoint1 = remoteEP as IPEndPoint;
      if (ipEndPoint1 != null)
      {
        IPEndPoint ipEndPoint2 = ipEndPoint1.Snapshot();
        remoteEP = (EndPoint) ipEndPoint2;
      }
      return this.CallSerializeCheckDnsEndPoint(remoteEP);
    }

    private SocketAddress CallSerializeCheckDnsEndPoint(EndPoint remoteEP)
    {
      if (!(remoteEP is DnsEndPoint))
        return remoteEP.Serialize();
      throw new ArgumentException(SR.GetString("net_sockets_invalid_dnsendpoint", new object[1]
      {
        (object) "remoteEP"
      }), "remoteEP");
    }

    private SocketAddress CheckCacheRemote(ref EndPoint remoteEP, bool isOverwrite)
    {
      IPEndPoint ipEndPoint = remoteEP as IPEndPoint;
      if (ipEndPoint != null)
      {
        ipEndPoint = ipEndPoint.Snapshot();
        remoteEP = (EndPoint) ipEndPoint;
      }
      SocketAddress socketAddress1 = this.CallSerializeCheckDnsEndPoint(remoteEP);
      SocketAddress socketAddress2 = this.m_PermittedRemoteAddress;
      if (socketAddress2 != null && socketAddress2.Equals((object) socketAddress1))
        return socketAddress2;
      if (ipEndPoint != null)
        new SocketPermission(NetworkAccess.Connect, this.Transport, ipEndPoint.Address.ToString(), ipEndPoint.Port).Demand();
      else
        ExceptionHelper.UnmanagedPermission.Demand();
      if (this.m_PermittedRemoteAddress == null || isOverwrite)
        this.m_PermittedRemoteAddress = socketAddress1;
      return socketAddress1;
    }

    internal static void InitializeSockets()
    {
      if (Socket.s_Initialized)
        return;
      lock (Socket.InternalSyncObject)
      {
        if (Socket.s_Initialized)
          return;
        WSAData local_0 = new WSAData();
        if (UnsafeNclNativeMethods.OSSOCK.WSAStartup((short) 514, out local_0) != SocketError.Success)
          throw new SocketException();
        if (!ComNetOS.IsWinNt)
          Socket.UseOverlappedIO = true;
        bool local_2 = true;
        bool local_3 = true;
        SafeCloseSocket.InnerSafeCloseSocket local_4 = UnsafeNclNativeMethods.OSSOCK.WSASocket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP, IntPtr.Zero, 0U, (SocketConstructorFlags) 0);
        if (local_4.IsInvalid && Marshal.GetLastWin32Error() == 10047)
          local_2 = false;
        local_4.Close();
        SafeCloseSocket.InnerSafeCloseSocket local_5 = UnsafeNclNativeMethods.OSSOCK.WSASocket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.IP, IntPtr.Zero, 0U, (SocketConstructorFlags) 0);
        if (local_5.IsInvalid && Marshal.GetLastWin32Error() == 10047)
          local_3 = false;
        local_5.Close();
        bool local_3_1 = local_3 && ComNetOS.IsPostWin2K;
        if (local_3_1)
        {
          Socket.s_OSSupportsIPv6 = true;
          local_3_1 = SettingsSectionInternal.Section.Ipv6Enabled;
        }
        Socket.s_SupportsIPv4 = local_2;
        Socket.s_SupportsIPv6 = local_3_1;
        Socket.s_PerfCountersEnabled = NetworkingPerfCounters.Instance.Enabled;
        Socket.s_Initialized = true;
      }
    }

    internal void InternalConnect(EndPoint remoteEP)
    {
      EndPoint remoteEP1 = remoteEP;
      SocketAddress socketAddress = this.SnapshotAndSerialize(ref remoteEP1);
      this.DoConnect(remoteEP1, socketAddress);
    }

    private void DoConnect(EndPoint endPointSnapshot, SocketAddress socketAddress)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "Connect", (object) endPointSnapshot);
      if (UnsafeNclNativeMethods.OSSOCK.WSAConnect(this.m_Handle.DangerousGetHandle(), socketAddress.m_Buffer, socketAddress.m_Size, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero) != SocketError.Success)
      {
        SocketException socketException = new SocketException(endPointSnapshot);
        this.UpdateStatusAfterSocketError(socketException);
        if (Socket.s_LoggingEnabled)
          Logging.Exception(Logging.Sockets, (object) this, "Connect", (Exception) socketException);
        throw socketException;
      }
      else
      {
        if (this.m_RightEndPoint == null)
          this.m_RightEndPoint = endPointSnapshot;
        this.SetToConnected();
        if (!Socket.s_LoggingEnabled)
          return;
        Logging.PrintInfo(Logging.Sockets, (object) this, SR.GetString("net_log_socket_connected", (object) this.LocalEndPoint, (object) this.RemoteEndPoint));
        Logging.Exit(Logging.Sockets, (object) this, "Connect", "");
      }
    }

    protected virtual unsafe void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      try
      {
        if (Socket.s_LoggingEnabled)
          Logging.Enter(Logging.Sockets, (object) this, "Dispose", (string) null);
      }
      catch (Exception ex)
      {
        if (NclUtilities.IsFatal(ex))
          throw;
      }
      int num;
      while ((num = Interlocked.CompareExchange(ref this.m_IntCleanedUp, 1, 0)) == 2)
        Thread.SpinWait(1);
      if (num == 1)
      {
        try
        {
          if (!Socket.s_LoggingEnabled)
            return;
          Logging.Exit(Logging.Sockets, (object) this, "Dispose", (string) null);
        }
        catch (Exception ex)
        {
          if (!NclUtilities.IsFatal(ex))
            return;
          throw;
        }
      }
      else
      {
        this.SetToDisconnected();
        AsyncEventBits asyncEventBits = AsyncEventBits.FdNone;
        if (this.m_BlockEventBits != AsyncEventBits.FdNone)
        {
          this.UnsetAsyncEventSelect();
          if (this.m_BlockEventBits == AsyncEventBits.FdConnect)
          {
            LazyAsyncResult lazyAsyncResult = this.m_AcceptQueueOrConnectResult as LazyAsyncResult;
            if (lazyAsyncResult != null)
            {
              if (!lazyAsyncResult.InternalPeekCompleted)
                asyncEventBits = AsyncEventBits.FdConnect;
            }
          }
          else if (this.m_BlockEventBits == AsyncEventBits.FdAccept)
          {
            System.Collections.Queue queue = this.m_AcceptQueueOrConnectResult as System.Collections.Queue;
            if (queue != null)
            {
              if (queue.Count != 0)
                asyncEventBits = AsyncEventBits.FdAccept;
            }
          }
        }
        try
        {
          int optionValue = this.m_CloseTimeout;
          if (optionValue == 0)
          {
            this.m_Handle.Dispose();
          }
          else
          {
            SocketError socketError;
            if (!this.willBlock || !this.willBlockInternal)
            {
              int argp = 0;
              socketError = UnsafeNclNativeMethods.OSSOCK.ioctlsocket(this.m_Handle, -2147195266, out argp);
            }
            if (optionValue < 0)
            {
              this.m_Handle.CloseAsIs();
            }
            else
            {
              socketError = UnsafeNclNativeMethods.OSSOCK.shutdown(this.m_Handle, 1);
              if (UnsafeNclNativeMethods.OSSOCK.setsockopt(this.m_Handle, SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, ref optionValue, 4) != SocketError.Success)
                this.m_Handle.Dispose();
              else if (UnsafeNclNativeMethods.OSSOCK.recv(this.m_Handle.DangerousGetHandle(), (byte*) null, 0, SocketFlags.None) != 0)
              {
                this.m_Handle.Dispose();
              }
              else
              {
                int argp = 0;
                if (UnsafeNclNativeMethods.OSSOCK.ioctlsocket(this.m_Handle, 1074030207, out argp) != SocketError.Success || argp != 0)
                  this.m_Handle.Dispose();
                else
                  this.m_Handle.CloseAsIs();
              }
            }
          }
        }
        catch (ObjectDisposedException ex)
        {
        }
        if (this.m_Caches != null)
        {
          OverlappedCache.InterlockedFree(ref this.m_Caches.SendOverlappedCache);
          OverlappedCache.InterlockedFree(ref this.m_Caches.ReceiveOverlappedCache);
        }
        if (asyncEventBits == AsyncEventBits.FdConnect)
          ThreadPool.UnsafeQueueUserWorkItem(new WaitCallback(((LazyAsyncResult) this.m_AcceptQueueOrConnectResult).InvokeCallback), (object) new SocketException(SocketError.OperationAborted));
        else if (asyncEventBits == AsyncEventBits.FdAccept)
          ThreadPool.UnsafeQueueUserWorkItem(new WaitCallback(this.CompleteAcceptResults), (object) null);
        if (this.m_AsyncEvent == null)
          return;
        this.m_AsyncEvent.Close();
      }
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    internal void InternalShutdown(SocketShutdown how)
    {
      if (this.CleanedUp)
        return;
      if (this.m_Handle.IsInvalid)
        return;
      try
      {
        int num = (int) UnsafeNclNativeMethods.OSSOCK.shutdown(this.m_Handle, (int) how);
      }
      catch (ObjectDisposedException ex)
      {
      }
    }

    private void DownLevelSendFile(string fileName)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "SendFile", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (!this.Connected)
        throw new NotSupportedException(SR.GetString("net_notconnected"));
      this.ValidateBlockingMode();
      FileStream fileStream = (FileStream) null;
      if (fileName != null)
      {
        if (fileName.Length > 0)
          fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
      }
      try
      {
        SocketError socketError = SocketError.Success;
        byte[] buffer = new byte[64000];
        while (true)
        {
          int size = fileStream.Read(buffer, 0, buffer.Length);
          if (size != 0)
            this.Send(buffer, 0, size, SocketFlags.None);
          else
            break;
        }
        if (!Socket.s_LoggingEnabled)
          return;
        Logging.Exit(Logging.Sockets, (object) this, "SendFile", (object) socketError);
      }
      finally
      {
        Socket.DownLevelSendFileCleanup(fileStream);
      }
    }

    internal void SetReceivingPacketInformation()
    {
      if (this.m_ReceivingPacketInformation)
        return;
      if (this.addressFamily == AddressFamily.InterNetwork)
        this.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
      else if (this.addressFamily == AddressFamily.InterNetworkV6)
        this.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.PacketInformation, true);
      this.m_ReceivingPacketInformation = true;
    }

    internal void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionValue, bool silent)
    {
      if (silent && (this.CleanedUp || this.m_Handle.IsInvalid))
        return;
      SocketError socketError;
      try
      {
        socketError = UnsafeNclNativeMethods.OSSOCK.setsockopt(this.m_Handle, optionLevel, optionName, ref optionValue, 4);
      }
      catch
      {
        if (silent && this.m_Handle.IsInvalid)
          return;
        throw;
      }
      if (optionName == SocketOptionName.PacketInformation && optionValue == 0 && socketError == SocketError.Success)
        this.m_ReceivingPacketInformation = false;
      if (silent || socketError != SocketError.SocketError)
        return;
      SocketException socketException = new SocketException();
      this.UpdateStatusAfterSocketError(socketException);
      if (Socket.s_LoggingEnabled)
        Logging.Exception(Logging.Sockets, (object) this, "SetSocketOption", (Exception) socketException);
      throw socketException;
    }

    private void setMulticastOption(SocketOptionName optionName, MulticastOption MR)
    {
      IPMulticastRequest mreq = new IPMulticastRequest();
      mreq.MulticastAddress = (int) MR.Group.m_Address;
      if (MR.LocalAddress != null)
      {
        mreq.InterfaceAddress = (int) MR.LocalAddress.m_Address;
      }
      else
      {
        int num = IPAddress.HostToNetworkOrder(MR.InterfaceIndex);
        mreq.InterfaceAddress = num;
      }
      if (UnsafeNclNativeMethods.OSSOCK.setsockopt(this.m_Handle, SocketOptionLevel.IP, optionName, ref mreq, IPMulticastRequest.Size) != SocketError.SocketError)
        return;
      SocketException socketException = new SocketException();
      this.UpdateStatusAfterSocketError(socketException);
      if (Socket.s_LoggingEnabled)
        Logging.Exception(Logging.Sockets, (object) this, "setMulticastOption", (Exception) socketException);
      throw socketException;
    }

    private void setIPv6MulticastOption(SocketOptionName optionName, IPv6MulticastOption MR)
    {
      if (UnsafeNclNativeMethods.OSSOCK.setsockopt(this.m_Handle, SocketOptionLevel.IPv6, optionName, ref new IPv6MulticastRequest()
      {
        MulticastAddress = MR.Group.GetAddressBytes(),
        InterfaceIndex = (int) MR.InterfaceIndex
      }, IPv6MulticastRequest.Size) != SocketError.SocketError)
        return;
      SocketException socketException = new SocketException();
      this.UpdateStatusAfterSocketError(socketException);
      if (Socket.s_LoggingEnabled)
        Logging.Exception(Logging.Sockets, (object) this, "setIPv6MulticastOption", (Exception) socketException);
      throw socketException;
    }

    private void setLingerOption(LingerOption lref)
    {
      if (UnsafeNclNativeMethods.OSSOCK.setsockopt(this.m_Handle, SocketOptionLevel.Socket, SocketOptionName.Linger, ref new Linger()
      {
        OnOff = lref.Enabled ? (ushort) 1 : (ushort) 0,
        Time = (ushort) lref.LingerTime
      }, 4) != SocketError.SocketError)
        return;
      SocketException socketException = new SocketException();
      this.UpdateStatusAfterSocketError(socketException);
      if (Socket.s_LoggingEnabled)
        Logging.Exception(Logging.Sockets, (object) this, "setLingerOption", (Exception) socketException);
      throw socketException;
    }

    private LingerOption getLingerOpt()
    {
      Linger optionValue = new Linger();
      int optionLength = 4;
      if (UnsafeNclNativeMethods.OSSOCK.getsockopt(this.m_Handle, SocketOptionLevel.Socket, SocketOptionName.Linger, out optionValue, out optionLength) != SocketError.SocketError)
        return new LingerOption((int) optionValue.OnOff != 0, (int) optionValue.Time);
      SocketException socketException = new SocketException();
      this.UpdateStatusAfterSocketError(socketException);
      if (Socket.s_LoggingEnabled)
        Logging.Exception(Logging.Sockets, (object) this, "getLingerOpt", (Exception) socketException);
      throw socketException;
    }

    private MulticastOption getMulticastOpt(SocketOptionName optionName)
    {
      IPMulticastRequest optionValue = new IPMulticastRequest();
      int optionLength = IPMulticastRequest.Size;
      if (UnsafeNclNativeMethods.OSSOCK.getsockopt(this.m_Handle, SocketOptionLevel.IP, optionName, out optionValue, out optionLength) != SocketError.SocketError)
        return new MulticastOption(new IPAddress(optionValue.MulticastAddress), new IPAddress(optionValue.InterfaceAddress));
      SocketException socketException = new SocketException();
      this.UpdateStatusAfterSocketError(socketException);
      if (Socket.s_LoggingEnabled)
        Logging.Exception(Logging.Sockets, (object) this, "getMulticastOpt", (Exception) socketException);
      throw socketException;
    }

    private IPv6MulticastOption getIPv6MulticastOpt(SocketOptionName optionName)
    {
      IPv6MulticastRequest optionValue = new IPv6MulticastRequest();
      int optionLength = IPv6MulticastRequest.Size;
      if (UnsafeNclNativeMethods.OSSOCK.getsockopt(this.m_Handle, SocketOptionLevel.IP, optionName, out optionValue, out optionLength) != SocketError.SocketError)
        return new IPv6MulticastOption(new IPAddress(optionValue.MulticastAddress), (long) optionValue.InterfaceIndex);
      SocketException socketException = new SocketException();
      this.UpdateStatusAfterSocketError(socketException);
      if (Socket.s_LoggingEnabled)
        Logging.Exception(Logging.Sockets, (object) this, "getIPv6MulticastOpt", (Exception) socketException);
      throw socketException;
    }

    private SocketError InternalSetBlocking(bool desired, out bool current)
    {
      if (this.CleanedUp)
      {
        current = this.willBlock;
        return SocketError.Success;
      }
      else
      {
        int argp = desired ? 0 : -1;
        SocketError socketError;
        try
        {
          socketError = UnsafeNclNativeMethods.OSSOCK.ioctlsocket(this.m_Handle, -2147195266, out argp);
          if (socketError == SocketError.SocketError)
            socketError = (SocketError) Marshal.GetLastWin32Error();
        }
        catch (ObjectDisposedException ex)
        {
          socketError = SocketError.NotSocket;
        }
        if (socketError == SocketError.Success)
          this.willBlockInternal = argp == 0;
        current = this.willBlockInternal;
        return socketError;
      }
    }

    internal void InternalSetBlocking(bool desired)
    {
      bool current;
      int num = (int) this.InternalSetBlocking(desired, out current);
    }

    private static IntPtr[] SocketListToFileDescriptorSet(IList socketList)
    {
      if (socketList == null || socketList.Count == 0)
        return (IntPtr[]) null;
      IntPtr[] numArray = new IntPtr[socketList.Count + 1];
      numArray[0] = (IntPtr) socketList.Count;
      for (int index = 0; index < socketList.Count; ++index)
      {
        if (!(socketList[index] is Socket))
          throw new ArgumentException(SR.GetString("net_sockets_select", (object) socketList[index].GetType().FullName, (object) typeof (Socket).FullName), "socketList");
        else
          numArray[index + 1] = ((Socket) socketList[index]).m_Handle.DangerousGetHandle();
      }
      return numArray;
    }

    private static void SelectFileDescriptor(IList socketList, IntPtr[] fileDescriptorSet)
    {
      if (socketList == null || socketList.Count == 0)
        return;
      if ((int) fileDescriptorSet[0] == 0)
      {
        socketList.Clear();
      }
      else
      {
        lock (socketList)
        {
          for (int local_0 = 0; local_0 < socketList.Count; ++local_0)
          {
            Socket local_1 = socketList[local_0] as Socket;
            int local_2 = 0;
            while (local_2 < (int) fileDescriptorSet[0] && !(fileDescriptorSet[local_2 + 1] == local_1.m_Handle.DangerousGetHandle()))
              ++local_2;
            if (local_2 == (int) fileDescriptorSet[0])
              socketList.RemoveAt(local_0--);
          }
        }
      }
    }

    private static void MicrosecondsToTimeValue(long microSeconds, ref TimeValue socketTime)
    {
      socketTime.Seconds = (int) (microSeconds / 1000000L);
      socketTime.Microseconds = (int) (microSeconds % 1000000L);
    }

    private IAsyncResult BeginConnectEx(EndPoint remoteEP, bool flowContext, AsyncCallback callback, object state)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "BeginConnectEx", "");
      EndPoint remoteEP1 = remoteEP;
      SocketAddress socketAddress = flowContext ? this.CheckCacheRemote(ref remoteEP1, true) : this.SnapshotAndSerialize(ref remoteEP1);
      if (this.m_RightEndPoint == null)
      {
        if (remoteEP1.AddressFamily == AddressFamily.InterNetwork)
          this.InternalBind((EndPoint) new IPEndPoint(IPAddress.Any, 0));
        else
          this.InternalBind((EndPoint) new IPEndPoint(IPAddress.IPv6Any, 0));
      }
      ConnectOverlappedAsyncResult overlappedAsyncResult = new ConnectOverlappedAsyncResult(this, remoteEP1, state, callback);
      if (flowContext)
        overlappedAsyncResult.StartPostingAsyncOp(false);
      overlappedAsyncResult.SetUnmanagedStructures((object) socketAddress.m_Buffer);
      EndPoint endPoint = this.m_RightEndPoint;
      if (this.m_RightEndPoint == null)
        this.m_RightEndPoint = remoteEP1;
      SocketError errorCode = SocketError.Success;
      try
      {
        int bytesSent;
        if (!this.ConnectEx(this.m_Handle, Marshal.UnsafeAddrOfPinnedArrayElement((Array) socketAddress.m_Buffer, 0), socketAddress.m_Size, IntPtr.Zero, 0, out bytesSent, overlappedAsyncResult.OverlappedHandle))
          errorCode = (SocketError) Marshal.GetLastWin32Error();
      }
      catch
      {
        overlappedAsyncResult.InternalCleanup();
        this.m_RightEndPoint = endPoint;
        throw;
      }
      if (errorCode == SocketError.Success)
        this.SetToConnected();
      SocketError socketError = overlappedAsyncResult.CheckAsyncCallOverlappedResult(errorCode);
      if (socketError != SocketError.Success)
      {
        this.m_RightEndPoint = endPoint;
        SocketException socketException = new SocketException(socketError);
        this.UpdateStatusAfterSocketError(socketException);
        if (Socket.s_LoggingEnabled)
          Logging.Exception(Logging.Sockets, (object) this, "BeginConnectEx", (Exception) socketException);
        throw socketException;
      }
      else
      {
        overlappedAsyncResult.FinishPostingAsyncOp(ref this.Caches.ConnectClosureCache);
        if (Socket.s_LoggingEnabled)
          Logging.Exit(Logging.Sockets, (object) this, "BeginConnectEx", (object) overlappedAsyncResult);
        return (IAsyncResult) overlappedAsyncResult;
      }
    }

    internal void MultipleSend(BufferOffsetSize[] buffers, SocketFlags socketFlags)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "MultipleSend", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      WSABuffer[] buffersArray = new WSABuffer[buffers.Length];
      GCHandle[] gcHandleArray = (GCHandle[]) null;
      SocketError socketError;
      try
      {
        gcHandleArray = new GCHandle[buffers.Length];
        for (int index = 0; index < buffers.Length; ++index)
        {
          gcHandleArray[index] = GCHandle.Alloc((object) buffers[index].Buffer, GCHandleType.Pinned);
          buffersArray[index].Length = buffers[index].Size;
          buffersArray[index].Pointer = Marshal.UnsafeAddrOfPinnedArrayElement((Array) buffers[index].Buffer, buffers[index].Offset);
        }
        int bytesTransferred;
        socketError = UnsafeNclNativeMethods.OSSOCK.WSASend_Blocking(this.m_Handle.DangerousGetHandle(), buffersArray, buffersArray.Length, out bytesTransferred, socketFlags, (SafeHandle) SafeNativeOverlapped.Zero, IntPtr.Zero);
      }
      finally
      {
        if (gcHandleArray != null)
        {
          for (int index = 0; index < gcHandleArray.Length; ++index)
          {
            if (gcHandleArray[index].IsAllocated)
              gcHandleArray[index].Free();
          }
        }
      }
      if (socketError != SocketError.Success)
      {
        SocketException socketException = new SocketException();
        this.UpdateStatusAfterSocketError(socketException);
        if (Socket.s_LoggingEnabled)
          Logging.Exception(Logging.Sockets, (object) this, "MultipleSend", (Exception) socketException);
        throw socketException;
      }
      else
      {
        if (!Socket.s_LoggingEnabled)
          return;
        Logging.Exit(Logging.Sockets, (object) this, "MultipleSend", "");
      }
    }

    private static void DnsCallback(IAsyncResult result)
    {
      if (result.CompletedSynchronously)
        return;
      bool flag = false;
      Socket.MultipleAddressConnectAsyncResult context = (Socket.MultipleAddressConnectAsyncResult) result.AsyncState;
      try
      {
        flag = Socket.DoDnsCallback(result, context);
      }
      catch (Exception ex)
      {
        context.InvokeCallback((object) ex);
      }
      if (!flag)
        return;
      context.InvokeCallback();
    }

    private static bool DoDnsCallback(IAsyncResult result, Socket.MultipleAddressConnectAsyncResult context)
    {
      IPAddress[] hostAddresses = Dns.EndGetHostAddresses(result);
      context.addresses = hostAddresses;
      return Socket.DoMultipleAddressConnectCallback(Socket.PostOneBeginConnect(context), context);
    }

    private static object PostOneBeginConnect(Socket.MultipleAddressConnectAsyncResult context)
    {
      IPAddress address = context.addresses[context.index];
      if (address.AddressFamily != context.socket.AddressFamily)
      {
        if (context.lastException == null)
          return (object) new ArgumentException(SR.GetString("net_invalidAddressList"), "context");
        else
          return (object) context.lastException;
      }
      else
      {
        try
        {
          EndPoint remoteEP = (EndPoint) new IPEndPoint(address, context.port);
          context.socket.CheckCacheRemote(ref remoteEP, true);
          IAsyncResult asyncResult = context.socket.UnsafeBeginConnect(remoteEP, new AsyncCallback(Socket.MultipleAddressConnectCallback), (object) context);
          if (asyncResult.CompletedSynchronously)
            return (object) asyncResult;
        }
        catch (Exception ex)
        {
          if (!(ex is OutOfMemoryException) && !(ex is StackOverflowException) && !(ex is ThreadAbortException))
            return (object) ex;
          throw;
        }
        return (object) null;
      }
    }

    private static void MultipleAddressConnectCallback(IAsyncResult result)
    {
      if (result.CompletedSynchronously)
        return;
      bool flag = false;
      Socket.MultipleAddressConnectAsyncResult context = (Socket.MultipleAddressConnectAsyncResult) result.AsyncState;
      try
      {
        flag = Socket.DoMultipleAddressConnectCallback((object) result, context);
      }
      catch (Exception ex)
      {
        context.InvokeCallback((object) ex);
      }
      if (!flag)
        return;
      context.InvokeCallback();
    }

    private static bool DoMultipleAddressConnectCallback(object result, Socket.MultipleAddressConnectAsyncResult context)
    {
      for (; result != null; result = Socket.PostOneBeginConnect(context))
      {
        Exception exception = result as Exception;
        if (exception == null)
        {
          try
          {
            context.socket.EndConnect((IAsyncResult) result);
          }
          catch (Exception ex)
          {
            exception = ex;
          }
        }
        if (exception == null)
          return true;
        if (++context.index >= context.addresses.Length)
          throw exception;
        context.lastException = exception;
      }
      return false;
    }

    private static void DownLevelSendFileCallback(IAsyncResult result)
    {
      if (result.CompletedSynchronously)
        return;
      Socket.DownLevelSendFileAsyncResult context = (Socket.DownLevelSendFileAsyncResult) result.AsyncState;
      Socket.DoDownLevelSendFileCallback(result, context);
    }

    private static void DoDownLevelSendFileCallback(IAsyncResult result, Socket.DownLevelSendFileAsyncResult context)
    {
      try
      {
        do
        {
          while (context.writing)
          {
            context.socket.EndSend(result);
            context.writing = false;
            result = context.fileStream.BeginRead(context.buffer, 0, context.buffer.Length, new AsyncCallback(Socket.DownLevelSendFileCallback), (object) context);
            if (!result.CompletedSynchronously)
              return;
          }
          int size = context.fileStream.EndRead(result);
          if (size > 0)
          {
            context.writing = true;
            result = context.socket.BeginSend(context.buffer, 0, size, SocketFlags.None, new AsyncCallback(Socket.DownLevelSendFileCallback), (object) context);
          }
          else
            goto label_3;
        }
        while (result.CompletedSynchronously);
        return;
label_3:
        Socket.DownLevelSendFileCleanup(context.fileStream);
        context.InvokeCallback();
      }
      catch (Exception ex)
      {
        if (NclUtilities.IsFatal(ex))
        {
          throw;
        }
        else
        {
          Socket.DownLevelSendFileCleanup(context.fileStream);
          context.InvokeCallback((object) ex);
        }
      }
    }

    private static void DownLevelSendFileCleanup(FileStream fileStream)
    {
      if (fileStream == null)
        return;
      fileStream.Close();
      fileStream = (FileStream) null;
    }

    private IAsyncResult BeginDownLevelSendFile(string fileName, bool flowContext, AsyncCallback callback, object state)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "BeginSendFile", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (!this.Connected)
        throw new NotSupportedException(SR.GetString("net_notconnected"));
      FileStream fileStream = (FileStream) null;
      if (fileName != null && fileName.Length > 0)
        fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
      Socket.DownLevelSendFileAsyncResult context;
      IAsyncResult result;
      try
      {
        context = new Socket.DownLevelSendFileAsyncResult(fileStream, this, state, callback);
        if (flowContext)
          context.StartPostingAsyncOp(false);
        result = fileStream.BeginRead(context.buffer, 0, context.buffer.Length, new AsyncCallback(Socket.DownLevelSendFileCallback), (object) context);
      }
      catch (Exception ex)
      {
        if (!NclUtilities.IsFatal(ex))
          Socket.DownLevelSendFileCleanup(fileStream);
        throw;
      }
      if (result.CompletedSynchronously)
        Socket.DoDownLevelSendFileCallback(result, context);
      context.FinishPostingAsyncOp(ref this.Caches.SendClosureCache);
      if (Socket.s_LoggingEnabled)
        Logging.Exit(Logging.Sockets, (object) this, "BeginSendFile", (object) 0);
      return (IAsyncResult) context;
    }

    internal IAsyncResult BeginMultipleSend(BufferOffsetSize[] buffers, SocketFlags socketFlags, AsyncCallback callback, object state)
    {
      OverlappedAsyncResult asyncResult = new OverlappedAsyncResult(this, state, callback);
      asyncResult.StartPostingAsyncOp(false);
      this.DoBeginMultipleSend(buffers, socketFlags, asyncResult);
      asyncResult.FinishPostingAsyncOp(ref this.Caches.SendClosureCache);
      return (IAsyncResult) asyncResult;
    }

    internal IAsyncResult UnsafeBeginMultipleSend(BufferOffsetSize[] buffers, SocketFlags socketFlags, AsyncCallback callback, object state)
    {
      OverlappedAsyncResult asyncResult = new OverlappedAsyncResult(this, state, callback);
      this.DoBeginMultipleSend(buffers, socketFlags, asyncResult);
      return (IAsyncResult) asyncResult;
    }

    private void DoBeginMultipleSend(BufferOffsetSize[] buffers, SocketFlags socketFlags, OverlappedAsyncResult asyncResult)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "BeginMultipleSend", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      SocketError socketError = SocketError.SocketError;
      try
      {
        asyncResult.SetUnmanagedStructures(buffers, ref this.Caches.SendOverlappedCache);
        int bytesTransferred;
        socketError = UnsafeNclNativeMethods.OSSOCK.WSASend(this.m_Handle, asyncResult.m_WSABuffers, asyncResult.m_WSABuffers.Length, out bytesTransferred, socketFlags, asyncResult.OverlappedHandle, IntPtr.Zero);
        if (socketError != SocketError.Success)
          socketError = (SocketError) Marshal.GetLastWin32Error();
      }
      finally
      {
        socketError = asyncResult.CheckAsyncCallOverlappedResult(socketError);
      }
      if (socketError != SocketError.Success)
      {
        asyncResult.ExtractCache(ref this.Caches.SendOverlappedCache);
        SocketException socketException = new SocketException(socketError);
        this.UpdateStatusAfterSocketError(socketException);
        if (Socket.s_LoggingEnabled)
          Logging.Exception(Logging.Sockets, (object) this, "BeginMultipleSend", (Exception) socketException);
        throw socketException;
      }
      else
      {
        if (!Socket.s_LoggingEnabled)
          return;
        Logging.Exit(Logging.Sockets, (object) this, "BeginMultipleSend", (object) asyncResult);
      }
    }

    private void EndDownLevelSendFile(IAsyncResult asyncResult)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "EndSendFile", (object) asyncResult);
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (asyncResult == null)
        throw new ArgumentNullException("asyncResult");
      LazyAsyncResult lazyAsyncResult = (LazyAsyncResult) (asyncResult as Socket.DownLevelSendFileAsyncResult);
      if (lazyAsyncResult == null || lazyAsyncResult.AsyncObject != this)
        throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
      if (lazyAsyncResult.EndCalled)
      {
        throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[1]
        {
          (object) "EndSendFile"
        }));
      }
      else
      {
        lazyAsyncResult.InternalWaitForCompletion();
        lazyAsyncResult.EndCalled = true;
        Exception exception = lazyAsyncResult.Result as Exception;
        if (exception != null)
          throw exception;
        if (!Socket.s_LoggingEnabled)
          return;
        Logging.Exit(Logging.Sockets, (object) this, "EndSendFile", "");
      }
    }

    internal int EndMultipleSend(IAsyncResult asyncResult)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "EndMultipleSend", (object) asyncResult);
      OverlappedAsyncResult overlappedAsyncResult = asyncResult as OverlappedAsyncResult;
      int num = (int) overlappedAsyncResult.InternalWaitForCompletion();
      overlappedAsyncResult.EndCalled = true;
      overlappedAsyncResult.ExtractCache(ref this.Caches.SendOverlappedCache);
      if (Socket.s_PerfCountersEnabled && num > 0)
      {
        NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketBytesSent, (long) num);
        if (this.Transport == TransportType.Udp)
          NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketDatagramsSent);
      }
      if (overlappedAsyncResult.ErrorCode != 0)
      {
        SocketException socketException = new SocketException(overlappedAsyncResult.ErrorCode);
        if (Socket.s_LoggingEnabled)
          Logging.Exception(Logging.Sockets, (object) this, "EndMultipleSend", (Exception) socketException);
        throw socketException;
      }
      else
      {
        if (Socket.s_LoggingEnabled)
          Logging.Exit(Logging.Sockets, (object) this, "EndMultipleSend", (object) num);
        return num;
      }
    }

    private Socket CreateAcceptSocket(SafeCloseSocket fd, EndPoint remoteEP, bool needCancelSelect)
    {
      return this.UpdateAcceptSocket(new Socket(fd), remoteEP, needCancelSelect);
    }

    internal Socket UpdateAcceptSocket(Socket socket, EndPoint remoteEP, bool needCancelSelect)
    {
      socket.addressFamily = this.addressFamily;
      socket.socketType = this.socketType;
      socket.protocolType = this.protocolType;
      socket.m_RightEndPoint = this.m_RightEndPoint;
      socket.m_RemoteEndPoint = remoteEP;
      socket.SetToConnected();
      socket.willBlock = this.willBlock;
      if (needCancelSelect)
        socket.UnsetAsyncEventSelect();
      else
        socket.InternalSetBlocking(this.willBlock);
      return socket;
    }

    internal void SetToConnected()
    {
      if (this.m_IsConnected)
        return;
      this.m_IsConnected = true;
      this.m_IsDisconnected = false;
      if (!Socket.s_PerfCountersEnabled)
        return;
      NetworkingPerfCounters.Instance.Increment(NetworkingPerfCounterName.SocketConnectionsEstablished);
    }

    internal void SetToDisconnected()
    {
      if (!this.m_IsConnected)
        return;
      this.m_IsConnected = false;
      this.m_IsDisconnected = true;
      if (this.CleanedUp)
        return;
      this.UnsetAsyncEventSelect();
    }

    internal void UpdateStatusAfterSocketError(SocketException socketException)
    {
      this.UpdateStatusAfterSocketError((SocketError) socketException.NativeErrorCode);
    }

    internal void UpdateStatusAfterSocketError(SocketError errorCode)
    {
      if (!this.m_IsConnected || !this.m_Handle.IsInvalid && (errorCode == SocketError.WouldBlock || errorCode == SocketError.IOPending || errorCode == SocketError.NoBufferSpaceAvailable))
        return;
      this.SetToDisconnected();
    }

    private void UnsetAsyncEventSelect()
    {
      RegisteredWaitHandle registeredWaitHandle = this.m_RegisteredWait;
      if (registeredWaitHandle != null)
      {
        this.m_RegisteredWait = (RegisteredWaitHandle) null;
        registeredWaitHandle.Unregister((WaitHandle) null);
      }
      SocketError errorCode = SocketError.NotSocket;
      try
      {
        errorCode = UnsafeNclNativeMethods.OSSOCK.WSAEventSelect(this.m_Handle, IntPtr.Zero, AsyncEventBits.FdNone);
      }
      catch (Exception ex)
      {
        if (NclUtilities.IsFatal(ex))
          throw;
      }
      if (this.m_AsyncEvent != null)
      {
        try
        {
          this.m_AsyncEvent.Reset();
        }
        catch (ObjectDisposedException ex)
        {
        }
      }
      if (errorCode == SocketError.SocketError)
        this.UpdateStatusAfterSocketError(errorCode);
      this.InternalSetBlocking(this.willBlock);
    }

    private bool SetAsyncEventSelect(AsyncEventBits blockEventBits)
    {
      if (this.m_RegisteredWait != null)
        return false;
      if (this.m_AsyncEvent == null)
      {
        Interlocked.CompareExchange<ManualResetEvent>(ref this.m_AsyncEvent, new ManualResetEvent(false), (ManualResetEvent) null);
        if (Socket.s_RegisteredWaitCallback == null)
          Socket.s_RegisteredWaitCallback = new WaitOrTimerCallback(Socket.RegisteredWaitCallback);
      }
      if (Interlocked.CompareExchange(ref this.m_IntCleanedUp, 2, 0) != 0)
        return false;
      this.m_BlockEventBits = blockEventBits;
      this.m_RegisteredWait = ThreadPool.UnsafeRegisterWaitForSingleObject((WaitHandle) this.m_AsyncEvent, Socket.s_RegisteredWaitCallback, (object) this, -1, true);
      Interlocked.Exchange(ref this.m_IntCleanedUp, 0);
      SocketError errorCode = SocketError.NotSocket;
      try
      {
        errorCode = UnsafeNclNativeMethods.OSSOCK.WSAEventSelect(this.m_Handle, (SafeHandle) this.m_AsyncEvent.SafeWaitHandle, blockEventBits);
      }
      catch (Exception ex)
      {
        if (NclUtilities.IsFatal(ex))
          throw;
      }
      if (errorCode == SocketError.SocketError)
        this.UpdateStatusAfterSocketError(errorCode);
      this.willBlockInternal = false;
      return errorCode == SocketError.Success;
    }

    private static void RegisteredWaitCallback(object state, bool timedOut)
    {
      Socket socket = (Socket) state;
      if (Interlocked.Exchange<RegisteredWaitHandle>(ref socket.m_RegisteredWait, (RegisteredWaitHandle) null) == null)
        return;
      switch (socket.m_BlockEventBits)
      {
        case AsyncEventBits.FdAccept:
          socket.AcceptCallback((object) null);
          break;
        case AsyncEventBits.FdConnect:
          socket.ConnectCallback();
          break;
      }
    }

    private void ValidateBlockingMode()
    {
      if (this.willBlock && !this.willBlockInternal)
        throw new InvalidOperationException(SR.GetString("net_invasync"));
    }

    [SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
    internal void BindToCompletionPort()
    {
      if (this.m_BoundToThreadPool || Socket.UseOverlappedIO)
        return;
      lock (this)
      {
        if (this.m_BoundToThreadPool)
          return;
        try
        {
          ThreadPool.BindHandle((SafeHandle) this.m_Handle);
          this.m_BoundToThreadPool = true;
        }
        catch (Exception exception_0)
        {
          if (NclUtilities.IsFatal(exception_0))
          {
            throw;
          }
          else
          {
            this.Close(0);
            throw;
          }
        }
      }
    }

    public bool AcceptAsync(SocketAsyncEventArgs e)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "AcceptAsync", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (e.m_BufferList != null)
        throw new ArgumentException(SR.GetString("net_multibuffernotsupported"), "BufferList");
      if (this.m_RightEndPoint == null)
        throw new InvalidOperationException(SR.GetString("net_sockets_mustbind"));
      if (!this.isListening)
        throw new InvalidOperationException(SR.GetString("net_sockets_mustlisten"));
      if (e.AcceptSocket == null)
        e.AcceptSocket = new Socket(this.addressFamily, this.socketType, this.protocolType);
      else if (e.AcceptSocket.m_RightEndPoint != null && !e.AcceptSocket.m_IsDisconnected)
        throw new InvalidOperationException(SR.GetString("net_sockets_namedmustnotbebound", new object[1]
        {
          (object) "AcceptSocket"
        }));
      e.StartOperationCommon(this);
      e.StartOperationAccept();
      this.BindToCompletionPort();
      SocketError socketError = SocketError.Success;
      int bytesReceived;
      try
      {
        if (!this.AcceptEx(this.m_Handle, e.AcceptSocket.m_Handle, e.m_PtrSingleBuffer != IntPtr.Zero ? e.m_PtrSingleBuffer : e.m_PtrAcceptBuffer, e.m_PtrSingleBuffer != IntPtr.Zero ? e.Count - e.m_AcceptAddressBufferCount : 0, e.m_AcceptAddressBufferCount / 2, e.m_AcceptAddressBufferCount / 2, out bytesReceived, (SafeHandle) e.m_PtrNativeOverlapped))
          socketError = (SocketError) Marshal.GetLastWin32Error();
      }
      catch (Exception ex)
      {
        e.Complete();
        throw ex;
      }
      bool flag;
      if (socketError != SocketError.Success && socketError != SocketError.IOPending)
      {
        e.FinishOperationSyncFailure(socketError, bytesReceived, SocketFlags.None);
        flag = false;
      }
      else
        flag = true;
      if (Socket.s_LoggingEnabled)
        Logging.Exit(Logging.Sockets, (object) this, "AcceptAsync", (object) (bool) (flag ? 1 : 0));
      return flag;
    }

    public bool ConnectAsync(SocketAsyncEventArgs e)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "ConnectAsync", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (e.m_BufferList != null)
        throw new ArgumentException(SR.GetString("net_multibuffernotsupported"), "BufferList");
      if (e.RemoteEndPoint == null)
        throw new ArgumentNullException("remoteEP");
      if (this.isListening)
        throw new InvalidOperationException(SR.GetString("net_sockets_mustnotlisten"));
      EndPoint remoteEndPoint = e.RemoteEndPoint;
      DnsEndPoint endPoint1 = remoteEndPoint as DnsEndPoint;
      bool flag;
      if (endPoint1 != null)
      {
        if (Socket.s_LoggingEnabled)
          Logging.PrintInfo(Logging.Sockets, "Socket#" + ValidationHelper.HashString((object) this) + "::ConnectAsync Connecting to a DnsEndPoint");
        if (endPoint1.AddressFamily != AddressFamily.Unspecified && endPoint1.AddressFamily != this.addressFamily)
          throw new NotSupportedException(SR.GetString("net_invalidversion"));
        MultipleConnectAsync args = (MultipleConnectAsync) new SingleSocketMultipleConnectAsync(this, true);
        e.StartOperationCommon(this);
        e.StartOperationWrapperConnect(args);
        flag = args.StartConnectAsync(e, endPoint1);
      }
      else
      {
        if (this.addressFamily != e.RemoteEndPoint.AddressFamily)
          throw new NotSupportedException(SR.GetString("net_invalidversion"));
        e.m_SocketAddress = this.CheckCacheRemote(ref remoteEndPoint, false);
        if (this.m_RightEndPoint == null)
        {
          if (remoteEndPoint.AddressFamily == AddressFamily.InterNetwork)
            this.InternalBind((EndPoint) new IPEndPoint(IPAddress.Any, 0));
          else
            this.InternalBind((EndPoint) new IPEndPoint(IPAddress.IPv6Any, 0));
        }
        EndPoint endPoint2 = this.m_RightEndPoint;
        if (this.m_RightEndPoint == null)
          this.m_RightEndPoint = remoteEndPoint;
        e.StartOperationCommon(this);
        e.StartOperationConnect();
        this.BindToCompletionPort();
        SocketError socketError = SocketError.Success;
        int bytesSent;
        try
        {
          if (!this.ConnectEx(this.m_Handle, e.m_PtrSocketAddressBuffer, e.m_SocketAddress.m_Size, e.m_PtrSingleBuffer, e.Count, out bytesSent, (SafeHandle) e.m_PtrNativeOverlapped))
            socketError = (SocketError) Marshal.GetLastWin32Error();
        }
        catch (Exception ex)
        {
          this.m_RightEndPoint = endPoint2;
          e.Complete();
          throw ex;
        }
        if (socketError != SocketError.Success && socketError != SocketError.IOPending)
        {
          e.FinishOperationSyncFailure(socketError, bytesSent, SocketFlags.None);
          flag = false;
        }
        else
          flag = true;
      }
      if (Socket.s_LoggingEnabled)
        Logging.Exit(Logging.Sockets, (object) this, "ConnectAsync", (object) (bool) (flag ? 1 : 0));
      return flag;
    }

    public static bool ConnectAsync(SocketType socketType, ProtocolType protocolType, SocketAsyncEventArgs e)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (string) null, "ConnectAsync", "");
      if (e.m_BufferList != null)
        throw new ArgumentException(SR.GetString("net_multibuffernotsupported"), "BufferList");
      if (e.RemoteEndPoint == null)
        throw new ArgumentNullException("remoteEP");
      EndPoint remoteEndPoint = e.RemoteEndPoint;
      DnsEndPoint endPoint = remoteEndPoint as DnsEndPoint;
      bool flag;
      if (endPoint != null)
      {
        Socket socket = (Socket) null;
        MultipleConnectAsync args;
        if (endPoint.AddressFamily == AddressFamily.Unspecified)
        {
          args = (MultipleConnectAsync) new MultipleSocketMultipleConnectAsync(socketType, protocolType);
        }
        else
        {
          socket = new Socket(endPoint.AddressFamily, socketType, protocolType);
          args = (MultipleConnectAsync) new SingleSocketMultipleConnectAsync(socket, false);
        }
        e.StartOperationCommon(socket);
        e.StartOperationWrapperConnect(args);
        flag = args.StartConnectAsync(e, endPoint);
      }
      else
        flag = new Socket(remoteEndPoint.AddressFamily, socketType, protocolType).ConnectAsync(e);
      if (Socket.s_LoggingEnabled)
        Logging.Exit(Logging.Sockets, (string) null, "ConnectAsync", (object) (bool) (flag ? 1 : 0));
      return flag;
    }

    public static void CancelConnectAsync(SocketAsyncEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException("e");
      e.CancelConnectAsync();
    }

    public bool DisconnectAsync(SocketAsyncEventArgs e)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "DisconnectAsync", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      e.StartOperationCommon(this);
      e.StartOperationDisconnect();
      this.BindToCompletionPort();
      SocketError socketError = SocketError.Success;
      try
      {
        if (!this.DisconnectEx(this.m_Handle, (SafeHandle) e.m_PtrNativeOverlapped, e.DisconnectReuseSocket ? 2 : 0, 0))
          socketError = (SocketError) Marshal.GetLastWin32Error();
      }
      catch (Exception ex)
      {
        e.Complete();
        throw ex;
      }
      bool flag;
      if (socketError != SocketError.Success && socketError != SocketError.IOPending)
      {
        e.FinishOperationSyncFailure(socketError, 0, SocketFlags.None);
        flag = false;
      }
      else
        flag = true;
      if (Socket.s_LoggingEnabled)
        Logging.Exit(Logging.Sockets, (object) this, "DisconnectAsync", (object) (bool) (flag ? 1 : 0));
      return flag;
    }

    public bool ReceiveAsync(SocketAsyncEventArgs e)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "ReceiveAsync", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      e.StartOperationCommon(this);
      e.StartOperationReceive();
      this.BindToCompletionPort();
      SocketFlags socketFlags = e.m_SocketFlags;
      int bytesTransferred;
      SocketError socketError;
      try
      {
        socketError = e.m_Buffer == null ? UnsafeNclNativeMethods.OSSOCK.WSARecv(this.m_Handle, e.m_WSABufferArray, e.m_WSABufferArray.Length, out bytesTransferred, out socketFlags, (SafeHandle) e.m_PtrNativeOverlapped, IntPtr.Zero) : UnsafeNclNativeMethods.OSSOCK.WSARecv(this.m_Handle, out e.m_WSABuffer, 1, out bytesTransferred, out socketFlags, (SafeHandle) e.m_PtrNativeOverlapped, IntPtr.Zero);
      }
      catch (Exception ex)
      {
        e.Complete();
        throw ex;
      }
      if (socketError != SocketError.Success)
        socketError = (SocketError) Marshal.GetLastWin32Error();
      bool flag;
      if (socketError != SocketError.Success && socketError != SocketError.IOPending)
      {
        e.FinishOperationSyncFailure(socketError, bytesTransferred, socketFlags);
        flag = false;
      }
      else
        flag = true;
      if (Socket.s_LoggingEnabled)
        Logging.Exit(Logging.Sockets, (object) this, "ReceiveAsync", (object) (bool) (flag ? 1 : 0));
      return flag;
    }

    public bool ReceiveFromAsync(SocketAsyncEventArgs e)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "ReceiveFromAsync", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (e.RemoteEndPoint == null)
        throw new ArgumentNullException("RemoteEndPoint");
      if (e.RemoteEndPoint.AddressFamily != this.addressFamily)
      {
        throw new ArgumentException(SR.GetString("net_InvalidEndPointAddressFamily", (object) e.RemoteEndPoint.AddressFamily, (object) this.addressFamily), "RemoteEndPoint");
      }
      else
      {
        EndPoint remoteEndPoint = e.RemoteEndPoint;
        e.m_SocketAddress = this.SnapshotAndSerialize(ref remoteEndPoint);
        e.StartOperationCommon(this);
        e.StartOperationReceiveFrom();
        this.BindToCompletionPort();
        SocketFlags socketFlags = e.m_SocketFlags;
        int bytesTransferred;
        SocketError socketError;
        try
        {
          socketError = e.m_Buffer == null ? UnsafeNclNativeMethods.OSSOCK.WSARecvFrom(this.m_Handle, e.m_WSABufferArray, e.m_WSABufferArray.Length, out bytesTransferred, out socketFlags, e.m_PtrSocketAddressBuffer, e.m_PtrSocketAddressBufferSize, (SafeHandle) e.m_PtrNativeOverlapped, IntPtr.Zero) : UnsafeNclNativeMethods.OSSOCK.WSARecvFrom(this.m_Handle, out e.m_WSABuffer, 1, out bytesTransferred, out socketFlags, e.m_PtrSocketAddressBuffer, e.m_PtrSocketAddressBufferSize, (SafeHandle) e.m_PtrNativeOverlapped, IntPtr.Zero);
        }
        catch (Exception ex)
        {
          e.Complete();
          throw ex;
        }
        if (socketError != SocketError.Success)
          socketError = (SocketError) Marshal.GetLastWin32Error();
        bool flag;
        if (socketError != SocketError.Success && socketError != SocketError.IOPending)
        {
          e.FinishOperationSyncFailure(socketError, bytesTransferred, socketFlags);
          flag = false;
        }
        else
          flag = true;
        if (Socket.s_LoggingEnabled)
          Logging.Exit(Logging.Sockets, (object) this, "ReceiveFromAsync", (object) (bool) (flag ? 1 : 0));
        return flag;
      }
    }

    public bool ReceiveMessageFromAsync(SocketAsyncEventArgs e)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "ReceiveMessageFromAsync", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (e.RemoteEndPoint == null)
        throw new ArgumentNullException("RemoteEndPoint");
      if (e.RemoteEndPoint.AddressFamily != this.addressFamily)
      {
        throw new ArgumentException(SR.GetString("net_InvalidEndPointAddressFamily", (object) e.RemoteEndPoint.AddressFamily, (object) this.addressFamily), "RemoteEndPoint");
      }
      else
      {
        EndPoint remoteEndPoint = e.RemoteEndPoint;
        e.m_SocketAddress = this.SnapshotAndSerialize(ref remoteEndPoint);
        this.SetReceivingPacketInformation();
        e.StartOperationCommon(this);
        e.StartOperationReceiveMessageFrom();
        this.BindToCompletionPort();
        int bytesTransferred;
        SocketError socketError;
        try
        {
          socketError = this.WSARecvMsg(this.m_Handle, e.m_PtrWSAMessageBuffer, out bytesTransferred, (SafeHandle) e.m_PtrNativeOverlapped, IntPtr.Zero);
        }
        catch (Exception ex)
        {
          e.Complete();
          throw ex;
        }
        if (socketError != SocketError.Success)
          socketError = (SocketError) Marshal.GetLastWin32Error();
        bool flag;
        if (socketError != SocketError.Success && socketError != SocketError.IOPending)
        {
          e.FinishOperationSyncFailure(socketError, bytesTransferred, SocketFlags.None);
          flag = false;
        }
        else
          flag = true;
        if (Socket.s_LoggingEnabled)
          Logging.Exit(Logging.Sockets, (object) this, "ReceiveMessageFromAsync", (object) (bool) (flag ? 1 : 0));
        return flag;
      }
    }

    public bool SendAsync(SocketAsyncEventArgs e)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "SendAsync", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      e.StartOperationCommon(this);
      e.StartOperationSend();
      this.BindToCompletionPort();
      int bytesTransferred;
      SocketError socketError;
      try
      {
        socketError = e.m_Buffer == null ? UnsafeNclNativeMethods.OSSOCK.WSASend(this.m_Handle, e.m_WSABufferArray, e.m_WSABufferArray.Length, out bytesTransferred, e.m_SocketFlags, (SafeHandle) e.m_PtrNativeOverlapped, IntPtr.Zero) : UnsafeNclNativeMethods.OSSOCK.WSASend(this.m_Handle, ref e.m_WSABuffer, 1, out bytesTransferred, e.m_SocketFlags, (SafeHandle) e.m_PtrNativeOverlapped, IntPtr.Zero);
      }
      catch (Exception ex)
      {
        e.Complete();
        throw ex;
      }
      if (socketError != SocketError.Success)
        socketError = (SocketError) Marshal.GetLastWin32Error();
      bool flag;
      if (socketError != SocketError.Success && socketError != SocketError.IOPending)
      {
        e.FinishOperationSyncFailure(socketError, bytesTransferred, SocketFlags.None);
        flag = false;
      }
      else
        flag = true;
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "SendAsync", (object) (bool) (flag ? 1 : 0));
      return flag;
    }

    public bool SendPacketsAsync(SocketAsyncEventArgs e)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "SendPacketsAsync", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (!this.Connected)
        throw new NotSupportedException(SR.GetString("net_notconnected"));
      e.StartOperationCommon(this);
      e.StartOperationSendPackets();
      this.BindToCompletionPort();
      bool flag1;
      try
      {
        flag1 = this.TransmitPackets(this.m_Handle, e.m_PtrSendPacketsDescriptor, e.m_SendPacketsElements.Length, e.m_SendPacketsSendSize, e.m_PtrNativeOverlapped, e.m_SendPacketsFlags);
      }
      catch (Exception ex)
      {
        e.Complete();
        throw ex;
      }
      SocketError socketError = flag1 ? SocketError.Success : (SocketError) Marshal.GetLastWin32Error();
      bool flag2;
      if (socketError != SocketError.Success && socketError != SocketError.IOPending)
      {
        e.FinishOperationSyncFailure(socketError, 0, SocketFlags.None);
        flag2 = false;
      }
      else
        flag2 = true;
      if (Socket.s_LoggingEnabled)
        Logging.Exit(Logging.Sockets, (object) this, "SendPacketsAsync", (object) (bool) (flag2 ? 1 : 0));
      return flag2;
    }

    public bool SendToAsync(SocketAsyncEventArgs e)
    {
      if (Socket.s_LoggingEnabled)
        Logging.Enter(Logging.Sockets, (object) this, "SendToAsync", "");
      if (this.CleanedUp)
        throw new ObjectDisposedException(this.GetType().FullName);
      if (e.RemoteEndPoint == null)
        throw new ArgumentNullException("RemoteEndPoint");
      EndPoint remoteEndPoint = e.RemoteEndPoint;
      e.m_SocketAddress = this.CheckCacheRemote(ref remoteEndPoint, false);
      e.StartOperationCommon(this);
      e.StartOperationSendTo();
      this.BindToCompletionPort();
      int bytesTransferred;
      SocketError socketError;
      try
      {
        socketError = e.m_Buffer == null ? UnsafeNclNativeMethods.OSSOCK.WSASendTo(this.m_Handle, e.m_WSABufferArray, e.m_WSABufferArray.Length, out bytesTransferred, e.m_SocketFlags, e.m_PtrSocketAddressBuffer, e.m_SocketAddress.m_Size, e.m_PtrNativeOverlapped, IntPtr.Zero) : UnsafeNclNativeMethods.OSSOCK.WSASendTo(this.m_Handle, ref e.m_WSABuffer, 1, out bytesTransferred, e.m_SocketFlags, e.m_PtrSocketAddressBuffer, e.m_SocketAddress.m_Size, (SafeHandle) e.m_PtrNativeOverlapped, IntPtr.Zero);
      }
      catch (Exception ex)
      {
        e.Complete();
        throw ex;
      }
      if (socketError != SocketError.Success)
        socketError = (SocketError) Marshal.GetLastWin32Error();
      bool flag;
      if (socketError != SocketError.Success && socketError != SocketError.IOPending)
      {
        e.FinishOperationSyncFailure(socketError, bytesTransferred, SocketFlags.None);
        flag = false;
      }
      else
        flag = true;
      if (Socket.s_LoggingEnabled)
        Logging.Exit(Logging.Sockets, (object) this, "SendToAsync", (object) (bool) (flag ? 1 : 0));
      return flag;
    }

    private class CacheSet
    {
      internal CallbackClosure ConnectClosureCache;
      internal CallbackClosure AcceptClosureCache;
      internal CallbackClosure SendClosureCache;
      internal CallbackClosure ReceiveClosureCache;
      internal OverlappedCache SendOverlappedCache;
      internal OverlappedCache ReceiveOverlappedCache;
    }

    private class MultipleAddressConnectAsyncResult : ContextAwareResult
    {
      internal Socket socket;
      internal IPAddress[] addresses;
      internal int index;
      internal int port;
      internal Exception lastException;

      internal EndPoint RemoteEndPoint
      {
        get
        {
          if (this.addresses != null && this.index > 0 && this.index < this.addresses.Length)
            return (EndPoint) new IPEndPoint(this.addresses[this.index], this.port);
          else
            return (EndPoint) null;
        }
      }

      internal MultipleAddressConnectAsyncResult(IPAddress[] addresses, int port, Socket socket, object myState, AsyncCallback myCallBack)
        : base((object) socket, myState, myCallBack)
      {
        this.addresses = addresses;
        this.port = port;
        this.socket = socket;
      }
    }

    private class DownLevelSendFileAsyncResult : ContextAwareResult
    {
      internal Socket socket;
      internal FileStream fileStream;
      internal byte[] buffer;
      internal bool writing;

      internal DownLevelSendFileAsyncResult(FileStream stream, Socket socket, object myState, AsyncCallback myCallBack)
        : base((object) socket, myState, myCallBack)
      {
        this.socket = socket;
        this.fileStream = stream;
        this.buffer = new byte[64000];
      }
    }
  }
}
