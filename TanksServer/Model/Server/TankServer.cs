using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TanksServer.Model.Server
{
    internal class TankServer
    {
        internal static TcpListener tcpListener;
        internal List<TanksUser> clients;
        internal readonly int PORT;

        public TankServer(int port = 8008)
        {
            clients = new List<TanksUser>();
            this.PORT = port;
        }
        internal void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, PORT);
                tcpListener.Start();
                while (true)
                {
                    TcpClient client = tcpListener.AcceptTcpClient();
                    TanksUser myClient = new TanksUser(client, this);
                    Thread clientThread = new Thread(new ThreadStart(myClient.Work));
                    clientThread.Start();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                CloseServer();
            }
        }

        //
        //  send message all
        //
        internal void BroadcastMsg(string msg, string id)
        {
            byte[] data = Encoding.Unicode.GetBytes(msg);
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Id != id)
                {
                    clients[i].networkStream.Write(data, 0, data.Length);
                }
            }
        }

        //
        //  send message to one person
        //
        internal void PrivateMsg(string msg, string id)
        {
            byte[] data = Encoding.Unicode.GetBytes(msg);
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Id == id)
                {
                    clients[i].networkStream.Write(data, 0, data.Length);
                }
            }
        }

        
        internal void DeleteConnetion(string id)
        {
            TanksUser client = clients.FirstOrDefault(x => x.Id.Equals(id));
            if (client != null)
            {
                clients.Remove(client);
                ServerForm.listBox1.Items.Remove(client.Name);
                return;
            }
        }

        internal void AddConnection(TanksUser TanksUser)
        {
            clients.Add(TanksUser);
        }

        internal void CloseServer()
        {
            tcpListener.Stop();
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close();
            }
        }
    }
}
