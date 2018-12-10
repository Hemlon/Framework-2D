using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Framework2D.Multiplayer
{
    public class ServerSide
    {
        public int Port = 8080;
        public TcpListener listener;
        public IPAddress ip = default(IPAddress);
        public IPAddress ipV6;
     //   public static List<TcpClient> ConnectedClients = new List<TcpClient>();
        public Thread ConnectNewClient;

        public void StartServer()
        {
            var name = Dns.GetHostName();
            var ipV6 = Dns.GetHostEntry(name).AddressList.ToList().Find((p) => (p.AddressFamily == AddressFamily.InterNetworkV6));
            var ip = Dns.GetHostEntry(name).AddressList.ToList().Find((p) => (p.AddressFamily == AddressFamily.InterNetwork));
            GameWindow.GameMessages[0] = ip.ToString();
            var forceip = IPAddress.Parse("10.0.0.120");
            listener = new TcpListener(ip, Port);
            listener.Start();
            Console.WriteLine("Starting Server IP:" + ip + " Port:" + Port);
            Console.WriteLine("Waiting for clients...");

            while (true)
            {
                while (!listener.Pending())
                {
                    Console.WriteLine("Sleeping...");
                    Thread.Sleep(5000);
                }
                Console.WriteLine("Wake up! connecting client");
                ConnectClients newConnection = new ConnectClients();
                newConnection.listener = this.listener;
                ConnectNewClient = new Thread(new ThreadStart(newConnection.AcceptConnection));
                ConnectNewClient.Start();
            }

           
        }

        public class ConnectClients
        {
            public TcpListener listener;
            public static List<TcpClient> ConnectedClients = new List<TcpClient>();

            public void AcceptConnection()
            {
                listener.BeginAcceptSocket((r) =>
                {
                   // AcceptConnection();
                    TcpClient client = listener.EndAcceptTcpClient(r);
                    ConnectedClients.Add(client);
                    Console.WriteLine("New Connected Client" + client.Client.RemoteEndPoint.ToString());
                    Console.WriteLine("Pool Size " + ConnectedClients.Count);

                }, listener);
            }

            
        }

        /// <summary>
        /// Depreciated
        /// </summary>
        public void AcceptClient()
        {
            listener.BeginAcceptTcpClient(
                        (r) =>
                        {
                            AcceptClient();
                           // var server = (TcpListener)r.AsyncState;                        
                            TcpClient client = listener.EndAcceptTcpClient(r);
                            //ConnectedClients.Add(client);
                            Console.WriteLine("New Connected Client" + client.Client.RemoteEndPoint.ToString());
                        }
                         , listener);
          

        }
        public void StopServer()
        {
            ConnectNewClient.Abort();
        }
        public static void SendData(GameAction command, short[] dat, NetworkStream ns)
        {
            SendData((short)command, dat, ns);
        }
        public static void SendData(GameAction objType, string dat, NetworkStream ns)
        {
            try
            {
                ns.Write(BitConverter.GetBytes((short)objType), 0, sizeof(short));
                var b = BitConverter.GetBytes((short)dat.Length);
                ns.Write(b, 0, b.Length);
                ns.Write(Encoding.ASCII.GetBytes(dat), 0, dat.Length);
                ns.Flush();
            }
            catch { }
         
        }
        public static void SendData(short command, short[] dat, NetworkStream ns)
        {
            ns.Write(BitConverter.GetBytes(command), 0, sizeof(short));
            for (int i = 0; i < dat.Length; i++)
                ns.Write(BitConverter.GetBytes(dat[i]), 0, sizeof(short));
            ns.Flush();
        }
        public static void SendCommand(GameAction command, NetworkStream ns)
        {
            ns.Write(BitConverter.GetBytes((short)command), 0, sizeof(short));
            ns.Flush();
        }

    }
}
