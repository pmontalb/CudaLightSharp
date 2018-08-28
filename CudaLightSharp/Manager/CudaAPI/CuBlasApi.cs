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
    internal unsafe static class CuBlasApi
    {
        [DllImport("CudaLightKernels")]
        private static extern unsafe int _Add(MemoryBuffer z, MemoryBuffer x, MemoryBuffer y, double alpha);
        public static void Add(MemoryBuffer z, MemoryBuffer x, MemoryBuffer y, double alpha)
        {
            int err = _Add(z, x, y, alpha);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_Add", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _Subtract(MemoryBuffer z, MemoryBuffer x, MemoryBuffer y);
        public static void Subtract(MemoryBuffer z, MemoryBuffer x, MemoryBuffer y)
        {
            int err = _Subtract(z, x, y);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_Subtract", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _AddEqual(MemoryBuffer z, MemoryBuffer x, double alpha);
        public static void AddEqual(MemoryBuffer z, MemoryBuffer x, double alpha)
        {
            int err = _AddEqual(z, x, alpha);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_AddEqual", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _AddEqualMatrix(MemoryTile A, MemoryTile B, MatrixOperation aOperation, MatrixOperation bOperation, double alpha);
        public static void AddEqualMatrix(MemoryTile A, MemoryTile B, MatrixOperation aOperation, MatrixOperation bOperation, double alpha)
        {
            int err = _AddEqualMatrix(A, B, aOperation, bOperation, alpha);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_AddEqualMatrix", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _SubtractEqual(MemoryBuffer z, MemoryBuffer x);
        public static void SubtractEqual(MemoryBuffer z, MemoryBuffer x)
        {
            int err = _SubtractEqual(z, x);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_SubtractEqual", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _Scale(MemoryBuffer z, double alpha);
        public static void Scale(MemoryBuffer z, double alpha)
        {
            int err = _Scale(z, alpha);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_Scale", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _ElementwiseProduct(MemoryBuffer z, MemoryBuffer x, MemoryBuffer y, double alpha);
        public static void ElementwiseProduct(MemoryBuffer z, MemoryBuffer x, MemoryBuffer y, double alpha)
        {
            int err = _ElementwiseProduct(z, x, y, alpha);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_ElementwiseProduct", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _Multiply(MemoryTile A, MemoryTile B, MemoryTile C, int leadingDimensionB, int leadingDimensionC, MatrixOperation bOperation, MatrixOperation cOperation, double alpha);
        public static void Multiply(MemoryTile A, MemoryTile B, MemoryTile C, int leadingDimensionB, int leadingDimensionC, MatrixOperation bOperation, MatrixOperation cOperation, double alpha)
        {
            int err = _Multiply(A, B, C, leadingDimensionB, leadingDimensionC, bOperation, cOperation, alpha);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_Multiply", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _Dot(MemoryBuffer y, MemoryTile A, MemoryBuffer x, MatrixOperation aOperation, double alpha);
        public static void Dot(MemoryBuffer y, MemoryTile A, MemoryBuffer x, MatrixOperation aOperation, double alpha)
        {
            int err = _Dot(y, A, x, aOperation, alpha);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_Dot", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _CumulativeRowSum(MemoryTile A);
        public static void CumulativeRowSum(MemoryTile A)
        {
            int err = _CumulativeRowSum(A);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_CumulativeRowSum", err);
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
        private static extern unsafe int _Solve(MemoryTile A, MemoryTile B, MatrixOperation aOperation);
        public static void Solve(MemoryTile A, MemoryTile B, MatrixOperation aOperation)
        {
            int err = _Solve(A, B, aOperation);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_Solve", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _Invert(MemoryTile A, MatrixOperation aOperation);
        public static void Invert(MemoryTile A, MatrixOperation aOperation)
        {
            int err = _Invert(A, aOperation);
            if (err != 0)
                Exceptions.CuBlasKernelExceptionFactory.ThrowException("_Invert", err);
        }
    }
}