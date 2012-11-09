using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOGame
{
    public class XOCoordinate
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Player HoldingPlayer { get; set; }
    }
}
