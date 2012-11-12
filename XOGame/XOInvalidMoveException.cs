using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOGame
{
    public enum XOInvalidMoveExceptionReasons
    {
        SamePlayerTwice,
        AlreadySetPlace
    }
    public class XOInvalidMoveException : Exception
    {
        public XOInvalidMoveException(string message,XOInvalidMoveExceptionReasons reason) : base(message)
        {

        }
        public XOInvalidMoveExceptionReasons Reason { get; set; }
    }
}
