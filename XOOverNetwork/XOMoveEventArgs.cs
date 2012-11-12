using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XOGame;
namespace XOOverNetwork
{
    public class XOMoveEventArgs : EventArgs
    {
        public List<XOCoordinate> Board { get; set; }
    }
}
