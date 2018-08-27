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
    internal class MemoryCube: MemoryTile
    {
        public uint nCubes;

        public MemoryCube(PtrT pointer = 0,
                          uint nRows = 0,
                          uint nCols = 0,
                          uint nCubes = 0,
                          MemorySpace memorySpace = MemorySpace.Null,
                          MathDomain mathDomain = MathDomain.Null)
            : base(pointer, nRows, nCols, memorySpace, mathDomain)
        {
            this.size = nRows * nCols * nCubes;
            this.nRows = nRows;
            this.nCols = nCols;
            this.nCubes = nCubes;
        }

        public MemoryCube(MemoryCube rhs)
            : this(rhs.pointer, rhs.nRows, rhs.nCols, rhs.nCubes, rhs.memorySpace, rhs.mathDomain)
        {
        }

        public MemoryCube(MemoryTile tile)
            : this(tile.pointer, tile.nRows, tile.nCols, 1, tile.memorySpace, tile.mathDomain)
        {

        }

        public MemoryCube(MemoryBuffer buffer)
            : this(buffer.pointer, buffer.size, 1, 1, buffer.memorySpace, buffer.mathDomain)
        {

        }
    }
}
