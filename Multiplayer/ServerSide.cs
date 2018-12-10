using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Collections.Concurrent;

namespace Framework2D.Multiplayer
{
    public class ServerSide
    {
        public int Port = 8080;
        public TcpListener listener;
        public IPAddress ip = default(IPAddress);
        public IPAddress ipV6;
       public static List<Thread> ConnectionThreads = new List<Thread>();
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
                    Console.WriteLine("Listener Sleeping...");
                    Thread.Sleep(5000);
                }
                Console.WriteLine("Listener Wake up! connecting client");
                ConnectClients newConnection = new ConnectClients();
                newConnection.listener = this.listener;
                ConnectNewClient = new Thread(new ThreadStart(newConnection.AcceptConnection));
                ConnectionThreads.Add(ConnectNewClient);
                ConnectNewClient.Start();
            }

           
        }

        public class ConnectClients
        {
            public TcpListener listener;
            public static ConcurrentDictionary<int,TcpClient> ConnectedClients = new ConcurrentDictionary<int,TcpClient>();

            public void AcceptConnection()
            {
                listener.BeginAcceptSocket((r) =>
                {
                   // AcceptConnection();
                    TcpClient client = listener.EndAcceptTcpClient(r);
                    ConnectedClients.TryAdd(ConnectedClients.Count,client);
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
               // var t = new Thread(new ThreadStart(() =>{
                    ns.Write(BitConverter.GetBytes((short)objType), 0, sizeof(short));
                    var b = BitConverter.GetBytes((short)dat.Length);
                    ns.Write(b, 0, b.Length);
                    ns.Write(Encoding.ASCII.GetBytes(dat), 0, dat.Length);
                    ns.Flush();
             //   }));t.Start();
            }
            catch { }
         
        }
        public static void SendData(short command, short[] dat, NetworkStream ns)
        {
           // var t = new Thread(new ThreadStart(() => {
            ns.Write(BitConverter.GetBytes(command), 0, sizeof(short));
            for (int i = 0; i < dat.Length; i++)
                ns.Write(BitConverter.GetBytes(dat[i]), 0, sizeof(short));
            ns.Flush();
             //    })); t.Start();
        }
        public static void SendCommand(GameAction command, NetworkStream ns)
        {
            // var t = new Thread(new ThreadStart(() =>{
            ns.Write(BitConverter.GetBytes((short)command), 0, sizeof(short));
            ns.Flush();
            //    }));t.Start();
        }
        public static GameAction RecvCommand(NetworkStream ns)
        {
            byte[] b = new byte[sizeof(short)];
            ns.Read(b, 0, b.Length);
            return (GameAction)BitConverter.ToInt16(b, 0);
        }
        public static string RecvShortString(NetworkStream ns)
        {   
            var b = new byte[2];
            ns.Read(b,0,2);
            var size = (short)BitConverter.ToInt16(b,0);
            b = new byte[size];
            ns.Read(b,0,b.Length);
            return Encoding.ASCII.GetString(b);
        }

    }
}

