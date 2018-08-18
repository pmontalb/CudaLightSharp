using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CudaLightSharp.CudaEnumerators
{
    internal enum CudaKernelException
    {
        _NotImplementedException = -1,
        _NotSupportedException = -2,
        _ExpectedEvenSizeException = -3,
        _InternalException = -4
    }
}
