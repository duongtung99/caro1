using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoCaro
{
    class LAN
    {
        // init local ip
        public static string localIP;

        // port and broadcast
        private const int port = 1234;
        private const string broadCastAddress = "255.255.255.255";

        // init receiver
        private UdpClient receivingClient;

        // init sender
        private UdpClient sendingClient;

        // tạo thread riêng cho việc nhận data
        private Thread receivingThread;

        public LAN()
        {
            GetLocalIp();
        }

        // lấy ip trong mạng lan
        public static void GetLocalIp()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0);
            socket.Connect("8.8.8.8", 65530);
            IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
            localIP = endPoint.Address.ToString();
        }

        // khởi tạo sender
        public void InitSender(string hostIP)
        {
            if (hostIP.Equals("broadcast"))
            {
                sendingClient = new UdpClient(broadCastAddress, port);
                sendingClient.EnableBroadcast = true;
            } else
            {
                sendingClient = new UdpClient(hostIP, port);
                sendingClient.EnableBroadcast = true;
            }
        }

        // khởi tạo receiver
        public void InitReceiver()
        {
            receivingClient = new UdpClient(port);

            ThreadStart start = new ThreadStart(Receiver);
            receivingThread = new Thread(start);
            receivingThread.IsBackground = true;
            receivingThread.Start();
        }

        private void Receiver()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);

            while (true)
            {
                byte[] data = receivingClient.Receive(ref endPoint);
                string message = Encoding.ASCII.GetString(data);

                // get host player name: "get:hostname"
                string[] mess_code = message.Split(':');

                // giao tiếp giữa 2 client
                switch(mess_code[0])
                {
                    case "set":
                        switch(mess_code[1])
                        {
                            case "hostname":
                                FormLogin.host_name = mess_code[2];
                                break;
                            case "joinname":
                                FormLogin.join_name = mess_code[2];
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

        public void SendData(string mess)
        {
            byte[] send_test = Encoding.ASCII.GetBytes(mess);
            sendingClient.Send(send_test, send_test.Length);
            //sendingClient.Close();
        }
    }
}
