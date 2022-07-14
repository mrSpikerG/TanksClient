using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using TanksClient.Model;
using TanksClient.View;

namespace TanksClient.Control
{
    public class TimerControl
    {
        public static Thread receiveMsgThread;


        public static void UpdateBullets(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < GameForm.Tanks.Count; i++)
                {
                    if (GameForm.Tanks[i].Bullet.isVisible)
                    {
                        int xMultiply = 0;
                        int yMultiply = 0;


                        Tank TempTank = GameForm.Tanks[i];
                        switch (TempTank.Bullet.Direct)
                        {
                            case GameForm.Direction.NORTH:
                                if (TempTank.Bullet.Coords.Y < 0)
                                {
                                    TempTank.Bullet.isVisible = false;
                                    TempTank.Bullet.Coords = new Point(-200, -200);
                                }
                                yMultiply = -2;
                                break;
                            case GameForm.Direction.SOUTH:
                                if (TempTank.Bullet.Coords.Y > 500)
                                {
                                    TempTank.Bullet.isVisible = false;
                                    TempTank.Bullet.Coords = new Point(-200, -200);
                                }
                                yMultiply = 2;
                                break;
                            case GameForm.Direction.EAST:
                                if (TempTank.Bullet.Coords.X > 1000)
                                {
                                    TempTank.Bullet.isVisible = false;
                                    TempTank.Bullet.Coords = new Point(-200, -200);
                                }
                                xMultiply = 2;
                                break;
                            case GameForm.Direction.WEST:
                                if (TempTank.Bullet.Coords.X < 0)
                                {
                                    TempTank.Bullet.isVisible = false;
                                    TempTank.Bullet.Coords = new Point(-200, -200);
                                }
                                xMultiply = -2;
                                break;
                        }
                        GameForm.Tanks[i].Bullet.Coords = new Point(
                            TempTank.Bullet.Coords.X + xMultiply * TempTank.Bullet.Speed,
                            TempTank.Bullet.Coords.Y + yMultiply * TempTank.Bullet.Speed
                            );
                        Rectangle Bullet = new Rectangle(TempTank.Bullet.Coords, TempTank.Bullet.Size);
                        for (int j = 0; j < GameForm.Tanks.Count; j++)
                        {
                            if (Bullet.IntersectsWith(new Rectangle(GameForm.Tanks[j].Coords, GameForm.Tanks[j].Size)))
                            {
                                if (GameForm.Tanks[i].isAlive)
                                {
                                    TempTank.Bullet.isVisible = false;
                                    TempTank.Bullet.Coords = new Point(-200, -200);
                                    GameForm.Tanks[j].HealthPoint -= TempTank.Bullet.Strength;
                                }
                            }
                        }
                    }
                }
            });
        }
        public TimerControl()
        {
           
        }
        internal static void NetworkUpdate(object sender, ElapsedEventArgs e)
        {
         //   if (GameForm.Tanks.Count<Tank>((tank) => tank.isAlive) == 1 && GameForm.Tanks.Count != 1) 
         //   {
         //       MainMenuForm.Money += 10;
         //   }
            GameForm.GameStarted = true;
                byte[] buffer = Encoding.Unicode.GetBytes(JsonSerializer.Serialize(GameForm.Tanks.FindLast(tank => tank.Nickname == GameForm.CurrentUser.Login)));
                MainMenuForm.stream.Write(buffer, 0, buffer.Length);
                receiveMsgThread = new Thread(ReceiveMsg);
                receiveMsgThread.Start();
            
           /* else
            {
                if(GameForm.Tanks.Count != 1)
                {
                    File.AppendAllText("temp.txt", "1");
                }

            }*/
          //      if(GameForm.Tanks.FindLast(tank => tank.Nickname == GameForm.CurrentUser.Login).isAlive)
          //      {
          //          
          //      }
          //  }
            
        }

        private static void ReceiveMsg()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[2048];
                    StringBuilder builder = new StringBuilder();
                    builder.Clear();
                    int byteCount = 0;
                    do
                    {
                        byteCount = MainMenuForm.stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, byteCount));
                    } while (MainMenuForm.stream.DataAvailable);

                    Tank temp = JsonSerializer.Deserialize<Tank>(builder.ToString());

                    //  Нет слов... одни эмоции
                    if (GameForm.Tanks.IndexOf(GameForm.Tanks.Find(tank => tank.Nickname == temp.Nickname)) != -1)
                    {
                        GameForm.Tanks[GameForm.Tanks.IndexOf(GameForm.Tanks.Find(tank => tank.Nickname == temp.Nickname))] = temp;
                    }
                    else
                    {
                        if (GameForm.Tanks.Count < 4)
                        {
                            GameForm.Tanks.Add(temp);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    
                }
            }

        }


        internal static void DeathCheck(object sender, ElapsedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < GameForm.Tanks.Count; i++)
                {
                    if (GameForm.Tanks[i].HealthPoint <= 0 && GameForm.Tanks[i].isAlive)
                    {
                        GameForm.Tanks[i].isAlive = false;
                    }
                }
            });
        }
    }
}
