using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CoCaro
{
    class LAN
    {
        // init local ip
        public static string localIP;

        // init listener
        public static string listenIP { get; set; }
        public TcpListener listener = null;

        // init client
        TcpClient client = null;

        // init worker
        public BackgroundWorker workerListener = null;
        public BackgroundWorker workerClient = null;

        public LAN()
        {
            InitWorker();
            GetLocalIp();
        }

        public void InitWorker()
        {
            // create object worker
            workerListener = new BackgroundWorker
            {
                WorkerSupportsCancellation = true
            };

            workerClient = new BackgroundWorker
            {
                WorkerSupportsCancellation = true
            };

            // add work
            workerListener.DoWork += WorkerHost;
            workerClient.DoWork += WorkerClient;

            // get handle and set nox title
            //worker1.RunWorkerAsync();
        }

        public static void GetLocalIp()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0);
            socket.Connect("8.8.8.8", 65530);
            IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
            localIP = endPoint.Address.ToString();
        }

        private void WorkerHost(object sender, DoWorkEventArgs e)
        {
            // đặt ip port
            int port = 1234;
            IPAddress localAddress = IPAddress.Parse(localIP);

            // khởi động Host
            listener = new TcpListener(localAddress, port);
            listener.Start();

            // buff đọc ghi data
            Byte[] bytes = new Byte[256];
            String data = null;

            while(true)
            {   
                // đợi client kết nối
                TcpClient client = listener.AcceptTcpClient();

                data = null;

                // tạo stream obj cho việc đọc ghi
                NetworkStream stream = client.GetStream();

                int i;
                while ((i = stream.Read(bytes, 0, bytes.Length))!= 0) {
                    // chuyển byte thành ASCII string
                    data = Encoding.ASCII.GetString(bytes, 0, i);

                    // do something here
                }
            }
        }

        private void WorkerClient(object sender, DoWorkEventArgs e)
        {
            // đặt ip port
            int port = 1234;
            client = new TcpClient(listenIP, port);
        }

        public void SendData(string mess)
        {

        }
    }
}
