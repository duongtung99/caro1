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
        const int port = 1234;
        const string broadCastAddress = "255.255.255.255";

        // gửi lệnh từ thread này qua thread khác
        delegate void AddMessage(string message);

        // init receiver
        UdpClient receivingClient;

        // init sender
        UdpClient sendingClient;

        // tạo thread riêng cho việc nhận data
        Thread receivingThread;

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
        private void InitSender()
        {
            sendingClient = new UdpClient(broadCastAddress, port);
            sendingClient.EnableBroadcast = true;
        }

        // khởi tạo receiver
        private void InitReceiver()
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
            AddMessage messageDelegate = MessageReceived;

            while (true)
            {
                byte[] data = receivingClient.Receive(ref endPoint);
                string message = Encoding.ASCII.GetString(data);
                myform.Invoke(messageDelegate, message);

            }
        }

        private void MessageReceived(string mess)
        {
            // do something with the incoming mess
            myform. += mess + "\n";
        }

        public void SendData(string mess)
        {

        }
    }
}
