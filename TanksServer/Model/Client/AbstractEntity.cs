using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TanksServer.Model.Tank;
using static TanksServer.ServerForm;

namespace TanksServer.Model
{
    public abstract class AbstractEntity
    {
        public Point Coords { get; set; }
        public Size Size { get; set; }
        public Direction Direct { get; set; }
    }
}
