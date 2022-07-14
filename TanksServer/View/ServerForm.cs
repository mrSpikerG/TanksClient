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

namespace TanksServer
{
    public partial class ServerForm : Form
    {      
        internal TankServer server;
        public ServerForm()
        {
            InitializeComponent();
            this.FormClosing += CloseServer;
            server = new TankServer();
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
