using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TanksServer.ServerForm;

namespace TanksServer.Model
{
    public class Tank : AbstractEntity
    {
        
        public Bullet Bullet{ get; set; }

        public string Nickname { get; set; }
        public int HealthPoint { get; set; }
        public int Speed{ get; set; }
        public bool isAlive { get; set; }

        public Tank()
        {

        }
        public Tank(Point coord,string nick, int HealthPoint = 1, int strength = 1, int speed = 1,int tankSpeed = 1,Direction direction = Direction.NORTH)
        {
            this.isAlive = true;
            this.HealthPoint = HealthPoint;
            this.Speed = tankSpeed;
            this.Coords = coord;
            this.Size = new Size(50, 50);
            this.Nickname = nick;
            this.Bullet = new Bullet(strength,speed);
            this.Direct = direction;   
        }
        
      
    }
}
