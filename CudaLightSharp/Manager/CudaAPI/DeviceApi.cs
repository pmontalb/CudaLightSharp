using CudaLightSharp.CudaStructures;
using System.Runtime.InteropServices;

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
        private static extern unsafe int _GetDeviceProperties(ref CudaDeviceProperties props, int dev);
        public static void GetDeviceProperties(ref CudaDeviceProperties props, int dev)
        {
            int err = _GetDeviceProperties(ref props, dev);
            if (err != 0)
                Exceptions.CudaKernelExceptionFactory.ThrowException("_GetDeviceProperties", err);
        }
    }
}
