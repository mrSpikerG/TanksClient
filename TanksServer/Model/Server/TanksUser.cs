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
            this.Id = Guid.NewGuid().ToString();
            this._server = TanksServer;
            this.tcpClient = tcpClient;
                this.networkStream = tcpClient.GetStream();
       //     this.Name = GetMsg();
           // ServerForm.listBox1.Items.Add(this.Name);
            this._server.AddConnection(this);
        }

        public void Work()
        {
            try
            {
                while (true)
                {
                    // if (networkStream.DataAvailable)
                  //  {
                        try
                        {

                            string json = GetMsg();
                            if (json.StartsWith("cmd"))
                            {
                                string txt = json.Remove(0, 4);
                                switch (txt)
                                {
                                    case "getStats":
                                        break;
                                }
                            }
                            else
                            {
                                /* if (ServerForm.listBox1.Items == null)
                                 {
                                     this.Name = JsonSerializer.Deserialize<Tank>(json).Nickname;
                                     ServerForm.listBox1.Items.Add(this.Name);
                                 }
                                 else
                                 {*/

                                if (json != String.Empty)
                                {
                                    _server.BroadcastMsg(json, this.Id);
                                  /*  if (File.ReadAllText("Data\\activePlayers.txt").Split('\n').Any<string>((elem) => elem != JsonSerializer.Deserialize<Tank>(json).Nickname))
                                    {
                                        File.AppendAllText("Data\\activePlayers.txt", $"{JsonSerializer.Deserialize<Tank>(json).Nickname}\n");
                                        this.Name = JsonSerializer.Deserialize<Tank>(json).Nickname;
                                    }*/
                                }
                                //}
                            }

                            //File.WriteAllText("test.json", GetMsg());
                            //Console.WriteLine($"{GetMsg()}\n\n");

                        }
                        catch (Exception ex)
                        {

                            Console.WriteLine(ex.Message);
                            //   Console.WriteLine(ex.Message);
                            //   Console.WriteLine(ex.Message);
                            break;
                        }
                   // }
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
            while (networkStream.DataAvailable)
            {
                byteCount = networkStream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, byteCount));
            }

            return builder.ToString();
        }

        public void Close()
        {
            tcpClient.Close();
            networkStream.Close();
        }
    }
}
