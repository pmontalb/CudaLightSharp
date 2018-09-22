﻿using CudaLightSharp.CudaEnumerators;
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
    internal unsafe static class CuBlasApi
    {
        [DllImport("CudaLightKernels")]
        private static extern unsafe int _AddRaw(PtrT z, PtrT x, PtrT y, uint size, MemorySpace memorySpace, MathDomain mathDomain, double alpha);
        public static void Add(MemoryBuffer z, MemoryBuffer x, MemoryBuffer y, double alpha)
        {
            int err = _AddRaw(z.pointer, x.pointer, y.pointer, z.size, z.memorySpace, z.mathDomain, alpha);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_AddRaw", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _SubtractRaw(PtrT z, PtrT x, PtrT y, uint size, MemorySpace memorySpace, MathDomain mathDomain);
        public static void Subtract(MemoryBuffer z, MemoryBuffer x, MemoryBuffer y)
        {
            int err = _SubtractRaw(z.pointer, x.pointer, y.pointer, z.size, z.memorySpace, z.mathDomain);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_SubtractRaw", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _AddEqualRaw(PtrT z, PtrT x, uint size, MemorySpace memorySpace, MathDomain mathDomain, double alpha);
        public static void AddEqual(MemoryBuffer z, MemoryBuffer x, double alpha)
        {
            int err = _AddEqualRaw(z.pointer, x.pointer, z.size, z.memorySpace, z.mathDomain, alpha);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_AddEqualRaw", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _AddEqualMatrixRaw(PtrT A, PtrT B, uint nRows, uint nCols, MemorySpace memorySpace, MathDomain mathDomain, MatrixOperation aOperation, MatrixOperation bOperation, double alpha);
        public static void AddEqualMatrix(MemoryTile A, MemoryTile B, MatrixOperation aOperation, MatrixOperation bOperation, double alpha)
        {
            int err = _AddEqualMatrixRaw(A.pointer, B.pointer, A.nRows, A.nCols, A.memorySpace, A.mathDomain, aOperation, bOperation, alpha);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_AddEqualMatrixRaw", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _SubtractEqualRaw(PtrT z, PtrT x, uint size, MemorySpace memorySpace, MathDomain mathDomain);
        public static void SubtractEqual(MemoryBuffer z, MemoryBuffer x)
        {
            int err = _SubtractEqualRaw(z.pointer, x.pointer, z.size, z.memorySpace, z.mathDomain);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_SubtractEqualRaw", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _ScaleRaw(PtrT z, uint size, MemorySpace memorySpace, MathDomain mathDomain, double alpha);
        public static void Scale(MemoryBuffer z, double alpha)
        {
            int err = _ScaleRaw(z.pointer, z.size, z.memorySpace, z.mathDomain, alpha);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_ScaleRaw", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _ElementwiseProductRaw(PtrT z, PtrT x, PtrT y, uint size, MemorySpace memorySpace, MathDomain mathDomain, double alpha = 1.0);
        public static void ElementwiseProduct(MemoryBuffer z, MemoryBuffer x, MemoryBuffer y, double alpha)
        {
            int err = _ElementwiseProductRaw(z.pointer, x.pointer, y.pointer, z.size, z.memorySpace, z.mathDomain, alpha);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_ElementwiseProductRaw", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _MultiplyRaw(PtrT A, PtrT B, PtrT C, uint nRowsB, uint nRowsC, uint nColsC, MemorySpace memorySpace, MathDomain mathDomain, uint leadingDimensionB, uint leadingDimensionC, MatrixOperation bOperation, MatrixOperation cOperation, double alpha);
        public static void Multiply(MemoryTile A, MemoryTile B, MemoryTile C, int leadingDimensionB, int leadingDimensionC, MatrixOperation bOperation, MatrixOperation cOperation, double alpha)
        {
            int err = _MultiplyRaw(A.pointer, B.pointer, C.pointer, B.nRows, C.nRows, C.nCols, A.memorySpace, A.mathDomain, (uint)leadingDimensionB, (uint)leadingDimensionC, bOperation, cOperation, alpha);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_MultiplyRaw", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _DotRaw(PtrT y, PtrT A, PtrT x, uint nRows, uint nCols, MemorySpace memorySpace, MathDomain mathDomain, MatrixOperation aOperation, double alpha);
        public static void Dot(MemoryBuffer y, MemoryTile A, MemoryBuffer x, MatrixOperation aOperation, double alpha)
        {
            int err = _DotRaw(y.pointer, A.pointer, x.pointer, A.nRows, A.nCols, A.memorySpace, A.mathDomain, aOperation, alpha);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_DotRaw", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _CumulativeRowSumRaw(PtrT A, uint nRows, uint nCols, MemorySpace memorySpace, MathDomain mathDomain);
        public static void CumulativeRowSum(MemoryTile A)
        {
            int err = _CumulativeRowSumRaw(A.pointer, A.nRows, A.nCols, A.memorySpace, A.mathDomain);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_CumulativeRowSumRaw", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _EyeRaw(PtrT pointer, uint nRows, MemorySpace memorySpace, MathDomain mathDomain);
        public static void Eye(MemoryTile A)
        {
            int err = _EyeRaw(A.pointer, A.nRows, A.memorySpace, A.mathDomain);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_EyeRaw", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _SolveRaw(PtrT A, PtrT B, uint nRows, uint nCols, MemorySpace memorySpace, MathDomain mathDomain, MatrixOperation aOperation);
        public static void Solve(MemoryTile A, MemoryTile B, MatrixOperation aOperation)
        {
            int err = _SolveRaw(A.pointer, B.pointer, A.nRows, A.nCols, A.memorySpace, A.mathDomain, aOperation);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_SolveRaw", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _InvertRaw(PtrT A, uint nRows, uint nCols, MemorySpace memorySpace, MathDomain mathDomain, MatrixOperation aOperation);
        public static void Invert(MemoryTile A, MatrixOperation aOperation)
        {
            int err = _InvertRaw(A.pointer, A.nRows, A.nCols, A.memorySpace, A.mathDomain, aOperation);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_InvertRaw", err);
        }
    }
}