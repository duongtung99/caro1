using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCaro
{
    class OCo
    {
        public const int _ChieuRong = 30;
        public const int _ChieuCao = 30;

        private int _Dong;
        public int Dong
        {
            set { _Dong = value;  }
            get { return _Dong; }
        }
        private int _Cot;
        private int Cot
        {
            set { _Cot = value; }
            get { return _Cot; }
        }
    }
}
