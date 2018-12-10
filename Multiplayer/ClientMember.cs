using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Framework2D.Multiplayer
{
    public class ClientMember
    {
       public TcpClient tcpClient;
        public string key = "";
     public   NetworkStream ns;
   // public    StreamWriter sw;
  //public      StreamReader sr;
        public string status = "";

        public TcpClient Client
        {
            get
            {
                return tcpClient;
            }

            set
            {
                tcpClient = value;
            }
        }
        public NetworkStream Ns
        {
            get
            {
                return ns;
            }

            set
            {
                ns = value;
            }
        }
        //public StreamWriter Sw
        //{
        //    get
        //    {
        //        return sw;
        //    }

        //    set
        //    {
        //        sw = value;
        //    }
        //}
        //public StreamReader Sr
        //{
        //    get
        //    {
        //        return sr;
        //    }

        //    set
        //    {
        //        sr = value;
        //    }
        //}
        public ClientMember()
        {
            Client = new TcpClient();
          
            Ns = Client.GetStream();
        }
        public ClientMember(TcpClient client, string key, NetworkStream ns) :base()
        {
            Ns = ns; Client = client;// Sw = sw; Sr = sr;
            try
            {
                this.key = key;
            }
            catch { key = ""; }


        }
      
    }
}

