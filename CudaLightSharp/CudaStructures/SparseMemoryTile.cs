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
    /**
    * CSR Matrix representation
    */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal class SparseMemoryTile: MemoryBuffer
    {
        public PtrT nonZeroColumnIndices;
        public PtrT nNonZeroRows;
        public uint nRows;
        public uint nCols;

        public SparseMemoryTile(PtrT pointer = 0,
                                uint nNonZeros = 0,
                                PtrT nonZeroColumnIndices = 0,
                                PtrT nNonZeroRows = 0,
                                uint nRows = 0,
                                uint nCols = 0,
                                MemorySpace memorySpace = MemorySpace.Null,
                                MathDomain mathDomain = MathDomain.Null)
            : base(pointer, nNonZeros, memorySpace, mathDomain)
        {
            this.nonZeroColumnIndices = nonZeroColumnIndices;
            this.nNonZeroRows = nNonZeroRows;
            this.nRows = nRows;
            this.nCols = nCols;
        }
    }
}
