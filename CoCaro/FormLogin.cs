using System;
using System.Windows.Forms;

namespace CoCaro
{
    public partial class FormLogin : Form
    {
        // init user_name
        public static string host_name = "";
        public static string join_name = "";

        // init LAN
        LAN lan = new LAN();

        public FormLogin()
        {
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            // show local ip
            this.label4.Text = LAN.localIP;

            // init receiver
            lan.InitReceiver();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // lưu giá trị người dùng nhập vào 
            join_name = "";
            host_name = textBox1.Text;

            // init sender
            lan.InitSender("broadcast");

            // hiện cửa sổ form1
            Form1 form = new Form1();
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // lưu giá trị người dùng nhập vào
            host_name = "";
            join_name = textBox1.Text;

            // chạy listener
            string hostIP = textBox2.Text;

            // init sender
            lan.InitSender(hostIP);

            // hỏi host thông tin người chơi host
            lan.SendData("get:hostname");
            lan.SendData("set:joinname:" + join_name);

            // hiện cửa sổ form1
            Form1 form = new Form1();
            form.ShowDialog();
        }
    }
}
