using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TanksServer.Model.Server
{
    class TanksUser
    {

        public string Name { get; set; }

        private TankServer _server;

        public string Id { get; private set; }
        protected TcpClient tcpClient;
        internal NetworkStream networkStream { get; set; }

        public TanksUser(TcpClient tcpClient, TankServer TanksServer)
        {
            Id = Guid.NewGuid().ToString();
            _server = TanksServer;
            this.tcpClient = tcpClient;
            _server.AddConnection(this);


            if (!Directory.Exists("UserStats"))
            {
                Directory.CreateDirectory("UserStats");
            }
        }

        public void Work()
        {
            try
            {
                networkStream = tcpClient.GetStream();
                while (true)
                {
                    try
                    {
                        
                        string json = GetMsg();
                        if (json.StartsWith("cmd"))
                        {
                            string txt = json.Remove(0, 4);
                            switch(txt){
                                case "getStats":         
                                    break;
                            }
                        }
                        else
                        {
                            if (ServerForm.listBox1.Items.IndexOf(JsonSerializer.Deserialize<Tank>(json).Nickname) == -1)
                            {
                                this.Name = JsonSerializer.Deserialize<Tank>(json).Nickname;
                                ServerForm.listBox1.Items.Add(this.Name);
                            }
                        _server.BroadcastMsg(GetMsg(), this.Id);
                        }
                        
                        //File.WriteAllText("test.json", GetMsg());
                       //Console.WriteLine($"{GetMsg()}\n\n");

                    }
                    catch (Exception ex)
                    {
                     
                       Console.WriteLine(ex.Message);
                       Console.WriteLine(ex.Message);
                       Console.WriteLine(ex.Message);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _server.DeleteConnetion(this.Id);
                Close();
            }
        }

        public string GetMsg()
        {
            byte[] data = new byte[2048];
            StringBuilder builder = new StringBuilder();
            int byteCount = 0;
            do
            {
                byteCount = networkStream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, byteCount));
            } while (networkStream.DataAvailable);

            return builder.ToString();
        }

        public void Close()
        {
            tcpClient.Close();
            networkStream.Close();
        }
    }
}
