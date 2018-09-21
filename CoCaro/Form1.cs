using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace CoCaro
{
    public partial class Form1 : Form
    {
        private Caro caro;
        private Graphics grs;

        // xac dinh so dong so cot
        private int soDong = 9;
        private int soCot = 9;

        // player turn
        int turn = 0;
        int player = 0;
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
            Point point = e.Location;
            if (turn % 2 == 0) //if turn is even
            {
                player = 1;
                int vi_tri = caro.DanhCo(point.X, point.Y, player, grs);
                if (vi_tri != 0) {
                    bool win = caro.CheckWin(player, vi_tri);
                    KeHuyDiet.Add(vi_tri);
                    turn++;
                    if (win)
                    {
                        MessageBox.Show("Player " + player + " won");
                        caro.NewGame(grs);
                        caro.vebanco(grs);
                        caro.check(soDong, soCot);
                    }
                }
            }
            else //otherwise its odd
            {
                player = 2;

                int vi_tri = caro.DanhCo(point.X, point.Y, player, grs);
                if (vi_tri != 0)
                {
                    bool win = caro.CheckWin(player, vi_tri);
                    KeHuyDiet.Add(vi_tri);
                    turn++;
                    if (win)
                    {
                        MessageBox.Show("Player " + player + " won");
                        caro.NewGame(grs);
                        caro.vebanco(grs);
                        caro.check(soDong, soCot);
                    }
                }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            caro.NewGame(grs);
            caro.vebanco(grs);
            caro.check(soDong, soCot);
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            checkPlayerNameThread.Abort();
        }
    }
}
