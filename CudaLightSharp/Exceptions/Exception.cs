using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CudaLightSharp.Exceptions
{
    internal class InternalErrorException : Exception
    {
        public InternalErrorException(string message = "")
            : base("InternalErrorException: " + message)
        {
        }
    };

    internal class ExpectedEvenSizeException : Exception
    {
        public ExpectedEvenSizeException(string message = "")
            : base("ExpectedEvenSizeException: " + message)
        {
        }
    };

    internal class NotSupportedException : Exception
    {
        public NotSupportedException(string message = "")
            : base("NotSupportedException: " + message)
        {
        }
    };

    internal class NotImplementedException : Exception
    {
        public NotImplementedException(string message = "")
            : base("NotImplementedException: " + message)
        {
        }
    };

    internal class BufferNotInitialisedException : Exception
    {
        public BufferNotInitialisedException(string message = "")
            : base("BufferNotInitialisedException: " + message)
        {
        }
    };
}
