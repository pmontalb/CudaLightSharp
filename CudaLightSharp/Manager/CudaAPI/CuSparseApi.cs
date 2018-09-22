using CudaLightSharp.CudaEnumerators;
using CudaLightSharp.CudaStructures;
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

namespace CudaLightSharp.Manager.CudaAPI
{
    internal unsafe static class CuSparseApi
    {
        [DllImport("CudaLightKernels")]
        private static extern unsafe int _SparseAddRaw(PtrT z, PtrT x, PtrT y, uint nNonZeros, PtrT nonZeroIndices, MemorySpace memorySpace, MathDomain mathDomain, uint size, double alpha);
        public static void SparseAdd(MemoryBuffer z, SparseMemoryBuffer x, MemoryBuffer y, double alpha)
        {
            int err = _SparseAddRaw(z.pointer, x.pointer, y.pointer, x.size, x.indices, x.memorySpace, x.mathDomain, z.size, alpha);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_SparseAddRaw", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _SparseSubtractRaw(PtrT z, PtrT x, PtrT y, uint nNonZeros, PtrT nonZeroIndices, MemorySpace memorySpace, MathDomain mathDomain, uint size);
        public static void SparseSubtract(MemoryBuffer z, SparseMemoryBuffer x, MemoryBuffer y)
        {
            int err = _SparseSubtractRaw(z.pointer, x.pointer, y.pointer, x.size, x.indices, x.memorySpace, x.mathDomain, z.size);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_SparseSubtract", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _SparseDotRaw(PtrT y, PtrT A, PtrT x, uint nNonZeros, PtrT nonZeroColumnIndices, PtrT nNonZeroRows, uint nRows, uint nCols, MemorySpace memorySpace, MathDomain mathDomain, MatrixOperation aOperation, double alpha);
        public static void SparseDot(MemoryBuffer y, SparseMemoryTile A, MemoryBuffer x, MatrixOperation aOperation, double alpha)
        {
            int err = _SparseDotRaw(y.pointer, A.pointer, x.pointer, A.size, A.nonZeroColumnIndices, A.nNonZeroRows, A.nRows, A.nCols, A.memorySpace, A.mathDomain, aOperation, alpha);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_SparseDotRaw", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _SparseMultiplyRaw(PtrT A, PtrT B, PtrT C, uint nNonZeros, PtrT nonZeroColumnIndices, PtrT nNonZeroRows, uint nRowsB, uint nRowsC, uint nColsC, MemorySpace memorySpace, MathDomain mathDomain, uint leadingDimensionB, uint leadingDimensionC, MatrixOperation bOperation, double alpha);
        public static void SparseMultiply(MemoryTile A, SparseMemoryTile B, MemoryTile C, int leadingDimensionB, int leadingDimensionC, MatrixOperation bOperation, MatrixOperation cOperation, double alpha)
        {
            int err = _SparseMultiplyRaw(A.pointer, B.pointer, C.pointer, B.size, B.nonZeroColumnIndices, B.nNonZeroRows, B.nRows, C.nRows, C.nCols, B.memorySpace, B.mathDomain, (uint)leadingDimensionB, (uint)leadingDimensionC, bOperation, alpha);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_SparseMultiplyRaw", err);
        }
    }
}