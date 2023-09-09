using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace _2DShooterGame
{
    public partial class Form1 : Form
    {
        WindowsMediaPlayer gameMedia;
        WindowsMediaPlayer shootingMedia;
        WindowsMediaPlayer explosion;
        
        PictureBox[] stars;
        int backgroundSpeed;
        int playerSpeed;

        PictureBox[] ammo;
        int AmmoSpeed;

        PictureBox[] enemyAmmo;
        int EnemyAmmoSpeed;

        PictureBox[] enemies;
        int enemySpeed;

        Random random;


        int score;
        int level;
        int difficulty;
        bool pause;
        bool gameIsOver;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pause = false;
            gameIsOver = false;
            score = 0;
            level = 1;
            difficulty = 9;
            
            backgroundSpeed = 4;
            playerSpeed = 4;
            enemySpeed = 4;
            AmmoSpeed = 20;
            EnemyAmmoSpeed = 4;

            ammo = new PictureBox[3];

            //Load images
            Image ammopic = Image.FromFile(@"Game_Assets\munition.png");
            Image enemy1 = Image.FromFile(@"Game_Assets\E1.png");
            Image enemy2 = Image.FromFile(@"Game_Assets\E2.png");
            Image enemy3 = Image.FromFile(@"Game_Assets\E3.png");
            Image boss1 = Image.FromFile(@"Game_Assets\Boss1.png");
            Image boss2 = Image.FromFile(@"Game_Assets\Boss2.png");

            enemies = new PictureBox[10];

            //Initialize EnemiesPictureBoxes
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i] = new PictureBox();
                enemies[i].Size = new Size(40, 40);
                enemies[i].SizeMode = PictureBoxSizeMode.Zoom;
                enemies[i].BorderStyle = BorderStyle.None;
                enemies[i].Visible = false;
                this.Controls.Add(enemies[i]);
                enemies[i].Location = new Point((i + 1) * 50, -50);
            }

            enemies[0].Image = boss1;
            enemies[1].Image = enemy1;
            enemies[2].Image = enemy2;
            enemies[3].Image = enemy1;
            enemies[4].Image = enemy3;
            enemies[5].Image = enemy2;
            enemies[6].Image = enemy2;
            enemies[7].Image = enemy3;
            enemies[8].Image = enemy3;
            enemies[9].Image = boss2;


            for (int i = 0; i < ammo.Length; i++)
            {
                ammo[i] = new PictureBox();
                ammo[i].Size = new Size(8, 8);
                ammo[i].Image = ammopic;
                ammo[i].SizeMode = PictureBoxSizeMode.Zoom;
                ammo[i].BorderStyle = BorderStyle.None;
                this.Controls.Add(ammo[i]);
            }

            //Create WMP
            gameMedia = new WindowsMediaPlayer();
            shootingMedia = new WindowsMediaPlayer();
            explosion = new WindowsMediaPlayer();

            //Load all songs
            gameMedia.URL = "Game_Assets\\BgMusic.mp3";
            shootingMedia.URL = "Game_Assets\\laserblast2.mp3";
            explosion.URL = "Game_Assets\\explosion.mp3";

            //Setup Songs settings
            gameMedia.settings.setMode("loop", true);
            gameMedia.settings.volume = 12;
            shootingMedia.settings.volume = 1;
            explosion.settings.volume = 0;

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

            //Enemies Ammo
            enemyAmmo = new PictureBox[10];
            
            for (int i = 0; i < enemyAmmo.Length;i++)
            {
                enemyAmmo[i] = new PictureBox();
                enemyAmmo[i].Size = new Size(2, 25);
                enemyAmmo[i].Visible = false;
                enemyAmmo[i].BackColor = Color.Yellow;
                int x = random.Next(0, 10);
                enemyAmmo[i].Location = new Point(enemies[x].Location.X, enemies[x].Location.Y - 20);
                this.Controls.Add(enemyAmmo[i]);
            }

            gameMedia.controls.play();
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
            if (!pause)
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
            
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            RightMoveTimer.Stop();
            LeftMoveTimer.Stop();
            UpMoveTimer.Stop();
            DownMoveTimer.Stop();

            if (e.KeyCode == Keys.Space)
            {
                if (pause)
                {
                    StartTimers();
                    label3.Visible = false;
                    gameMedia.controls.play();
                    pause = false;
                }
                else 
                {
                    label3.Location = new Point(this.Width/2 -120, 150);
                    label3.Text = "PAUSED";
                    label3.Visible = true;
                    gameMedia.controls.pause();
                    StopTimers();
                    pause = true;
                }
            }
        }

        private void MoveAmmoTimer_Tick(object sender, EventArgs e)
        {
            shootingMedia.controls.play();
            for (int i = 0; i < ammo.Length; i++)
            {
                if (ammo[i].Top > 0)
                {
                    ammo[i].Visible = true;
                    ammo[i].Top -= AmmoSpeed;

                    Collision();
                }
                else 
                {
                    ammo[i].Visible = false;
                    ammo[i].Location = new Point(Player.Location.X + 20, Player.Location.Y - i * 30);
                }
            }
        }

        private void MoveEnemiesTimer_Tick(object sender, EventArgs e)
        {
            MoveEnemies(enemies, enemySpeed);
        }

        private void MoveEnemies(PictureBox[] array, int speed)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i].Visible = true;
                array[i].Top += speed;

                if (array[i].Top > this.Height)
                {
                    array[i].Location = new Point((i + 1) * 50, -200);
                }
            }
            

        }

        private void Collision()
        {
            for (int i = 0; i < enemies.Length; i++) 
            {
                if (ammo[0].Bounds.IntersectsWith(enemies[i].Bounds) || 
                    ammo[1].Bounds.IntersectsWith(enemies[i].Bounds) ||
                    ammo[2].Bounds.IntersectsWith(enemies[i].Bounds))
                {
                    explosion.settings.volume = 10;
                    explosion.controls.play();

                    score += 1;
                    scorelabel.Text = (score < 10) ? "SCORE: 0" + score.ToString() : "SCORE: " + score.ToString();
                    
                    if (score % 10 == 0)
                    {
                        level += 1;
                        levellabel.Text = (level < 10) ? "LEVEL: 0" + level.ToString() : "LEVEL: " + level.ToString();

                        if (enemySpeed <= 10 && EnemyAmmoSpeed <= 10 && difficulty >= 0)
                        {
                            difficulty--;
                            enemySpeed++;
                            EnemyAmmoSpeed++;
                        }
                        if (level == 10)
                        {
                            GameOver("WELL DONE");
                        }
                    }
                        
                    enemies[i].Location = new Point((i + 1) * 50, -100);
                }

                if (Player.Bounds.IntersectsWith(enemies[i].Bounds))
                {
                    explosion.settings.volume = 30;
                    explosion.controls.play();
                    Player.Visible = false;
                    GameOver("Game Over");
                }
            }
        }

        private void GameOver(String str)
        {
            label3.Text = str;
            label3.Location = new Point(170, 110);
            label3.Visible = true;
            ReplayBtn.Visible = true;
            ExitBtn.Visible = true;
            
            gameMedia.controls.stop();
            StopTimers();

        }

        //Stop Timers
        private void StopTimers() 
        {
            MoveBgTimer.Stop();
            MoveEnemiesTimer.Stop();
            MoveAmmoTimer.Stop();
            EnemyAmmoTimer.Stop();
        }

        //Start Timers
        private void StartTimers()
        {
            MoveBgTimer.Start();
            MoveEnemiesTimer.Start();
            MoveAmmoTimer.Start();
            EnemyAmmoTimer.Start();
        }

        private void EnemyAmmoTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < (enemyAmmo.Length - difficulty); i++)
            {
                if (enemyAmmo[i].Top < this.Height)
                {
                    enemyAmmo[i].Visible = true;
                    enemyAmmo[i].Top += EnemyAmmoSpeed;

                    CollisionWithEnemyAmmo();
                }
                else
                {
                    enemyAmmo[i].Visible = false;
                    int x = random.Next(0, 10);
                    enemyAmmo[i].Location = new Point(enemies[x].Location.X + 20, enemies[x].Location.Y + 30);
                }
            }
        }

        private void CollisionWithEnemyAmmo()
        {
            for (int i = 0; i < enemyAmmo.Length; i++)
            {
                if (enemyAmmo[i].Bounds.IntersectsWith(Player.Bounds))
                {
                    enemyAmmo[i].Visible = false;
                    explosion.settings.volume = 30;
                    explosion.controls.play();
                    Player.Visible = false;
                    GameOver("Game Over");
                }
            }
            
        }

        

        private void ReplayBtn_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            InitializeComponent();
            Form1_Load(e, e);
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }
    }
}