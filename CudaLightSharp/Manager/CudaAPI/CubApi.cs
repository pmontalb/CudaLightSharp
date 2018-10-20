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
                Exceptions.CudaKernelExceptionFactory.ThrowException("_CountEqualsRaw", err);

            return sum;
        }
    }
}
