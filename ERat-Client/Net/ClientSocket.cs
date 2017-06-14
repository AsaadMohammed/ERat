using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ERat.Net.Packets;
using static ERat.Misc.ConsoleWriter;

namespace ERat.Net {
    class ClientSocket {
        public int Port { get; private set; }
        public IPAddress IpAddress { get; private set; }
        public Thread ServerConnectionCheck { get; private set; }
        public ServerStatus ConnectedServerStatus { get; private set; }
        public Socket ConnectionsHandler { get; private set; } = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // TODO: Find a way to select a good ip address from the `GetHostAddresses` function returned array
        public ClientSocket(string hostname, int port, out ServerStatus serverStatus) : this(Dns.GetHostAddresses(hostname)[1], port, out serverStatus) { throw new NotImplementedException(); }

        public ClientSocket(IPAddress ipAddress, int port, out ServerStatus serverStatus) {
            IpAddress = ipAddress;
            Port = port;
            serverStatus = ConnectedServerStatus = new ServerStatus(ConnectionsHandler, IpAddress);
        }

        public void connect() {
            ConnectionsHandler.BeginConnect(IpAddress, Port, new AsyncCallback(ConnectCallback), ConnectedServerStatus);
        }

        private void ConnectCallback(IAsyncResult ar) {
            ServerStatus serverStatus = (ServerStatus)ar.AsyncState;
            try {
                serverStatus.Handler.EndConnect(ar);
                serverStatus.RaiseServerConnectedEvent();
                serverStatus.Handler.BeginReceive(serverStatus.ReceiveBuffer, 0, serverStatus.ReceiveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), serverStatus);
            }
            catch (SocketException ex) when (ex.ErrorCode == 10061) {
                serverStatus.RaiseServerDisconnectedEvent(this);
            }
        }

        private void ReceiveCallback(IAsyncResult ar) {
            ServerStatus serverStatus = (ServerStatus)ar.AsyncState;
            try {
                int length = serverStatus.Handler.EndReceive(ar);

                serverStatus.BufferQueue.Enqueue(serverStatus.ReceiveBuffer.Take(length).ToArray());

                serverStatus.RaiseDataReceivedEvent(length);
                serverStatus.Handler.BeginReceive(serverStatus.ReceiveBuffer, 0, serverStatus.ReceiveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), serverStatus);
            }
            catch (SocketException ex) when (ex.ErrorCode == 10054) {
                serverStatus.Handler.Disconnect(true);
                serverStatus.RaiseServerDisconnectedEvent(this);
            }
        }
    }
}
