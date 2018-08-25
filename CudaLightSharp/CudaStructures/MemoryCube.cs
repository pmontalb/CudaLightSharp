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
    internal class MemoryCube : MemoryTile
    {
        public int nCubes;

        public MemoryCube(UIntPtr pointer = default(UIntPtr),
                          int nRows = 0,
                          int nCols = 0,
                          int nCubes = 0,
                          MemorySpace memorySpace = MemorySpace.Null,
                          MathDomain mathDomain = MathDomain.Null)
            : base(pointer, nRows, nCols, nRows * nCols * nCubes, memorySpace, mathDomain)
        {
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
