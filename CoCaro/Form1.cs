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
        private int soDong = 30;
        private int soCot = 30;

        // player turn
        public static int turn = 0;
        //int player = 0;
        public List<int> KeHuyDiet = new List<int>();

        // check player name join
        Thread checkPlayerNameThread = null;

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
            while(true)
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
                Point point = e.Location;
                int vi_tri = Caro.DanhCo(point.X, point.Y, FormLogin.player, grs);
                LAN.SendData("set:play:" + FormLogin.player + ":" + point.X + ":" + point.Y);
                //MessageBox.Show("next turn: " + Convert.ToString(turn));

                if (vi_tri != 0)
                {
                    bool win = caro.CheckWin(FormLogin.player, vi_tri);
                    KeHuyDiet.Add(vi_tri);
                    turn++;

                    if (win)
                    {
                        MessageBox.Show("Player " + FormLogin.player + " won");
                        caro.NewGame(grs);
                        caro.vebanco(grs);
                        caro.check(soDong, soCot);
                    }
                }
            } else if (turn % 2 != 0 && FormLogin.player == 2)
            {
                Point point = e.Location;
                int vi_tri = Caro.DanhCo(point.X, point.Y, FormLogin.player, grs);
                LAN.SendData("set:play:" + FormLogin.player + ":" + point.X + ":" + point.Y);

                if (vi_tri != 0)
                {
                    bool win = caro.CheckWin(FormLogin.player, vi_tri);
                    KeHuyDiet.Add(vi_tri);
                    turn++;
                    //MessageBox.Show("next turn: " + Convert.ToString(turn));

                    if (win)
                    {
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
            checkPlayerNameThread.Abort();
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
