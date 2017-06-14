using System;
using System.Net.Sockets;

namespace ERat.Net {

    public class ClientStatus {

        public ClientStatus(Socket handler, int bufferSize) : this(handler, new byte[bufferSize]) { }

        public ClientStatus(Socket handler, byte[] buffer) {
            Handler = handler;
            Buffer = buffer;

            UniqueID = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 8);
        }

        public string UniqueID { get; private set; }
        public Socket Handler { get; private set; }
        public byte[] Buffer { get; private set; }

        public delegate void ClientDisconnectedHandler(ClientStatus client);
        public event ClientDisconnectedHandler OnClientDisconnection;
        public void RaiseClientDisconnectedEvent() => OnClientDisconnection?.Invoke(this);

        public delegate void DataReceivedHandler(ClientStatus client, int dataLength);
        public event DataReceivedHandler OnDataReceiving;
        public void RaiseDataReceivedEvent(int dataLength) => OnDataReceiving?.Invoke(this, dataLength);
    }
}
