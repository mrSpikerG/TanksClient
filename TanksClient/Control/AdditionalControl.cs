using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanksClient.Model;
using static TanksClient.Model.Tank;

namespace TanksClient.Control
{
    public static class AdditionalControl
    {
       
        public static string CheckForDirection(GameForm.Direction direction)
        {
            switch (direction)
            {
                case GameForm.Direction.NORTH:
                    return "N";

                case GameForm.Direction.SOUTH:
                    return "S";

                case GameForm.Direction.EAST:
                    return "E";

                case GameForm.Direction.WEST:
                    return "W";
            }
            return "Error In CheckForDirection";
        }
    }
}
