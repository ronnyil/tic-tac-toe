using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using XOGame;
namespace XOOverNetwork
{
    public class XOGuestPlayer
    {
        //Enum Declarations
        private enum OuterStates
        {
            WaitingForConnection,
            Playing,
            Over
        }
        private enum InnerStates
        {
            WaitingForXMove,
            WaitingForMyMove
        }

        private Encoding encoder;
        private XOCoordinate _lastOMove;


        //Statechart variables
        private OuterStates _outerState;
        private InnerStates _innerState;

        //Threading variables
        private object _lastOMoveLock;
        private AutoResetEvent OMoved;
        private Thread _listeningThread;

        //Network variables
        private const int PORT = 21212;
        private int _port;
        private TcpListener _listener;
        private TcpClient _host;
        private NetworkStream _clientStream;

        //Events
        public event RecievedMoveEventHandler MoveRecieved;
        public event GameOverEventHandler GameOver;
        public event ConnectionEstablishedEventHandler ConnectionEstablished;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="port"></param>
        public XOGuestPlayer(int port = PORT)
        {
            _port = port;
            _listener = new TcpListener(IPAddress.Any, _port);
            _listeningThread = new Thread(new ThreadStart(ListenForClients));
            _lastOMoveLock = new object();
            encoder = new ASCIIEncoding();
        }

        private void ListenForClients()
        {
            _listener.Start();
            _outerState = OuterStates.WaitingForConnection;
            _host = _listener.AcceptTcpClient();
            _clientStream = _host.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;
            string stringMessage = "";

            while (true)
            {
                if (!(_outerState == OuterStates.Playing && _innerState == InnerStates.WaitingForMyMove))
                {

                    bytesRead = 0;
                    try
                    {
                        bytesRead = _clientStream.Read(message, 0, 4096);
                    }
                    catch (Exception)
                    {
                        break;
                    }
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    stringMessage = encoder.GetString(message, 0, bytesRead);
                }
                switch (_outerState)
                {
                    case OuterStates.WaitingForConnection:
                        //POSSIBLE: Think about implementing a naming mechanism in the original handshake.
                        if (stringMessage == "HI")
                        {
                            byte[] buffer = encoder.GetBytes("HIACK");
                            _clientStream.Write(buffer, 0, buffer.Length);
                            IPEndPoint endPoint = (IPEndPoint)_host.Client.RemoteEndPoint;
                            if (ConnectionEstablished.GetInvocationList().Length > 0)
                            {
                                XOConnectionEstablishedEventArgs args = new XOConnectionEstablishedEventArgs();
                                args.RemoteIPAddress = endPoint.Address;
                                args.RemotePort = endPoint.Port;
                                ConnectionEstablished(this, args);
                            }
                            _outerState = OuterStates.Playing;
                            _innerState = InnerStates.WaitingForXMove;
                        }
                        else
                        {
                            return;
                        }
                        break;
                    case OuterStates.Playing:
                        switch (_innerState)
                        {
                            case InnerStates.WaitingForXMove:
                                string messageType = stringMessage.Substring(0, 2);
                                if (messageType == "XM")
                                {
                                    XMoved(stringMessage);
                                    _innerState = InnerStates.WaitingForMyMove;
                                }
                                else if (messageType == "GO")
                                {
                                    DoGameOver(stringMessage);
                                    _outerState = OuterStates.Over;
                                }
                                break;
                            case InnerStates.WaitingForMyMove:
                                WaitForMyMove();
                                _innerState = InnerStates.WaitingForXMove;
                                break;
                            default:
                                break;
                        }
                        break;
                    case OuterStates.Over:
                        break;
                    default:
                        break;
                }
            }
        }

        private void WaitForMyMove()
        {
            OMoved.WaitOne();
            string sendString = String.Format("OM:X={0}:Y={1}", LastOMove.X, LastOMove.Y);
            byte[] stringBuffer = encoder.GetBytes(sendString);
            _clientStream.Write(stringBuffer, 0, stringBuffer.Length);
        }

        private void DoGameOver(string stringMessage)
        {
            var splitArray = stringMessage.Split(':');
            string boardString = splitArray[1].Reverse().ToString();
            var board = MakeBoardFromString(boardString);
            string solutionString = splitArray[2].Reverse().ToString();
            List<XOCoordinate> solution = new List<XOCoordinate>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (solutionString[i * 3 + j] == '1')
                    {
                        
                    }
                }
            }
            //TODO finish up this function.
            //TODO Call game over event
            if (GameOver.GetInvocationList().Count() > 0)
            {
               // GameOver(this, new XOGameOverEventArgs() { 
            }

        }

        private void XMoved(string stringMessage)
        {
            string boardString = stringMessage.Split(':')[1].Reverse().ToString();
            if (MoveRecieved.GetInvocationList().Length > 0)
            {
                MoveRecieved(this, new XOMoveEventArgs() { Board = MakeBoardFromString(boardString) });
            }
        }
        /// <summary>
        /// Helper function that turns a string of the format "XOOOXNNOX" into a list of
        /// XOCoordinates.
        /// </summary>
        /// <param name="boardString">The board string of of format /[XON]{9}/"</param>
        /// <returns>A list of XOCoordinates that make up the board.</returns>
        private List<XOCoordinate> MakeBoardFromString(string boardString)
        {
            List<XOCoordinate> board = new List<XOCoordinate>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    XOCoordinate coord = new XOCoordinate() { X = i, Y = j };
                    switch (boardString[i * 3 + j])
                    {
                        case 'X':
                            coord.HoldingPlayer = Player.X;
                            break;
                        case 'O':
                            coord.HoldingPlayer = Player.O;
                            break;
                        default:
                            coord.HoldingPlayer = Player.NotSet;
                            break;
                    }
                    board.Add(coord);
                }
            }
            return board;
        }

        public void Listen()
        {
            _listeningThread.Start();
        }

        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        public XOCoordinate LastOMove
        {
            get
            {
                lock (_lastOMoveLock)
                {
                    return _lastOMove;
                }
            }
            set
            {
                lock (_lastOMoveLock)
                {
                    _lastOMove = value;
                    OMoved.Set();
                }
            }
        }
    }
}
