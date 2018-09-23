using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace CoCaro
{
    public partial class Form1 : Form
    {
        private static Caro caro;
        public static Graphics grs;

        // xac dinh so dong so cot
        public static int soDong = 30;
        public static int soCot = 30;


        // player turn
        public static int turn = 0;
        //int player = 0;
        public List<int> KeHuyDiet = new List<int>();

        // check player name join
        private static Thread checkPlayerNameThread = null;
        private static bool getNameStop = false;

        public Form1()
        {
            InitializeComponent();

            // khởi tạo bàn cờ caro 9 ô (chiều rộng x chiều dài)
            caro = new Caro(soDong, soCot);

            // vẽ bàn cờ
            grs = panelBanCo.CreateGraphics();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "  The goal of the game is to order unbroken \n row of five signs horizontally, vertically, or \n  diagonally. You play by clicking with mouse \n on any empty field of the board. Then it is \n turn of Player 2 . And then it is your turn \n again and so on. ";
            InitNameChecker();
        }


        private void InitNameChecker()
        {
            ThreadStart start = new ThreadStart(GetPlayerName);
            checkPlayerNameThread = new Thread(start);
            checkPlayerNameThread.IsBackground = true;
            checkPlayerNameThread.Start();
        }

        private void GetPlayerName()
        {
            while(!getNameStop)
            {
                // đặt hostname
                Invoke((MethodInvoker)delegate ()
                {
                    label2.Text = FormLogin.host_name;
                });

                // đặt joinname
                Invoke((MethodInvoker)delegate ()
                {
                    label3.Text = FormLogin.join_name;
                });
            }
        }


        private void pnlBanCo_Paint(object sender, PaintEventArgs e)
        {
            caro.vebanco(grs);
            caro.check(soDong,soCot);
        }

        private void panelBanCo_MouseClick(object sender, MouseEventArgs e)
        {
            if (turn % 2 == 0 && FormLogin.player == 1) //if turn is even
            {
                // hiển thị nước đánh
                Point point = e.Location;
                int vi_tri = Caro.DanhCo(point.X, point.Y, FormLogin.player, grs);

                

                // kiểm tra win
                if (vi_tri != 0)
                {
                    // gửi thông tin cho người chơi còn lại biết mày vừa đánh ở đâu
                    LAN.SendData("set:play:" + FormLogin.player + ":" + point.X + ":" + point.Y);

                    bool win = caro.CheckWin(FormLogin.player, vi_tri);
                    KeHuyDiet.Add(vi_tri);
                    turn++;

                    if (win)
                    {
                        // gửi cho thằng chơi cùng biết mày là người chiến thắng
                        LAN.SendData("set:win:" + FormLogin.player);

                        // hiển thị nếu mày là người chiến thắng
                        MessageBox.Show("Player " + FormLogin.player + " won");

                        // tạo game mới
                        caro.NewGame(grs);
                        caro.vebanco(grs);
                        caro.check(soDong, soCot);
                    }
                }
            } else if (turn % 2 != 0 && FormLogin.player == 2)
            {
                Point point = e.Location;
                int vi_tri = Caro.DanhCo(point.X, point.Y, FormLogin.player, grs);
                

                if (vi_tri != 0)
                {
                    LAN.SendData("set:play:" + FormLogin.player + ":" + point.X + ":" + point.Y);

                    bool win = caro.CheckWin(FormLogin.player, vi_tri);
                    KeHuyDiet.Add(vi_tri);
                    turn++;

                    if (win)
                    {
                        // gửi cho thằng chơi cùng biết mày là người chiến thắng
                        LAN.SendData("set:win:" + FormLogin.player);

                        // hiển thị nếu mày là người chiến thắng
                        MessageBox.Show("Player " + FormLogin.player + " won");

                        caro.NewGame(grs);
                        caro.vebanco(grs);
                        caro.check(soDong, soCot);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            caro.NewGame(grs);
            caro.vebanco(grs);
            caro.check(soDong, soCot);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //getNameStop = true;
            //Thread.Sleep(100);
            //LAN.CloseConnect();
        }

        private void Form1_FormClosing(object sender, FormClosedEventArgs e)
        {
            getNameStop = true;
            Thread.Sleep(100);
            LAN.CloseConnect();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult mess = MessageBox.Show("Are you sure that you want to exit this game ? ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (mess == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
