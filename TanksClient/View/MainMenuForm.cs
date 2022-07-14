using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using TanksClient.Control;
using TanksClient.Model;

namespace TanksClient.View
{
    public partial class MainMenuForm : Form
    {

        //
        //  Networking
        //
        public static TcpClient client = new TcpClient();
        public static NetworkStream stream = null;
        public const int PORT = 8008;
        public const string HOST = "127.0.0.1";

        // 
        //  Characters
        //
        public static int Money = 100;
        public static int Speed = 1;
        public static int Damage = 1;
        public static int Health = 1;
        public static Statistic stats;
        public GameForm form;
        public SettingsShop settings;
        public static bool Debug = false;

        public MainMenuForm()
        {
            InitializeComponent();



            //
            //  Networking
            //
            client.Connect(HOST, PORT);
            stream = client.GetStream();

            //    byte[] buffer = Encoding.Unicode.GetBytes($"cmd getStats {Name}");
            //    stream.Write(buffer, 0, buffer.Length);



            //
            //  Settings
            //
            this.BackgroundImage = new Bitmap("MainMenuBG.png");
            this.ClientSize = new Size(688, 520);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.FormClosing += CloseNetwork;

            //
            //  Textbox
            //
            this.textBox1.Location = new Point(200, 200);
            this.textBox1.Size = new Size(267, 84);

            //
            //  Play Button
            //
            this.button1.BackgroundImage = new Bitmap("play1.png");
            this.button1.MouseHover += AnimatePlayButton;
            this.button1.MouseLeave += AnimatePlayButtonLeave;
            this.button1.Size = new Size(267, 84);
            this.button1.FlatStyle = FlatStyle.Popup;
            this.button1.BackColor = Color.Transparent;
            this.button1.Location = new Point(200, 230);


            //
            //  Settings
            //
            this.button2.BackgroundImage = new Bitmap("settings1.png");
            this.button2.MouseHover += AnimateSettingButton;
            this.button2.MouseLeave += AnimateSettingButtonLeave;
            this.button2.Size = new Size(188, 59);
            this.button2.FlatStyle = FlatStyle.Popup;
            this.button2.BackColor = Color.Transparent;
            this.button2.Location = new Point(247, 350);

        }

        private void CloseNetwork(object sender, FormClosingEventArgs e)
        {
            client.Close();
            stream.Close();
            //  TimerControl.receiveMsgThread.Abort();
        }

        private void AnimateSettingButtonLeave(object sender, EventArgs e)
        {
            this.button2.BackgroundImage = new Bitmap("settings1.png");
        }

        private void AnimatePlayButtonLeave(object sender, EventArgs e)
        {
            this.button1.BackgroundImage = new Bitmap("play1.png");
        }

        private void AnimateSettingButton(object sender, EventArgs e)
        {
            this.button2.BackgroundImage = new Bitmap("settings2.png");
        }

        private void AnimatePlayButton(object sender, EventArgs e)
        {
            this.button1.BackgroundImage = new Bitmap("play2.png");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text != String.Empty)
            {
                form = new GameForm(this.textBox1.Text);
                form.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            settings = new SettingsShop();
            settings.Show();
        }
    }
}
