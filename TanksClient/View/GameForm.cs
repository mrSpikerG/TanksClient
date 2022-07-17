using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using TanksClient.Control;
using TanksClient.Model;
using TanksClient.View;

namespace TanksClient
{
    public partial class GameForm : Form
    {



        //
        //  Models
        //
        public static List<Tank> Tanks = new List<Tank>();
        public static UserInfo CurrentUser;

        //
        //  Controls
        //
        public ButtonControl BControl = new ButtonControl();
        //        public TimerControl TControl = new TimerControl();

        //
        //  Another Features
        //

        public static bool GameStarted = false;
        public static bool GameEnded = false;
        public static Random Rand = new Random();


        //
        //  Timers
        //
        private System.Timers.Timer NetworkUpdater;
        private System.Timers.Timer BulletsUpdater;
        private System.Timers.Timer DeathChecker;
        private System.Timers.Timer EndGameUpdater;

        public GameForm(string name)
        {
            InitializeComponent();
            //
            //  Events
            //
            this.Paint += DrawTanks;
            this.Paint += DrawBullets;
            this.KeyDown += BControl.MoveTank;
            this.KeyDown += BControl.SendBullet;
            this.FormClosing += CloseTimers;

            //
            //  Form Settings
            //
            this.DoubleBuffered = true;
            this.ClientSize = new Size(1000, 500);
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.BackgroundImage = new Bitmap("GameBG.png");
            this.Text = name;

            //
            //  Init
            //
            CurrentUser = new UserInfo(name, "topPassword");
            Tanks.Clear();
            Tanks.Add(new Tank(new Point(Rand.Next(50, 900), Rand.Next(50, 400)), name, strength: MainMenuForm.Damage, tankSpeed: MainMenuForm.Speed, HealthPoint: MainMenuForm.Health, speed: 5));

            //
            //  Update
            //
            this.BulletsUpdater = new System.Timers.Timer();
            this.BulletsUpdater.Elapsed += TimerControl.UpdateBullets;
            this.BulletsUpdater.Interval = 200;
            this.BulletsUpdater.Start();

            this.DeathChecker = new System.Timers.Timer();
            this.DeathChecker.Elapsed += TimerControl.DeathCheck;
            this.DeathChecker.Interval = 300;
            this.DeathChecker.Start();

            this.NetworkUpdater = new System.Timers.Timer();
            this.NetworkUpdater.Elapsed += TimerControl.NetworkUpdate;
            this.NetworkUpdater.Interval = 300;
            this.NetworkUpdater.Start();

            this.EndGameUpdater = new System.Timers.Timer();
            this.EndGameUpdater.Elapsed += EndGameUpdate;
            this.EndGameUpdater.Interval = 1000;
            this.EndGameUpdater.Start();

        }

        private void EndGameUpdate(object sender, ElapsedEventArgs e)
        {
            if (GameEnded)
            {
                this.DeathChecker.Close();
                this.NetworkUpdater.Close();
                this.BulletsUpdater.Close();
                this.EndGameUpdater.Close();
                this.Close();
                this.Dispose();
            }
        }

        private void CloseTimers(object sender, FormClosingEventArgs e)
        {
            this.DeathChecker.Close();
            this.NetworkUpdater.Close();
            this.BulletsUpdater.Close();
        }



        //
        //  Отрисовка пуль
        //
        private void DrawBullets(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < Tanks.Count; i++)
            {
                if (MainMenuForm.Debug)
                {
                    //  Урон + Коллизии
                    TextRenderer.DrawText(e.Graphics, Tanks[i].Bullet.Strength.ToString(), this.Font, new Point(Tanks[i].Bullet.Coords.X + 3, Tanks[i].Bullet.Coords.Y - 15), Color.Black);
                    e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Red), 2), new Rectangle(Tanks[i].Bullet.Coords, Tanks[i].Bullet.Size));
                }

                //  Подбор нужной текстуры
                string img = $"Bullet{AdditionalControl.CheckForDirection(Tanks[i].Bullet.Direct)}";
                e.Graphics.DrawImage(new Bitmap($"{img}.png"), Tanks[i].Bullet.Coords.X, Tanks[i].Bullet.Coords.Y, Tanks[i].Bullet.Size.Width, Tanks[i].Bullet.Size.Height);

            }
            //  this.Invalidate();
        }


        //
        //  Отрисовка танков
        //
        private void DrawTanks(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < Tanks.Count; i++)
            {
                //  Ник
                TextRenderer.DrawText(e.Graphics, Tanks[i].Nickname, this.Font, new Point(Tanks[i].Coords.X + 5, Tanks[i].Coords.Y - 15), Color.Black);

                if (MainMenuForm.Debug)
                {
                    //  Коллизии
                    e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Red), 2), new Rectangle(Tanks[i].Coords, Tanks[i].Size));
                }

                //  Подбор нужной текстуры
                string img = Tanks[i].isAlive ? $"Tank{AdditionalControl.CheckForDirection(Tanks[i].Direct)}" : "PressF";
                e.Graphics.DrawImage(new Bitmap($"{img}.png"), Tanks[i].Coords.X, Tanks[i].Coords.Y, Tanks[i].Size.Width, Tanks[i].Size.Height);
            }
            this.Invalidate();
        }


        public enum Direction
        {
            NORTH,
            SOUTH,
            EAST,
            WEST

        }
    }
}
