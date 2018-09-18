using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCaro
{
    class Banco
    {
        private int _SoDong;
        private int _SoCot;

        public Banco()
        {
            _SoCot = 0;
            _SoDong = 0;
        }
        public Banco(int Sodong, int Socot)
        {
            _SoDong = Sodong;
            _SoCot = Socot;
        }

        public void VeBanCo(Graphics g)
        {
            for (int i = 0; i <= _SoCot; i++)
            {
                g.DrawLine(Program.pen, i * OCo._ChieuRong, 0, i * OCo._ChieuRong, _SoDong * OCo._ChieuCao);
            }
            for (int j = 0; j <= _SoDong ; j++)
            {
                g.DrawLine(Program.pen, 0, j * OCo._ChieuCao, _SoCot * OCo._ChieuRong, j * OCo._ChieuCao );
            }
        }

        public void play(Graphics g)
        {

        }
    }
}
