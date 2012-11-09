using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XOGame;
namespace XOOverNetwork
{
    public class XOGameOverEventArgs : EventArgs
    {
        public List<XOCoordinate> Solution { get; set; }
        public List<XOCoordinate> Board { get; set; }
    }
}
