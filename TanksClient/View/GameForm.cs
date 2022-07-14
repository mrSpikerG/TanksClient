using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
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
        public static Random Rand = new Random();




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


            //
            //  Form Settings
            //
            this.DoubleBuffered = true;
            this.ClientSize = new Size(1000, 500);
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.BackgroundImage = new Bitmap("GameBG.png");

            //
            //  Init
            //
            CurrentUser = new UserInfo(name, "topPassword");
            Tanks.Clear();
            Tanks.Add(new Tank(new Point(Rand.Next(50, 900), Rand.Next(50, 400)), name, strength: MainMenuForm.Damage, tankSpeed: MainMenuForm.Speed, HealthPoint: MainMenuForm.Health, speed: 5));

            //
            //  Update
            //
            System.Timers.Timer BulletsUpdater = new System.Timers.Timer();
            BulletsUpdater.Elapsed += TimerControl.UpdateBullets;
            BulletsUpdater.Interval = 200;
            BulletsUpdater.Start();

            System.Timers.Timer DeathChecker = new System.Timers.Timer();
            DeathChecker.Elapsed += TimerControl.DeathCheck;
            DeathChecker.Interval = 300;
            DeathChecker.Start();

            System.Timers.Timer NetworkUpdater = new System.Timers.Timer();
            NetworkUpdater.Elapsed += TimerControl.NetworkUpdate;
            NetworkUpdater.Interval = 300;
            NetworkUpdater.Start();
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
