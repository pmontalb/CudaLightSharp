using CudaLightSharp.CudaStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CudaLightSharp.Manager.CudaAPI
{
    internal unsafe static class DeviceApi
    {
        [DllImport("CudaLightKernels")]
        private static extern unsafe int _GetDevice(ref int device);
        public static int GetDevice()
        {
            int dev = -1;
            int err = _GetDevice(ref dev);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_GetDevice", err);

            return dev;
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _ThreadSynchronize();
        public static void ThreadSynchronize()
        {
            int err = _ThreadSynchronize();
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_ThreadSynchronize", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _SetDevice(int device);
        public static void SetDevice(int device)
        {
            int err = _SetDevice(device);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_SetDevice", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _GetDeviceStatus();
        public static int GetDeviceStatus()
        {
            int err = _GetDeviceStatus();
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_GetDeviceStatus", err);

            return err;
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _GetBestDevice(ref int dev);
        public static int GetBestDevice()
        {
            int dev = -1;
            int err = _GetBestDevice(ref dev);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_GetBestDevice", err);

            return dev;
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _GetDeviceCount(ref int count);
        public static int GetDeviceCount()
        {
            int count = 0;
            int err = _GetDeviceCount(ref count);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_GetDeviceCount", err);

            return count;
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _HostToHostCopy(MemoryBuffer dest, MemoryBuffer source);
        public static void HostToHostCopy(MemoryBuffer dest, MemoryBuffer source)
        {
            int err = _HostToHostCopy(dest, source);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_HostToHostCopy", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _HostToDeviceCopy(MemoryBuffer dest, MemoryBuffer source);
        public static void HostToDeviceCopy(MemoryBuffer dest, MemoryBuffer source)
        {
            int err = _HostToDeviceCopy(dest, source);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_HostToDeviceCopy", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _DeviceToDeviceCopy(MemoryBuffer dest, MemoryBuffer source);
        public static void DeviceToDeviceCopy(MemoryBuffer dest, MemoryBuffer source)
        {
            int err = _DeviceToDeviceCopy(dest, source);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_DeviceToDeviceCopy", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _AutoCopy(MemoryBuffer dest, MemoryBuffer source);
        public static void AutoCopy(MemoryBuffer dest, MemoryBuffer source)
        {
            int err = _AutoCopy(dest, source);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_AutoCopy", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _Alloc(ref MemoryBuffer buffer);
        public static void Alloc(ref MemoryBuffer buffer)
        {
            int err = _Alloc(ref buffer);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_Alloc", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _AllocHost(ref MemoryBuffer buffer);
        public static void AllocHost(ref MemoryBuffer buffer)
        {
            int err = _AllocHost(ref buffer);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_AllocHost", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _Free(MemoryBuffer buffer);
        public static void Free(MemoryBuffer buffer)
        {
            int err = _Free(buffer);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("Free", err);
        }

        [DllImport("CudaLightKernels")]
        private static extern unsafe int _FreeHost(MemoryBuffer buffer);
        public static void FreeHost(MemoryBuffer buffer)
        {
            int err = _FreeHost(buffer);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("Free", err);
        }
    }
}
