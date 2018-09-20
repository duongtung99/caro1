﻿using System;
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
        List<int> playerX = new List<int>();
        List<int> playerO = new List<int>();

        List<int> checkPlayer = new List<int>();

        private int chieu_rong;
        private int chieu_cao;

        private Banco _BanCo;
        public  Caro (int chieu_rong, int chieu_cao)
        {
            _BanCo = new Banco(chieu_rong, chieu_cao);
            this.chieu_rong = chieu_rong;
            this.chieu_cao = chieu_cao;
        }
        public void vebanco(Graphics g)
        {
            _BanCo.VeBanCo(g);
        }

        // thuật toán đánh cờ
        public int DanhCo(int x, int y, int player, Graphics g)
        {
            // lấy ảnh từ Resources
            Bitmap player_x = Resources.x;
            Bitmap player_o = Resources.o;

            // tính toán vị trí đặt ảnh theo vị trí click chuột
            int new_x = 0;
            int new_y = 0;
            for (int i = 1; i <= chieu_rong; i++)
            {
                for (int j = 1; j <= chieu_cao; j++)
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

            // lưu vị trí theo thứ tự 1-> 81 vào List
            int vi_tri = (new_x + new_y * chieu_rong + 60) / 60;

            // lưu vị trí người đánh vào list của ng đó
            if (player == 1)
            {
                playerX.Add(vi_tri);
            }
            else if (player == 2)
            {
                playerO.Add(vi_tri);
            }
            //checkPlayer.Add(vi_tri);
            //MessageBox.Show(Convert.ToString(vi_tri));
            return vi_tri;
        }

        // check thắng theo hàng
        public bool CheckWin(int player, int vi_tri)
        {
            if (player == 1)
            {
                checkPlayer = playerX;
            } else if (player == 2)
            {
                checkPlayer = playerO;
            }

            for (int i = -4; i < 1; i++)
            {
                // check hàng checkPlayer
                if (checkPlayer.Contains(vi_tri + i) &&
                checkPlayer.Contains(vi_tri + i + 1) &&
                checkPlayer.Contains(vi_tri + i + 2) &&
                checkPlayer.Contains(vi_tri + i + 3) &&
                checkPlayer.Contains(vi_tri + i + 4))
                {
                    return true;
                }

                // check hàng chéo phải sang trái
                if (checkPlayer.Contains(vi_tri + i * 8) &&
                checkPlayer.Contains(vi_tri + (i + 1) * 8) &&
                checkPlayer.Contains(vi_tri + (i + 2) * 8) &&
                checkPlayer.Contains(vi_tri + (i + 3) * 8) &&
                checkPlayer.Contains(vi_tri + (i + 4) * 8))
                {
                    return true;
                }

                // check hàng dọc
                if (checkPlayer.Contains(vi_tri + i * 9) &&
                checkPlayer.Contains(vi_tri + (i + 1) * 9) &&
                checkPlayer.Contains(vi_tri + (i + 2) * 9) &&
                checkPlayer.Contains(vi_tri + (i + 3) * 9) &&
                checkPlayer.Contains(vi_tri + (i + 4) * 9))
                {
                    return true;
                }

                // check hàng chéo trái sang phải
                if (checkPlayer.Contains(vi_tri + i * 10) &&
                checkPlayer.Contains(vi_tri + (i + 1) * 10) &&
                checkPlayer.Contains(vi_tri + (i + 2) * 10) &&
                checkPlayer.Contains(vi_tri + (i + 3) * 10) &&
                checkPlayer.Contains(vi_tri + (i + 4) * 10))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
