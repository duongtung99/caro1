using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoCaro
{
    public partial class Form1 : Form
    {
        private Caro caro;
        private Graphics grs;
        public Form1()
        {
            InitializeComponent();
            caro = new Caro();
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

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
