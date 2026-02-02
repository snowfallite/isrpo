using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientApp
{
    class Program
    {
        
        const string ServerHost = "server-app"; 
        
        static async Task Main(string[] args)
        {
            
            Console.WriteLine("CLIENT: Waiting for server to start...");
            Thread.Sleep(3000);

       
            Console.WriteLine("\n--- Task A: Sending UDP and TCP messages ---");
            
          
            try {
                using UdpClient udpClient = new UdpClient();
                string udpMsg = "Hello via UDP!";
                byte[] data = Encoding.UTF8.GetBytes(udpMsg);
               
                udpClient.Send(data, data.Length, ServerHost, 11000);
                Console.WriteLine("Client sent UDP message.");
            } catch (Exception ex) { Console.WriteLine($"UDP Error: {ex.Message}"); }

           
            try {
                using TcpClient tcpClient = new TcpClient(ServerHost, 11001);
                string tcpMsg = "Hello via TCP!";
                byte[] data = Encoding.UTF8.GetBytes(tcpMsg);
                tcpClient.GetStream().Write(data, 0, data.Length);
                Console.WriteLine("Client sent TCP message.");
            } catch (Exception ex) { Console.WriteLine($"TCP Error: {ex.Message}"); }

            // ЗАДАНИЕ B 
            Console.WriteLine("\n--- Task B: Echo Server Test ---");
           
            var t1 = Task.Run(() => RunEchoClient(1));
            var t2 = Task.Run(() => RunEchoClient(2));
            
            await Task.WhenAll(t1, t2);

            Console.WriteLine("\nCLIENT: Work finished.");
            
        
            Thread.Sleep(Timeout.Infinite);
        }

        static void RunEchoClient(int id)
        {
            try
            {
                using TcpClient client = new TcpClient(ServerHost, 11002);
                using NetworkStream stream = client.GetStream();
                
                for (int i = 0; i < 3; i++)
                {
                    string msg = $"Client {id} message {i}";
                    byte[] data = Encoding.UTF8.GetBytes(msg);
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine($"[Client {id}] Sent: {msg}");

                    //  ответ
                    byte[] buffer = new byte[256];
                    int bytes = stream.Read(buffer, 0, buffer.Length);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytes);
                    Console.WriteLine($"[Client {id}] Echo Received: {response}");
                    
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Client {id}] Error: {ex.Message}");
            }
        }
    }
}