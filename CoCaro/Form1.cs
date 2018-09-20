﻿using System;
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
        }

        private void panelBanCo_MouseClick(object sender, MouseEventArgs e)
        {
            Point point = e.Location;

            // tính turn của người chơi theo chẵn lẻ
            if (turn % 2 == 0) // chẵn turn người chơi 1
            {
                player = 1;
                turn++;
            }
            else // lẻ turn người chơi 2
            {
                player = 2;
                turn++;
            }

            // đánh cờ theo người chơi
            int vi_tri = caro.DanhCo(point.X, point.Y, player, grs);
            bool win = caro.CheckWin(player, vi_tri);
            if (win)
            {
                MessageBox.Show("Player " + player + " won");
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
