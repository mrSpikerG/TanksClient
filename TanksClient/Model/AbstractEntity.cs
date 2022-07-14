using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TanksClient.Model.Tank;

namespace TanksClient.Model
{
    public abstract class AbstractEntity
    {
        public Point Coords { get; set; }
        public Size Size { get; set; }
        public GameForm.Direction Direct { get; set; }
    }
}
