using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Framework2D.Multiplayer
{


    public enum GameAction
    {
        Ready = 0,
        TurnLeft = 1,
        TurnRight = 2,
        Accelerate = 3,
        Deaccelerate = 4,
        Shoot = 5,
        Done = 6,
        Update = 7,
        Star = 8,
        StarFighter = 9,
        NewPlayer = 10
    }


    //consts


    class ClientSide
    {

        public static TcpClient Client = new TcpClient();
        public string ipHost = "";
        public bool isWait = true;
        public IPAddress ipV6;
        public IPAddress ip;
        public int Port = 8080;
        public void InputIPForm()
        {
            var name = Dns.GetHostName();
            ipV6 = Dns.GetHostEntry(name).AddressList.ToList().Find((p) => (p.AddressFamily == AddressFamily.InterNetworkV6));
            ip = Dns.GetHostEntry(name).AddressList.ToList().Find((p) => (p.AddressFamily == AddressFamily.InterNetwork));
            var inputBox = new Form();
            var btnOK = new Button() { Text = "ok", Size = new System.Drawing.Size(100, 50), Location = new System.Drawing.Point(0, 25) };
            var txtIP = new TextBox() { Text = "ip here", Size = new System.Drawing.Size(100, 50) };
            btnOK.Click += new EventHandler((q, w) => { ipHost = txtIP.Text; isWait = false;  inputBox.Close(); });
            inputBox.Controls.Add(txtIP);
            inputBox.Controls.Add(btnOK);
            inputBox.Show();

        }

        public void Connect()
        {
            Client.BeginConnect(IPAddress.Parse(ipHost), Port, ConnectCallBack, Client);
        }

        public void ConnectCallBack(IAsyncResult ar)
        {
            var client = (TcpClient)ar;
            if (client.Connected)
            {
                Console.WriteLine("Connect Successful");
            }

        }
    }
}
