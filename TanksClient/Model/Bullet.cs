using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TanksClient.Model
{
    public class Bullet :AbstractEntity
    {
        public int Strength { get; set; }
        public int Speed { get; set; }
        public bool isVisible { get; set; }
        public Bullet(int strength,int speed)
        {
            this.Coords = new Point(-200, -200);
            this.Strength = strength;
            this.Speed= speed;
            this.Size = new Size(6, 10);
        }
    }
}
