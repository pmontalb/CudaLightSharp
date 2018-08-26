using CudaLightSharp.CudaEnumerators;
using CudaLightSharp.CudaStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CudaLightSharp.Manager.CudaAPI
{
    internal unsafe static class CuSparseApi
    {
        [DllImport("CudaLightKernels")]
        private static extern unsafe int _SparseAdd(MemoryBuffer z, SparseMemoryBuffer x, MemoryBuffer y, double alpha);
        public static void SparseAdd(MemoryBuffer z, SparseMemoryBuffer x, MemoryBuffer y, double alpha)
        {
            int err = _SparseAdd(z, x, y, alpha);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_SparseAdd", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _SparseSubtract(MemoryBuffer z, SparseMemoryBuffer x, MemoryBuffer y);
        public static void SparseSubtract(MemoryBuffer z, SparseMemoryBuffer x, MemoryBuffer y)
        {
            int err = _SparseSubtract(z, x, y);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_SparseSubtract", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _SparseDot(MemoryBuffer y, SparseMemoryTile A, MemoryBuffer x, MatrixOperation aOperation, double alpha);
        public static void SparseDot(MemoryBuffer y, SparseMemoryTile A, MemoryBuffer x, MatrixOperation aOperation, double alpha)
        {
            int err = _SparseDot(y, A, x, aOperation, alpha);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_SparseDot", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _SparseMultiply(MemoryTile A, SparseMemoryTile B, MemoryTile C, int leadingDimensionB, int leadingDimensionC, MatrixOperation bOperation, MatrixOperation cOperation, double alpha);
        public static void SparseMultiply(MemoryTile A, SparseMemoryTile B, MemoryTile C, int leadingDimensionB, int leadingDimensionC, MatrixOperation bOperation, MatrixOperation cOperation, double alpha)
        {
            int err = _SparseMultiply(A, B, C, leadingDimensionB, leadingDimensionC, bOperation, cOperation, alpha);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_SparseMultiply", err);
        }
    }
}