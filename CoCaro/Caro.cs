using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CoCaro.Properties;

namespace CoCaro
{
    class Caro
    {
        List<int> ngang = new List<int>();

        private Banco _BanCo;
        public  Caro ()
        {
            _BanCo = new Banco(20, 20);
        }
        public void vebanco(Graphics g)
        {
            _BanCo.VeBanCo(g);
        }

        public void DanhCo(int x, int y, int player, Graphics g)
        {
            Bitmap player_x = Resources.x;
            Bitmap player_o = Resources.o;

            int new_x = 0;
            int new_y = 0;
            for (int i = 1; i <= 9; i++)
            {
                for (int j = 1; j <= 9; j++)
                {
                    if ((x <= OCo._ChieuRong * i) && (y <= OCo._ChieuCao * j))
                    {
                        new_x = OCo._ChieuRong * (i - 1);
                        new_y = OCo._ChieuCao * (j - 1);
                        goto Next;
                    }
                }
            }

            Next:
            Rectangle rect = new Rectangle
            {
                Height = OCo._ChieuCao,
                Width = OCo._ChieuRong,
                X = new_x,
                Y = new_y,
            };
            if (player == 1)
            {
                g.DrawImage(player_x, rect);
            }
            else if (player == 2)
            {
                g.DrawImage(player_o, rect);
            }

            int vi_tri = (new_x + new_y * 9 + 60) / 60;

            ngang.Add(vi_tri);
            MessageBox.Show(Convert.ToString(vi_tri));
        }

        public void ThangNgang()
        {
            

            
        }
    }
}
