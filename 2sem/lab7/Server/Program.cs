using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ServerApp
{
    class Program
    {
        
        const int PortTaskA_Udp = 11000;
        const int PortTaskA_Tcp = 11001;
        const int PortTaskB_Echo = 11002;
        const int PortTaskC_Http = 8080;

        static async Task Main(string[] args)
        {
            Console.WriteLine("SERVER: Starting services...");

          
            var taskA_Udp = Task.Run(RunTaskA_UdpServer);
            var taskA_Tcp = Task.Run(RunTaskA_TcpServer);
            var taskB = Task.Run(RunTaskB_EchoServer);
            var taskC = Task.Run(RunTaskC_HttpServer);

            await Task.WhenAll(taskA_Udp, taskA_Tcp, taskB, taskC);
        }


        static void RunTaskA_UdpServer()
        {
            try
            {
                UdpClient udpServer = new UdpClient(PortTaskA_Udp);
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                Console.WriteLine($"[A-UDP] Listening on port {PortTaskA_Udp}");

                while (true)
                {
                    byte[] data = udpServer.Receive(ref remoteEP);
                    string message = Encoding.UTF8.GetString(data);
                    Console.WriteLine($"[A-UDP] Received from {remoteEP}: {message}");
                }
            }
            catch (Exception ex) { Console.WriteLine($"[A-UDP] Error: {ex.Message}"); }
        }

       
        static void RunTaskA_TcpServer()
        {
            try
            {
                TcpListener listener = new TcpListener(IPAddress.Any, PortTaskA_Tcp);
                listener.Start();
                Console.WriteLine($"[A-TCP] Listening on port {PortTaskA_Tcp}");

                while (true)
                {
                    using TcpClient client = listener.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"[A-TCP] Received: {message}");
                }
            }
            catch (Exception ex) { Console.WriteLine($"[A-TCP] Error: {ex.Message}"); }
        }

        // ЗАДАНИЕ B 
        static void RunTaskB_EchoServer()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, PortTaskB_Echo);
            listener.Start();
            Console.WriteLine($"[B-Echo] Server started on port {PortTaskB_Echo}");

            while (true)
            {
             
                TcpClient client = listener.AcceptTcpClient();
                
                Task.Run(() => HandleEchoClient(client));
            }
        }

        static void HandleEchoClient(TcpClient client)
        {
            try
            {
                using (client)
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] buffer = new byte[256];
                    int i;
             
                    while ((i = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        string data = Encoding.UTF8.GetString(buffer, 0, i);
                        Console.WriteLine($"[B-Echo] Thread {Thread.CurrentThread.ManagedThreadId} received: {data}");
                        
                      
                        byte[] msg = Encoding.UTF8.GetBytes(data.ToUpper());
                        stream.Write(msg, 0, msg.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[B-Echo] Client error: {ex.Message}");
            }
        }

        // ЗАДАНИЕ C 
        static void RunTaskC_HttpServer()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, PortTaskC_Http);
            listener.Start();
            Console.WriteLine($"[C-HTTP] Web Server started on port {PortTaskC_Http} (ttp://localhost:8080");

            while (true)
            {
                try 
                {
                    using TcpClient client = listener.AcceptTcpClient();
                    using NetworkStream stream = client.GetStream();

                    
                    byte[] buffer = new byte[1024];
                    if(stream.DataAvailable) stream.Read(buffer, 0, buffer.Length);

                    string htmlBody =
@"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>C# Net Server</title>
    <style>
        @import url('https://fonts.googleapis.com/css2?family=Share+Tech+Mono&display=swap');
        
        body {
            background-color: #0d1117;
            color: #00ff41;
            font-family: 'Share Tech Mono', monospace;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
            margin: 0;
            overflow: hidden;
            text-align: center;
        }
        
        .container {
            border: 2px solid #30363d;
            padding: 40px;
            border-radius: 15px;
            background: #161b22;
            box-shadow: 0 0 30px rgba(0, 255, 65, 0.15);
            position: relative;
            max-width: 500px;
            width: 90%;
        }

        h1 {
            font-size: 2.5rem;
            margin-bottom: 10px;
            text-shadow: 0 0 10px #00ff41;
            text-transform: uppercase;
        }

        p {
            font-size: 1.2rem;
            color: #c9d1d9;
            margin: 10px 0;
        }

        .status {
            color: #2ea043;
            font-weight: bold;
            border: 1px solid #2ea043;
            padding: 2px 8px;
            border-radius: 4px;
        }

        .clock {
            font-size: 3rem;
            margin: 20px 0;
            color: #58a6ff;
            text-shadow: 0 0 15px rgba(88, 166, 255, 0.6);
        }

        .blink {
            animation: blinker 1s linear infinite;
        }

        @keyframes blinker {
            50% { opacity: 0; }
        }

        .footer {
            margin-top: 30px;
            font-size: 0.8rem;
            color: #8b949e;
        }
        
        /* Эффект сканирующей линии */
        .scanline {
            width: 100%;
            height: 100px;
            z-index: 10;
            background: linear-gradient(0deg, rgba(0,0,0,0) 0%, rgba(0, 255, 65, 0.04) 50%, rgba(0,0,0,0) 100%);
            opacity: 0.1;
            position: absolute;
            bottom: 100%;
            pointer-events: none;
            animation: scanline 10s linear infinite;
        }
        @keyframes scanline {
            0% { bottom: 100%; }
            100% { bottom: -100%; }
        }
    </style>
</head>
<body>
    <div class=""scanline""></div>
    <div class=""container"">
        <h1>System Online</h1>
        <div class=""clock"" id=""clock"">00:00:00</div>
        <p>Connection established securely.</p>
        <p>Waiting for commands<span class=""blink"">_</span></p>
        <div class=""footer"">Powered by C# Socket Server</div>
    </div>

    <script>
        function updateTime() {
            const now = new Date();
            const timeString = now.toLocaleTimeString();
            document.getElementById('clock').textContent = timeString;
        }
        setInterval(updateTime, 1000);
        updateTime();
    </script>
</body>
</html>";

                    int bodyLength = Encoding.UTF8.GetByteCount(htmlBody);

                    StringBuilder response = new StringBuilder();
                    response.Append("HTTP/1.1 200 OK\r\n");
                    response.Append($"Date: {DateTime.UtcNow:R}\r\n"); // Текущая дата
                    response.Append("Server: C# Custom Server\r\n");
                    response.Append("Content-Type: text/html; charset=utf-8\r\n");
                    response.Append($"Content-Length: {bodyLength}\r\n");
                    response.Append("Connection: close\r\n");
                    response.Append("\r\n");
                    response.Append(htmlBody);

                    byte[] responseBytes = Encoding.UTF8.GetBytes(response.ToString());
                    stream.Write(responseBytes, 0, responseBytes.Length);
                    Console.WriteLine("[C-HTTP] Served a cool web page.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[C-HTTP] Error: {ex.Message}");
                }
            }
        }
    }
}