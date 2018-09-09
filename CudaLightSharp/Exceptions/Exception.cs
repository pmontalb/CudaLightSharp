using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CudaLightSharp.Exceptions
{
    [Serializable]
    internal class InternalErrorException : Exception
    {
        public InternalErrorException(string message = "")
            : base("InternalErrorException: " + message)
        {
        }
    };

    [Serializable]
    internal class ExpectedEvenSizeException : Exception
    {
        public ExpectedEvenSizeException(string message = "")
            : base("ExpectedEvenSizeException: " + message)
        {
        }
    };

    [Serializable]
    internal class NotSupportedException : Exception
    {
        public NotSupportedException(string message = "")
            : base("NotSupportedException: " + message)
        {
        }
    };

    [Serializable]
    internal class NotImplementedException : Exception
    {
        public NotImplementedException(string message = "")
            : base("NotImplementedException: " + message)
        {
        }
    };

    [Serializable]
    internal class BufferNotInitialisedException : Exception
    {
        public BufferNotInitialisedException(string message = "")
            : base("BufferNotInitialisedException: " + message)
        {
        }
    };
}
