using CudaLightSharp.CudaEnumerators;
using CudaLightSharp.CudaStructures;
using System.Runtime.InteropServices;

#if FORCE_32_BIT
    using PtrT = System.UInt32;
#else
    using PtrT = System.UInt64;
#endif

namespace CudaLightSharp.Manager.CudaAPI
{
    internal unsafe static class CubApi
    {
        [DllImport("CudaLightKernels")]
        private static extern unsafe int _SumRaw(ref double sum, PtrT v, uint size, MemorySpace memorySpace, MathDomain mathDomain);
        public static double Sum(MemoryBuffer vec)
        {
            double sum = 0.0;
            int err = _SumRaw(ref sum, vec.pointer, vec.size, vec.memorySpace, vec.mathDomain);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_SumRaw", err);

            return sum;
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _MinRaw(ref double min, PtrT v, uint size, MemorySpace memorySpace, MathDomain mathDomain);
        public static double Min(MemoryBuffer vec)
        {
            double min = 0.0;
            int err = _MinRaw(ref min, vec.pointer, vec.size, vec.memorySpace, vec.mathDomain);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_MinRaw", err);

            return min;
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _MaxRaw(ref double min, PtrT v, uint size, MemorySpace memorySpace, MathDomain mathDomain);
        public static double Max(MemoryBuffer vec)
        {
            double max = 0.0;
            int err = _MaxRaw(ref max, vec.pointer, vec.size, vec.memorySpace, vec.mathDomain);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_MaxRaw", err);

            return max;
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _AbsMinRaw(ref double min, PtrT v, uint size, MemorySpace memorySpace, MathDomain mathDomain);
        public static double AbsMin(MemoryBuffer vec)
        {
            double min = 0.0;
            int err = _AbsMinRaw(ref min, vec.pointer, vec.size, vec.memorySpace, vec.mathDomain);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_AbsMinRaw", err);

            return min;
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _AbsMaxRaw(ref double min, PtrT v, uint size, MemorySpace memorySpace, MathDomain mathDomain);
        public static double AbsMax(MemoryBuffer vec)
        {
            double max = 0.0;
            int err = _AbsMaxRaw(ref max, vec.pointer, vec.size, vec.memorySpace, vec.mathDomain);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_AbsMaxRaw", err);

            return max;
        }
    }
}
