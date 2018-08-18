using CudaLightSharp.CudaEnumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CudaLightSharp.CudaStructures
{
    internal class MemoryTile : MemoryBuffer
    {
        public int nRows;
        public int nCols;

        public MemoryTile(UIntPtr pointer = default(UIntPtr),
                          int nRows = 0,
                          int nCols = 0,
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
            : base(buffer)
        {
            this.nRows = buffer.size;
            this.nCols = 1;
        }

        protected MemoryTile(UIntPtr pointer,
            int nRows, int nCols, int size,
            MemorySpace memorySpace, MathDomain mathDomain)
            : base(pointer, size, memorySpace, mathDomain)
        {
            this.nRows = nRows;
            this.nCols = nCols;
        }
    }
}