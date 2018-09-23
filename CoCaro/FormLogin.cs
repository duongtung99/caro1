using System;
using System.Windows.Forms;

namespace CoCaro
{
    public partial class FormLogin : Form
    {
        // init user_name
        public static string host_name = "";
        public static string join_name = "";

        public static int player;

        public FormLogin()
        {
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            // init ip lan
            LAN.GetLocalIp();

            // show local ip
            this.label4.Text = LAN.localIP;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // lưu giá trị người dùng nhập vào 
            join_name = "";
            host_name = textBox1.Text;
            player = 1;

            // init wait for client
            LAN.InitWaitForClient();

            // hiện cửa sổ form1
            Form1 form = new Form1();
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // lưu giá trị người dùng nhập vào
            host_name = "";
            join_name = textBox1.Text;
            player = 2;

            // lấy ip host
            string hostIP = textBox2.Text;

            // chào host và đưa host local ip
            LAN.InitHelloHost(hostIP);

            // init receiver
            LAN.InitReceiver(hostIP);

            // hiện cửa sổ form1
            Form1 form = new Form1();
            form.ShowDialog();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
