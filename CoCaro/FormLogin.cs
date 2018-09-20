using System;
using System.Windows.Forms;

namespace CoCaro
{
    public partial class FormLogin : Form
    {
        // init user_name
        public static string user_name = "";

        // init LAN
        LAN lan = new LAN();

        public FormLogin()
        {
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            this.label4.Text = LAN.localIP;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // lưu giá trị người dùng nhập vào 
            user_name = textBox1.Text;

            // ẩn cửa sổ 
            Hide();

            // chạy listener
            string listenIP = textBox2.Text;
            LAN.listenIP = listenIP;
            lan.workerListener.RunWorkerAsync();

            // kết nối tới đối thủ
            lan.workerClient.RunWorkerAsync();

            // hiện cửa sổ form1
            Form1 form = new Form1();
            //form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // lưu giá trị người dùng nhập vào 
            user_name = textBox2.Text;
        }
    }
}
