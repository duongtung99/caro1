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
        private static UdpClient receivingClient = null;
        private static UdpClient waitForClient = null;

        // init sender
        private static UdpClient sendingClient = null;
        private static UdpClient helloHost = null;

        // tạo thread riêng cho việc nhận data
        private static Thread receivingThread = null;
        private static Thread waitClientThread = null;

        // lưu giá trị để shutdown thread khi cần
        private static bool waitClientStop = false;
        private static bool receiverStop = false;


        // lấy ip trong mạng lan
        public static void GetLocalIp()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0);
            socket.Connect("8.8.8.8", 65530);
            IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
            localIP = endPoint.Address.ToString();
        }

        // chào host
        public static void InitHelloHost(string sendtoIP)
        {
            helloHost = new UdpClient(sendtoIP, hello_port);
            helloHost.EnableBroadcast = true;

            byte[] send_test = Encoding.ASCII.GetBytes("hello:" + localIP);
            helloHost.Send(send_test, send_test.Length);

            helloHost.Close();
        }

        // đợi kết nối từ client
        public static void InitWaitForClient()
        {
            if (waitForClient == null)
            {
                waitForClient = new UdpClient(hello_port);

                if (waitClientThread == null)
                {
                    ThreadStart start = new ThreadStart(WaitForClient);
                    waitClientThread = new Thread(start);
                    waitClientThread.IsBackground = true;
                    waitClientThread.Start();
                } else
                {
                    // dừng waitClientThread nếu đang chạy
                    waitClientStop = true;

                    // nghỉ giải lao 0.1s
                    Thread.Sleep(100);

                    // chạy thread mới
                    ThreadStart start = new ThreadStart(WaitForClient);
                    waitClientThread = new Thread(start);
                    waitClientThread.IsBackground = true;
                    waitClientThread.Start();
                }
            }
        }

        public static void WaitForClient()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, hello_port);

            while (!waitClientStop)
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
                    break;
                }
            }
        }

        // khởi tạo sender
        public static void InitSender(string sendtoIP)
        {
            if (sendingClient == null)
            {
                sendingClient = new UdpClient(sendtoIP, port);
                sendingClient.EnableBroadcast = true;
            } else
            {
                // đóng kết nối hiện tại
                sendingClient.Close();

                // mở kết nối mới
                sendingClient = new UdpClient(sendtoIP, port);
                sendingClient.EnableBroadcast = true;
            }
        }

        // khởi tạo receiver
        public static void InitReceiver(string listentoIP)
        {
            if (receivingClient == null)
            {
                receivingClient = new UdpClient(port);
                receivingIP = listentoIP;
                if (receivingThread == null)
                {
                    ThreadStart start = new ThreadStart(Receiver);
                    receivingThread = new Thread(start);
                    receivingThread.IsBackground = true;
                    receivingThread.Start();
                }
                else
                {
                    // dừng waitClientThread nếu đang chạy
                    receiverStop = true;

                    // nghỉ giải lao 0.1s
                    Thread.Sleep(100);

                    // chạy thread mới
                    ThreadStart start = new ThreadStart(Receiver);
                    receivingThread = new Thread(start);
                    receivingThread.IsBackground = true;
                    receivingThread.Start();
                }
            }
        }

        private static void Receiver()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(receivingIP), port);

            while (!receiverStop)
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
                            case "win":
                                MessageBox.Show("Player " + mess_code[2] + " won");

                                Caro caro = new Caro(Form1.soDong, Form1.soCot);
                                caro.NewGame(Form1.grs);
                                caro.vebanco(Form1.grs);
                                caro.check(Form1.soDong, Form1.soCot);
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

        public static void SendData(string mess)
        {
            byte[] send_test = Encoding.ASCII.GetBytes(mess);
            sendingClient.Send(send_test, send_test.Length);
        }

        public static void CloseConnect()
        {
            waitClientStop = true;
            Thread.Sleep(100);
            receiverStop = true;
            Thread.Sleep(100);
            receivingClient.Close();
            Thread.Sleep(100);
            sendingClient.Close();
        }
    }
}
