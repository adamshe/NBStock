using System;

namespace NB.StockStudio
{
    public class TerminateException : Exception
    {

        public TerminateException()
        {
        }

        public TerminateException(string Message) : base(Message)
        {
        }
    }

}
