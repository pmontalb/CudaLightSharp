using CudaLightSharp.CudaEnumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

#if FORCE_32_BIT
    using PtrT = System.UInt32;
#else
using PtrT = System.UInt64;
#endif

namespace CudaLightSharp.CudaStructures
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal class SparseMemoryBuffer: MemoryBuffer
    {
        public PtrT indices;

        SparseMemoryBuffer(PtrT pointer = 0,
                           uint nNonZeros = 0,
                           PtrT indices = 0,
                           MemorySpace memorySpace = MemorySpace.Null,
                           MathDomain mathDomain = MathDomain.Null)
            : base(pointer, nNonZeros, memorySpace, mathDomain)
        {
            this.indices = indices;
        }
    };
}
