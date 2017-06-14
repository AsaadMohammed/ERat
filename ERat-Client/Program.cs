using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using ERat.Net;
using static ERat.Misc.ConsoleWriter;

namespace ERat {
    class Program {
        static void Main(string[] args) {
            ClientSocket clientSocket = new ClientSocket(IPAddress.Parse("127.0.0.1"), 6699, out ServerStatus serverStatus);        

            serverStatus.OnDataReceiving += (s, l) => {
                s.BufferQueue.TryDequeue(out byte[] buffer);
                WriteInfo("Data received from server: {0}", Encoding.Default.GetString(buffer));
            };

            serverStatus.OnServerConnected += (s) => {
                WriteInfo("Connected to server");
                // TODO: Receive the client encryption key
            };

            serverStatus.OnServerDisconnected += (s, c) => {
                WriteInfo("Connection to server lost");
                c.connect();
            };

            clientSocket.connect();

            System.Diagnostics.Process.GetCurrentProcess().WaitForExit();
        }
    }
}
