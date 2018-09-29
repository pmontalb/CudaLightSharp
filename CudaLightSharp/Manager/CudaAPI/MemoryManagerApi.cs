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
    internal unsafe static class MemoryManagerApi
    {
        [DllImport("CudaLightKernels")]
        private static extern unsafe int _HostToHostCopyRaw(PtrT destPointer, PtrT sourcePointer, uint size, MemorySpace memorySpace, MathDomain mathDomain);
        public static void HostToHostCopy(MemoryBuffer dest, MemoryBuffer source)
        {
            int err = _HostToHostCopyRaw(dest.pointer, source.pointer, dest.size, dest.memorySpace, dest.mathDomain);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_HostToHostCopyRaw", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _HostToDeviceCopyRaw(PtrT destPointer, PtrT sourcePointer, uint size, MemorySpace memorySpace, MathDomain mathDomain);
        public static void HostToDeviceCopy(MemoryBuffer dest, MemoryBuffer source)
        {
            int err = _HostToDeviceCopyRaw(dest.pointer, source.pointer, dest.size, dest.memorySpace, dest.mathDomain);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_HostToDeviceCopyRaw", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _DeviceToHostCopyRaw(PtrT destPointer, PtrT sourcePointer, uint size, MemorySpace memorySpace, MathDomain mathDomain);
        public static void DeviceToDeviceCopy(MemoryBuffer dest, MemoryBuffer source)
        {
            int err = _DeviceToHostCopyRaw(dest.pointer, source.pointer, dest.size, dest.memorySpace, dest.mathDomain);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_DeviceToDeviceCopyRaw", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _AutoCopyRaw(PtrT destPointer, PtrT sourcePointer, uint size, MemorySpace memorySpace, MathDomain mathDomain);
        public static void AutoCopy(MemoryBuffer dest, MemoryBuffer source)
        {
            int err = _AutoCopyRaw(dest.pointer, source.pointer, dest.size, dest.memorySpace, dest.mathDomain);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_AutoCopyRaw", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _AllocRaw(ref PtrT pointer, uint size, MemorySpace memorySpace, MathDomain mathDomain);
        public static void Alloc(MemoryBuffer buffer)
        {
            int err = _AllocRaw(ref buffer.pointer, buffer.size, buffer.memorySpace, buffer.mathDomain);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_AllocRaw", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _AllocHostRaw(ref PtrT pointer, uint size, MemorySpace memorySpace, MathDomain mathDomain);
        public static void AllocHost(MemoryBuffer buffer)
        {
            int err = _AllocHostRaw(ref buffer.pointer, buffer.size, buffer.memorySpace, buffer.mathDomain);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_AllocHost", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _FreeRaw(PtrT pointer, uint size, MemorySpace memorySpace, MathDomain mathDomain);
        public static void Free(MemoryBuffer buffer)
        {
            int err = _FreeRaw(buffer.pointer, buffer.size, buffer.memorySpace, buffer.mathDomain);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("Free", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _FreeHostRaw(PtrT pointer, uint size, MemorySpace memorySpace, MathDomain mathDomain);
        public static void FreeHost(MemoryBuffer buffer)
        {
            int err = _FreeHostRaw(buffer.pointer, buffer.size, buffer.memorySpace, buffer.mathDomain);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("Free", err);
        }
    }
}
