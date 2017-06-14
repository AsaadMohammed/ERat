using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using ERat.Net;

namespace ERat {
    class Program {
        static void Main(string[] args) {
            ClientSocket clientSocket = new ClientSocket(IPAddress.Parse("127.0.0.1"), 6699);
            clientSocket.connect();

            System.Diagnostics.Process.GetCurrentProcess().WaitForExit();
        }
    }
}
