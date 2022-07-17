using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using TanksServer.Model.Server;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using TanksServer.Control;

namespace TanksServer
{
    public partial class ServerForm : Form
    {      
        internal TankServer server;



        //
        //  Timers
        //
        private System.Timers.Timer ListBoxUpdater;

        public ServerForm()
        {
            InitializeComponent();
            this.FormClosing += CloseServer;
            server = new TankServer();


            if (!Directory.Exists("UserStats"))
            {
                Directory.CreateDirectory("UserStats");
            }
            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }
           

         

            try
            {
                Thread thread = new Thread(new ThreadStart(server.Listen));
                thread.Start();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void CloseServer(object sender, FormClosingEventArgs e)
        {
            server.CloseServer();
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
