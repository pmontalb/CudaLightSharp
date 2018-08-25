using CudaLightSharp.Manager.CudaAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CudaLightSharp.Manager
{
    internal static class DeviceManager
    {
        public static int GetDeviceCount()
        {
            return DeviceApi.GetDeviceCount();
        }

        public static void SetDevice(int i)
        {
            DeviceApi.SetDevice(i);
            CheckDeviceSanity();
        }

        public static void SetBestDevice()
        {
            SetDevice(DeviceApi.GetBestDevice());
        }

        public static void CheckDeviceSanity()
        {
            DeviceApi.ThreadSynchronize();
            int err = DeviceApi.GetDeviceStatus();
            if (err != 0)
                throw new InvalidProgramException("GPU is in a non-valid state: " + err);
        }
    }
}
