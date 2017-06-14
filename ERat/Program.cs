using ERat.Net;
using ERat.Encryption;

namespace ERat {
    class Program {
        static void Main(string[] args) {
            // Start the server listener
            var serverSocket = new ServerSocket();
            serverSocket.Listen();

            System.Diagnostics.Process.GetCurrentProcess().WaitForExit();
        }
    }
}
