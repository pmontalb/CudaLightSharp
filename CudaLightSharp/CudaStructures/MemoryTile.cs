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
    internal class MemoryTile: MemoryBuffer
    {
        public uint nRows;
        public uint nCols;

        public MemoryTile(PtrT pointer = 0,
                          uint nRows = 0,
                          uint nCols = 0,
                          MemorySpace memorySpace = MemorySpace.Null,
                          MathDomain mathDomain = MathDomain.Null)
               : base(pointer, nRows * nCols, memorySpace, mathDomain)
        {
            this.nRows = nRows;
            this.nCols = nCols;
        }

        public MemoryTile(MemoryTile rhs)
            : this(rhs.pointer, rhs.nRows, rhs.nCols, rhs.memorySpace, rhs.mathDomain)
        {
        }

        public MemoryTile(MemoryBuffer buffer)
            : this(buffer.pointer, buffer.size, 1, buffer.memorySpace, buffer.mathDomain)
        {
        }
    }
}