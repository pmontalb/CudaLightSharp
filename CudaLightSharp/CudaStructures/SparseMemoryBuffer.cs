using CudaLightSharp.CudaEnumerators;
using System.Runtime.InteropServices;

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

        public SparseMemoryBuffer(PtrT pointer = 0,
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
