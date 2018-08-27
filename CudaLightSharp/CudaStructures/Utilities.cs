using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CudaLightSharp.CudaStructures
{
    internal static class Utilities
    {
        static uint GetElementarySize(MemoryBuffer memoryBuffer)
        {
            return memoryBuffer.ElementarySize();
        }

        static int GetTotalSize(MemoryBuffer memoryBuffer)
        {
            return (int)(memoryBuffer.TotalSize);
        }

        static void ExtractColumnBufferFromMatrix(out MemoryBuffer output, MemoryTile rhs, uint column)
        {
            Debug.Assert(column < rhs.nCols);
            output = new MemoryBuffer(rhs.pointer + (column * rhs.nRows * rhs.ElementarySize()), rhs.nRows, rhs.memorySpace, rhs.mathDomain);
        }

        static void ExtractColumnBufferFromCube(out MemoryBuffer output, MemoryCube rhs, uint matrix, uint column)
        {
            Debug.Assert(matrix < rhs.nCubes);
            Debug.Assert(column < rhs.nCols);
            output = new MemoryBuffer(rhs.pointer + (rhs.nRows * (matrix * rhs.nCols + column) * rhs.ElementarySize()),
                             rhs.nRows,
                             rhs.memorySpace,
                             rhs.mathDomain);
        }

        static void ExtractMatrixBufferFromCube(MemoryTile output, MemoryCube rhs, uint matrix)
        {
            Debug.Assert(matrix < rhs.nCubes);
            output = new MemoryTile(rhs.pointer + (matrix * rhs.nRows * rhs.nCols * rhs.ElementarySize()),
                               rhs.nRows, rhs.nCols,
                               rhs.memorySpace,
                               rhs.mathDomain);
        }
    }
}
