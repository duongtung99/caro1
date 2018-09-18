using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCaro
{
    class Caro
    {
        private Banco _BanCo;
        public  Caro ()
        {
            _BanCo = new Banco(20, 20);
        }
        public void vebanco(Graphics g)
        {
            _BanCo.VeBanCo(g);
        }
    }
}
