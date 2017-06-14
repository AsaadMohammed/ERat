using System.Net;
using System.Net.Sockets;
using ERat.Net.Packets;
using System.Collections.Concurrent;

namespace ERat.Net {

    class ServerStatus {
        public Socket Handler { get; private set; }
        public IPAddress IpAddress { get; private set; }
        public byte[] ReceiveBuffer { get; set; }

        public bool ServerConnected { get; set; } = false;
        public ConcurrentQueue<byte[]> BufferQueue { get; set; } = new ConcurrentQueue<byte[]>();
        public ConcurrentQueue<PacketInterface> PacketsQueue { get; set; } = new ConcurrentQueue<PacketInterface>();

        public ServerStatus(Socket handler, IPAddress ipAddress) {
            Handler = handler;
            IpAddress = ipAddress;

            ReceiveBuffer = new byte[Misc.Configurations.BufferSize];
        }

        public delegate void ServerConnectedHandler(ServerStatus server);
        public event ServerConnectedHandler OnServerConnected;
        public void RaiseServerConnectedEvent() { ServerConnected = true; OnServerConnected?.Invoke(this); }

        public delegate void ServerDisconnectedHandler(ServerStatus server, ClientSocket socket);
        public event ServerDisconnectedHandler OnServerDisconnected;
        public void RaiseServerDisconnectedEvent(ClientSocket socket) { ServerConnected = false; OnServerDisconnected?.Invoke(this, socket); }

        public delegate void DataReceivedHandler(ServerStatus client, int dataLength);
        public event DataReceivedHandler OnDataReceiving;
        public void RaiseDataReceivedEvent(int dataLength) { if (ServerConnected) OnDataReceiving?.Invoke(this, dataLength); }
    }
}
