﻿using System.Drawing;

namespace CoCaro
{
    class Banco
    {
        private int _SoDong;
        private int _SoCot;

        public Banco(int Socot, int Sodong)
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
        
    }
}
