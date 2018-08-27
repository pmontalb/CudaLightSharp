using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CudaLightSharp.Manager;

namespace UnitTests
{
    [TestClass]
    public class DeviceManagerTests
    {
        [TestMethod]
        public void HasAtLeastOneDevice()
        {
            Assert.IsTrue(DeviceManager.GetDeviceCount() > 0);
            DeviceManager.CheckDeviceSanity();
        }

        [TestMethod]
        public void DeviceInitialization()
        {
            for (int i = 0; i < DeviceManager.GetDeviceCount(); ++i)
            {
                DeviceManager.SetDevice(i);
                DeviceManager.CheckDeviceSanity();
            }

            DeviceManager.SetBestDevice();
            DeviceManager.CheckDeviceSanity();
        }

        [TestMethod]
        public void DeviceProperties()
        {
            for (int i = 0; i < DeviceManager.GetDeviceCount(); ++i)
            {
                var dp = DeviceManager.GetDeviceProperties(i);
                Assert.IsTrue(dp.major > 0);
                Assert.IsTrue(dp.clockRate > 0);
                Assert.IsTrue(dp.warpSize > 0);
            }

            DeviceManager.SetBestDevice();
            DeviceManager.CheckDeviceSanity();
        }
    }
}
