using CudaLightSharp.CudaEnumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CudaLightSharp.CudaStructures
{
    /**
    * CSR Matrix representation
    */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal class SparseMemoryTile : MemoryBuffer
    {
        public UIntPtr nonZeroColumnIndices;
        public UIntPtr nNonZeroRows;
        public int nRows;
        public int nCols;

        public SparseMemoryTile(UIntPtr pointer = default(UIntPtr),
                                int nNonZeros = 0,
                                UIntPtr nonZeroColumnIndices = default(UIntPtr),
                                UIntPtr nNonZeroRows = default(UIntPtr),
                                int nRows = 0,
                                int nCols = 0,
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
