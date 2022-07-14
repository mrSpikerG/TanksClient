using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TanksClient.Model;

namespace TanksClient.Control
{
    public class ButtonControl
    {

        public void MoveTank(object sender, KeyEventArgs e)
        {
            if (GameForm.GameStarted)
            {
                Task.Factory.StartNew(() =>
                {
                    var MovingTank = GameForm.Tanks.Find(tank => tank.Nickname == GameForm.CurrentUser.Login);
                    if (MovingTank.isAlive)
                    {
                        switch (e.KeyData)
                        {
                            case Keys.Right:
                                if (MovingTank.Coords.X + MovingTank.Size.Width + MovingTank.Speed > 1000)
                                    break;
                                MovingTank.Coords = new Point(MovingTank.Coords.X + MovingTank.Speed, MovingTank.Coords.Y);
                                MovingTank.Direct = GameForm.Direction.EAST;
                                break;

                            case Keys.Left:
                                if (MovingTank.Coords.X - MovingTank.Speed < 0)
                                    break;
                                MovingTank.Coords = new Point(MovingTank.Coords.X - MovingTank.Speed, MovingTank.Coords.Y);
                                MovingTank.Direct = GameForm.Direction.WEST;
                                break;

                            case Keys.Up:
                                if (MovingTank.Coords.Y - MovingTank.Speed < 0)
                                    break;
                                MovingTank.Coords = new Point(MovingTank.Coords.X, MovingTank.Coords.Y - MovingTank.Speed);
                                MovingTank.Direct = GameForm.Direction.NORTH;
                                break;

                            case Keys.Down:
                                if (MovingTank.Coords.Y + MovingTank.Size.Height + MovingTank.Speed > 500)
                                    break;
                                MovingTank.Coords = new Point(MovingTank.Coords.X, MovingTank.Coords.Y + MovingTank.Speed);
                                MovingTank.Direct = GameForm.Direction.SOUTH;
                                break;
                        }
                    }
                });
            }
        }

        internal void SendBullet(object sender, KeyEventArgs e)
        {

            if (GameForm.GameStarted)
            {
                Task.Factory.StartNew(() =>
            {
                if (GameForm.Tanks.Find(tank => tank.Nickname == GameForm.CurrentUser.Login).isAlive)
                {
                    switch (e.KeyData)
                    {
                        case Keys.Space:
                            var UserTank = GameForm.Tanks.Find(tank => tank.Nickname == GameForm.CurrentUser.Login);
                            if (!UserTank.Bullet.isVisible)
                            {
                                UserTank.Bullet.isVisible = true;

                                Point BulletCoords = new Point(-100, -100);
                                switch (UserTank.Direct)
                                {
                                    case GameForm.Direction.NORTH:
                                        BulletCoords = new Point(UserTank.Coords.X + 25, UserTank.Coords.Y + 25 - 35);
                                        break;
                                    case GameForm.Direction.SOUTH:
                                        BulletCoords = new Point(UserTank.Coords.X + 25, UserTank.Coords.Y + 25 + 35);
                                        break;
                                    case GameForm.Direction.EAST:
                                        BulletCoords = new Point(UserTank.Coords.X + 25 + 35, UserTank.Coords.Y + 25);
                                        break;
                                    case GameForm.Direction.WEST:
                                        BulletCoords = new Point(UserTank.Coords.X + 25 - 35, UserTank.Coords.Y + 25);
                                        break;

                                }
                                UserTank.Bullet.Coords = BulletCoords;
                                UserTank.Bullet.Direct = UserTank.Direct;

                                if (AdditionalControl.CheckForDirection(UserTank.Direct) != "N" && AdditionalControl.CheckForDirection(UserTank.Bullet.Direct) != "S")
                                {
                                    UserTank.Bullet.Size = new Size(10, 6);
                                }
                                else
                                {
                                    UserTank.Bullet.Size = new Size(6, 10);
                                }
                            }
                            break;
                    }
                }
            });
            }
        }
    }
}
