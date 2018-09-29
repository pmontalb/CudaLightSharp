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
    internal unsafe static class BufferInitializerApi
    {
        [DllImport("CudaLightKernels")]
        private static extern unsafe int _InitializeRaw(PtrT pointer, uint size, MemorySpace memorySpace, MathDomain mathDomain, double value);
        public static void Initialize(MemoryBuffer buffer, double value)
        {
            int err = _InitializeRaw(buffer.pointer, buffer.size, buffer.memorySpace, buffer.mathDomain, value);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_InitializeRaw", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _LinSpaceRaw(PtrT pointer, uint size, MemorySpace memorySpace, MathDomain mathDomain, double x0, double x1);
        public static void LinSpace(MemoryBuffer buffer, double x0, double x1)
        {
            int err = _LinSpaceRaw(buffer.pointer, buffer.size, buffer.memorySpace, buffer.mathDomain, x0, x1);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_LinSpace", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _RandUniformRaw(PtrT pointer, uint size, MemorySpace memorySpace, MathDomain mathDomain, int seed);
        public static void RandUniform(MemoryBuffer buffer, int seed)
        {
            int err = _RandUniformRaw(buffer.pointer, buffer.size, buffer.memorySpace, buffer.mathDomain, seed);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_RandUniformRaw", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _RandNormalRaw(PtrT pointer, uint size, MemorySpace memorySpace, MathDomain mathDomain, int seed);
        public static void RandNormal(MemoryBuffer buffer, int seed)
        {
            int err = _RandNormalRaw(buffer.pointer, buffer.size, buffer.memorySpace, buffer.mathDomain, seed);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_RandNormalRaw", err);
        }

    }
}
