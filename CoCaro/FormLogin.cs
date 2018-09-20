using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace CoCaro
{
    public partial class FormLogin : Form
    {
        public static string user_name = "";


        public FormLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // lưu giá trị người dùng nhập vào 
            user_name = textBox1.Text;

            // ẩn cửa sổ 
            Hide();
            
            // hiện cửa sổ form1
            Form1 form = new Form1();
            //form.ShowDialog();
        }

        
        private string GetLocalIp()
        {
            string localIP;
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0);
            socket.Connect("8.8.8.8", 65530);
            IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
            localIP = endPoint.Address.ToString();
            return localIP;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            this.label4.Text = GetLocalIp();
        }
    }
}
