using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoCaro
{
    class LAN
    {
        // init local ip
        public static string localIP;

        // init receiving ip
        public static string receivingIP;

        // port and broadcast
        private const int port = 1234;
        private const int hello_port = 1235;
        private const string broadCastAddress = "255.255.255.255";

        // init receiver
        private static UdpClient receivingClient;
        private static UdpClient waitForClient;

        // init sender
        private static UdpClient sendingClient;
        private static UdpClient helloHost;

        // tạo thread riêng cho việc nhận data
        private static Thread receivingThread;
        private static Thread waitClientThread;
        //private static Thread getHostnameThread;

        // lấy ip trong mạng lan
        public static void GetLocalIp()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0);
            socket.Connect("8.8.8.8", 65530);
            IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
            localIP = endPoint.Address.ToString();
        }

        public static void InitHelloHost(string sendtoIP)
        {
            helloHost = new UdpClient(sendtoIP, hello_port);
            helloHost.EnableBroadcast = true;

            byte[] send_test = Encoding.ASCII.GetBytes("hello:" + localIP);
            helloHost.Send(send_test, send_test.Length);

            helloHost.Close();
        }

        public static void InitWaitForClient()
        {
            waitForClient = new UdpClient(hello_port);

            ThreadStart start = new ThreadStart(WaitForClient);
            waitClientThread = new Thread(start);
            waitClientThread.IsBackground = true;
            waitClientThread.Start();
        }

        public static void WaitForClient()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, hello_port);

            while (true)
            {
                byte[] data = waitForClient.Receive(ref endPoint);
                string message = Encoding.ASCII.GetString(data);

                // get host player name: "hello:clientIP"
                string[] mess_code = message.Split(':');

                // giao tiếp giữa 2 client
                if (mess_code[0].Equals("hello"))
                {
                    InitSender(mess_code[1]);
                    InitReceiver(mess_code[1]);
                    SendData("hello_form_host");

                    waitForClient.Close();
                    waitClientThread.Abort();
                    break;
                }
            }
        }

        // khởi tạo sender
        public static void InitSender(string sendtoIP)
        {
            sendingClient = new UdpClient(sendtoIP, port);
            sendingClient.EnableBroadcast = true;
        }

        // khởi tạo receiver
        public static void InitReceiver(string listentoIP)
        {
            receivingClient = new UdpClient(port);
            receivingIP = listentoIP;

            ThreadStart start = new ThreadStart(Receiver);
            receivingThread = new Thread(start);
            receivingThread.IsBackground = true;
            receivingThread.Start();
        }

        private static void Receiver()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(receivingIP), port);

            while (true)
            {
                byte[] data = receivingClient.Receive(ref endPoint);
                string message = Encoding.ASCII.GetString(data);

                // get host player name: "get:hostname"
                string[] mess_code = message.Split(':');

                // giao tiếp giữa 2 client
                switch(mess_code[0])
                {
                    case "hello_form_host":
                        // init sender
                        InitSender(receivingIP);

                        // get hostname
                        SendData("get:hostname");
                        SendData("set:joinname:" + FormLogin.join_name);
                        break;
                    case "set":
                        switch(mess_code[1])
                        {
                            case "hostname":
                                FormLogin.host_name = mess_code[2];
                                break;
                            case "joinname":
                                FormLogin.join_name = mess_code[2];
                                break;
                            case "play":
                                int player = Convert.ToInt32(mess_code[2]);
                                int x = Convert.ToInt32(mess_code[3]);
                                int y = Convert.ToInt32(mess_code[4]);
                                Caro.DanhCo(x, y, player, Form1.grs);
                                Form1.turn++;
                                //MessageBox.Show(Convert.ToString(Form1.turn));
                                break;
                        }
                        break;
                    case "get":
                        switch (mess_code[1])
                        {
                            case "hostname":
                                SendData("set:hostname:" + FormLogin.host_name);
                                break;
                        }
                        break;
                }
            }
        }

        public static int SendData(string mess)
        {
            byte[] send_test = Encoding.ASCII.GetBytes(mess);
            int res = sendingClient.Send(send_test, send_test.Length);
            return res;
        }
    }
}
