using CudaLightSharp.CudaEnumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CudaLightSharp.CudaStructures
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal class SparseMemoryBuffer : MemoryBuffer
    {
        public UIntPtr indices;

        SparseMemoryBuffer(UIntPtr pointer = default(UIntPtr),
            int nNonZeros = 0,
            UIntPtr indices = default(UIntPtr),
            MemorySpace memorySpace = MemorySpace.Null,
            MathDomain mathDomain = MathDomain.Null)
            : base(pointer, nNonZeros, memorySpace, mathDomain)
        {
            this.indices = indices;
        }
    };
}
