using System;
using System.Net;
using System.Net.Sockets;
using static ERat.Misc.ConsoleWriter;
using static ERat.Misc.Configurations;

namespace ERat.Net {

    class ServerSocket {
        public int Port { get; private set; }
        public int Backlog { get; private set; }

        private Socket ConnectionsHandler { get; set; } = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public ServerSocket(int port = 6699, int backlog = 100) {
            if (port < 1024 || port > 65535) {
                WriteError("Port {0} is invalid (OutOfRange)", port);
                Console.ReadKey();
                Environment.Exit(0);
            }
            else {
                Port = port;
            }

            Backlog = backlog;
        }

        public void Listen() {
            ConnectionsHandler.Bind(new IPEndPoint(0, Port));
            ConnectionsHandler.Listen(Backlog);
            WriteInfo("Listener Started on port {0}", Port);

            ConnectionsHandler.BeginAccept(new AsyncCallback(AcceptCallback), ConnectionsHandler);
        }

        private void AcceptCallback(IAsyncResult ar) {
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            handler.BeginReceive(new ClientStatus(handler, BufferSize).Buffer, 0, BufferSize, SocketFlags.None, new AsyncCallback(ReceiveCallback), new ClientStatus(handler, BufferSize));
            ConnectionsHandler.BeginAccept(new AsyncCallback(AcceptCallback), ConnectionsHandler);
        }

        private void ReceiveCallback(IAsyncResult ar) {
            ClientStatus client = (ClientStatus)ar.AsyncState;
            try {
                client.RaiseDataReceivedEvent(client.Handler.EndReceive(ar));
                client.Handler.BeginReceive(client.Buffer, 0, BufferSize, SocketFlags.None, new AsyncCallback(ReceiveCallback), client);
            }
            catch (SocketException ex) when (ex.ErrorCode == 10054) {
                client.RaiseClientDisconnectedEvent();
                WriteError("Lost connection to client {0}", client.Handler.LocalEndPoint.ToString());
            }
        }
    }
}
