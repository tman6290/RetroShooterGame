﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DShooterGame
{
    public partial class Form1 : Form
    {
        PictureBox[] stars;
        int backgroundSpeed;
        int playerSpeed;

        PictureBox[] ammo;
        int AmmoSpeed;

        Random random;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            backgroundSpeed = 4;
            playerSpeed = 4;

            AmmoSpeed = 20;
            ammo = new PictureBox[3];

            //Load images
            Image ammopic = Image.FromFile(@"Game_Assets\munition.png");

            for (int i = 0; i < ammo.Length; i++)
            {
                ammo[i] = new PictureBox();
                ammo[i].Size = new Size(8, 8);
                ammo[i].Image = ammopic;
                ammo[i].SizeMode = PictureBoxSizeMode.Zoom;
                ammo[i].BorderStyle = BorderStyle.None;
                this.Controls.Add(ammo[i]);
            }


            stars = new PictureBox[15];
            random = new Random();

            for (int i = 0; i < stars.Length; i++)
            {
                stars[i] = new PictureBox();
                stars[i].BorderStyle = BorderStyle.None;
                stars[i].Location = new Point(random.Next(20, 580), random.Next(-10, 400));
                if (i % 2 == 1)
                {
                    stars[i].Size = new Size(2, 2);
                    stars[i].BackColor = Color.Wheat;
                }
                else
                {
                    stars[i].Size = new Size(3, 3);
                    stars[i].BackColor = Color.DarkGray;
                }

                this.Controls.Add(stars[i]);

            }
        }

        private void MoveBgTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < stars.Length / 2; i++)
            {
                stars[i].Top += backgroundSpeed;
                if (stars[i].Top >= this.Height)
                {
                    stars[i].Top = -stars[i].Height;
                }
            }

            for (int i = stars.Length / 2; i < stars.Length; i++)
            {
                stars[i].Top += backgroundSpeed - 2;

                if (stars[i].Top >= this.Height)
                {
                    stars[i].Top = -stars[i].Height;
                }
            }
        }

        private void LeftMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Left > 10)
            {
                Player.Left -= playerSpeed;
            }
        }

        private void RightMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Right < 580)
            {
                Player.Left += playerSpeed;
            }
        }

        private void UpMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Top > 10)
            {
                Player.Top -= playerSpeed;
            }
        }

        private void DownMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Top < 400)
            {
                Player.Top += playerSpeed;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                RightMoveTimer.Start();
            }
            if (e.KeyCode == Keys.Left)
            {
                LeftMoveTimer.Start();
            }
            if (e.KeyCode == Keys.Up)
            {
                UpMoveTimer.Start();
            }
            if (e.KeyCode == Keys.Down)
            {
                DownMoveTimer.Start();
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            RightMoveTimer.Stop();
            LeftMoveTimer.Stop();
            UpMoveTimer.Stop();
            DownMoveTimer.Stop();
        }

        private void MoveAmmoTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < ammo.Length; i++)
            {
                if (ammo[i].Top > 0)
                {
                    ammo[i].Visible = true;
                    ammo[i].Top -= AmmoSpeed;
                }
                else 
                {
                    ammo[i].Visible = false;
                    ammo[i].Location = new Point(Player.Location.X + 20, Player.Location.Y - i * 30);
                }
            }
        }
    }
}