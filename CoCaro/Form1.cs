using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CoCaro.Properties;

namespace CoCaro
{
    public partial class Form1 : Form
    {
        private Caro caro;
        private Graphics grs;

        // player turn
        int turn = 0;
        int player = 0;
        public List<int> KeHuyDiet = new List<int>();

        public Form1()
        {
            InitializeComponent();

            // khởi tạo bàn cờ caro 9 ô (chiều rộng x chiều dài)
            caro = new Caro(9, 9);

            // vẽ bàn cờ
            grs = panelBanCo.CreateGraphics();
        }

        private void fIleToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }


        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "  The goal of the game is to order unbroken \n row of five signs horizontally, vertically, or \n  diagonally. You play by clicking with mouse \n on any empty field of the board. Then it is \n turn of Player 2 . And then it is your turn \n again and so on. ";
        }


        private void pnlBanCo_Paint(object sender, PaintEventArgs e)
        {
            caro.vebanco(grs);
            caro.check();
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
                    }
                }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
