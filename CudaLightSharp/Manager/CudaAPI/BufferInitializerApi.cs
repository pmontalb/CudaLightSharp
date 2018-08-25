using CudaLightSharp.CudaStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CudaLightSharp.Manager.CudaAPI
{
    internal unsafe static class BufferInitializerApi
    {
        [DllImport("CudaKernels")]
        private static extern unsafe int _Initialize(MemoryBuffer buffer, double value);
        public static void Initialize(MemoryBuffer buffer, double value)
        {
            int err = _Initialize(buffer, value);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_Initialize", err);
        }

        [DllImport("CudaKernels")]
        private static extern unsafe int _LinSpace(MemoryBuffer buffer, double x0, double x1);
        public static void LinSpace(MemoryBuffer buffer, double x0, double x1)
        {
            int err = _LinSpace(buffer, x0, x1);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_LinSpace", err);
        }

        [DllImport("CudaKernels")]
        private static extern unsafe int _RandUniform(MemoryBuffer buffer, int seed);
        public static void RandUniform(MemoryBuffer buffer, int seed)
        {
            int err = _RandUniform(buffer, seed);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_RandUniform", err);
        }

        [DllImport("CudaKernels")]
        private static extern unsafe int _RandNormal(MemoryBuffer buffer, int seed);
        public static void RandNormal(MemoryBuffer buffer, int seed)
        {
            int err = _RandNormal(buffer, seed);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_RandNormal", err);
        }

    }
}
