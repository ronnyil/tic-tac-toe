using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOOverNetwork
{
    public delegate void ConnectionEstablishedEventHandler(object sender, XOConnectionEstablishedEventArgs e);
    public delegate void RecievedMoveEventHandler(object sender, XOMoveEventArgs e);
    public delegate void GameOverEventHandler(object sender, XOGameOverEventArgs e);
}
