using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using ERat.Net.Packets;
using static ERat.Misc.ConsoleWriter;

namespace ERat.Net {
    class ClientSocket {
        public int Port { get; private set; }
        public IPAddress IpAddress { get; private set; }

        private Socket ConnectionsHandler { get; set; } = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public ClientSocket(string hostname, int port) {
            throw new NotImplementedException();
        }

        public ClientSocket(IPAddress ipAddress, int port) {
            IpAddress = ipAddress;
            Port = port;
        }

        public void connect() {
            ConnectionsHandler.BeginConnect(IpAddress, Port, new AsyncCallback(ConnectCallback), ConnectionsHandler);
        }

        private void ConnectCallback(IAsyncResult ar) {
            try {
                Socket handler = (Socket)ar.AsyncState;
                handler.EndConnect(ar);
                WriteInfo("Connected to {0}:{1}", IpAddress.ToString(), Port);

                ServerStatus serverStatus = new ServerStatus(handler, IpAddress);
                serverStatus.OnDataReceiving += new ServerStatus.DataReceivedHandler(DataDispatcher);
                serverStatus.OnServerDisconnected += new ServerStatus.ServerDisconnectedHandler(ServerDisconnected);

                handler.BeginReceive(serverStatus.Buffer, 0, ERat.Misc.Configurations.BufferSize, SocketFlags.None, new AsyncCallback(ReceiveCallback), serverStatus);
            }
            catch (SocketException ex) when (ex.ErrorCode == 10061) {
                WriteError("Connection refused");
                WriteInfo("Reconnecting");
                connect();
            }
        }

        private void ReceiveCallback(IAsyncResult ar) {
            try {
                ServerStatus serverStatus = (ServerStatus)ar.AsyncState;
                int length = serverStatus.Handler.EndReceive(ar);

                serverStatus.BufferQueue.Enqueue(serverStatus.Buffer.Take(length).ToArray());

                serverStatus.RaiseDataReceivedEvent(length);
                serverStatus.Handler.BeginReceive(serverStatus.Buffer, 0, ERat.Misc.Configurations.BufferSize, SocketFlags.None, new AsyncCallback(ReceiveCallback), serverStatus);
            }
            catch (SocketException ex) when (ex.ErrorCode == 10054) {
                WriteError("Connection Lost");
                ConnectionsHandler.Disconnect(true);
                connect();
            }
        }

        public void DataDispatcher(ServerStatus server, int dataLength) {
            byte[] packet;
            server.BufferQueue.TryDequeue(out packet);
            WriteInfo("{0}", System.Text.Encoding.Default.GetString(packet));
        }

        public void ServerDisconnected(ServerStatus server) {
            WriteError("Connection Lost");
        }
    }
}
