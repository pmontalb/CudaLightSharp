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
    }
}
