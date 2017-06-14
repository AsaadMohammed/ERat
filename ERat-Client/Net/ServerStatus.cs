using System.Net;
using System.Net.Sockets;
using ERat.Net.Packets;
using System.Collections.Concurrent;

namespace ERat.Net {

    class ServerStatus {
        public Socket Handler { get; private set; }
        public IPAddress IpAddress { get; private set; }
        public byte[] Buffer { get; set; }

        public ConcurrentQueue<byte[]> BufferQueue { get; set; } = new ConcurrentQueue<byte[]>();
        public ConcurrentQueue<PacketInterface> PacketsQueue { get; set; } = new ConcurrentQueue<PacketInterface>();

        public ServerStatus(Socket handler, IPAddress ipAddress) {
            Handler = handler;
            IpAddress = ipAddress;

            Buffer = new byte[ERat.Misc.Configurations.BufferSize];
        }

        public delegate void ServerDisconnectedHandler(ServerStatus server);
        public event ServerDisconnectedHandler OnServerDisconnected;
        public void RaiseServerDisconnectedEvent() => OnServerDisconnected?.Invoke(this);

        public delegate void DataReceivedHandler(ServerStatus client, int dataLength);
        public event DataReceivedHandler OnDataReceiving;
        public void RaiseDataReceivedEvent(int dataLength) => OnDataReceiving?.Invoke(this, dataLength);
    }
}
