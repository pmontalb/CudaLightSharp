using Microsoft.VisualStudio.TestTools.UnitTesting;
using CudaLightSharp.Buffers;
using CudaLightSharp.Manager;
using CudaLightSharp.CudaEnumerators;
using MathNet.Numerics.LinearAlgebra;
using System;

namespace UnitTests
{
    [TestClass]
    public class TensorTests
    {
        [TestMethod]
        public void Allocation()
        {
            var t1 = new Tensor(10, 5, 5, 1, MemorySpace.Device, MathDomain.Int);
            t1.Print();
            DeviceManager.CheckDeviceSanity();

            var t2 = new Tensor(10, 5, 4, 1.234f, MemorySpace.Device, MathDomain.Float);
            t2.Print();
            DeviceManager.CheckDeviceSanity();

            var t3 = new Tensor(10, 5, 3, 1.2345, MemorySpace.Device, MathDomain.Double);
            t3.Print();
            DeviceManager.CheckDeviceSanity();

            var t4 = new Tensor(10, 5, 7, 1, MemorySpace.Host, MathDomain.Int);
            t4.Print();
            DeviceManager.CheckDeviceSanity();

            var t5 = new Tensor(10, 5, 1, 1.234f, MemorySpace.Host, MathDomain.Float);
            t5.Print();
            DeviceManager.CheckDeviceSanity();

            var t6 = new Tensor(10, 5, 10, 1.2345, MemorySpace.Host, MathDomain.Double);
            t6.Print();
            DeviceManager.CheckDeviceSanity();
        }

        [TestMethod]
        public void Copy()
        {
            var t1 = new Tensor(10, 5, 5, 1, MemorySpace.Device, MathDomain.Int);
            t1.Print();
            var t1Copy = new Tensor(t1);
            DeviceManager.CheckDeviceSanity();
            t1Copy.Print();
            Assert.AreEqual(t1, t1Copy);

            var t2 = new Tensor(10, 5, 5, 1.234f, MemorySpace.Device, MathDomain.Float);
            t2.Print();
            var t2Copy = new Tensor(t2);
            DeviceManager.CheckDeviceSanity();
            t2Copy.Print();
            Assert.AreEqual(t2, t2Copy);

            var t3 = new Tensor(10, 5, 5, 1.2345, MemorySpace.Device, MathDomain.Double);
            t3.Print();
            var t3Copy = new Tensor(t3);
            DeviceManager.CheckDeviceSanity();
            t3Copy.Print();
            Assert.AreEqual(t3, t3Copy);

            var t4 = new Tensor(10, 5, 5, 1, MemorySpace.Host, MathDomain.Int);
            t4.Print();
            var t4Copy = new Tensor(t4);
            DeviceManager.CheckDeviceSanity();
            t4Copy.Print();
            Assert.AreEqual(t4, t4Copy);

            var t5 = new Tensor(10, 5, 5, 1.234f, MemorySpace.Host, MathDomain.Float);
            t5.Print();
            var t5Copy = new Tensor(t5);
            DeviceManager.CheckDeviceSanity();
            t5Copy.Print();
            Assert.AreEqual(t5, t5Copy);

            var t6 = new Tensor(10, 5, 5, 1.2345, MemorySpace.Host, MathDomain.Double);
            t6.Print();
            var t6Copy = new Tensor(t6);
            DeviceManager.CheckDeviceSanity();
            t6Copy.Print();
            Assert.AreEqual(t6, t6Copy);
        }

        [TestMethod]
        public void LinSpace()
        {
            var t = Tensor.LinSpace(10, 5, 5, 0.0, 1.0);
            DeviceManager.CheckDeviceSanity();

            var _t = t.Get<float>();
            Assert.IsTrue(System.Math.Abs(_t[0] - 0.0) <= 1e-7);
            Assert.IsTrue(System.Math.Abs(_t[_t.Count - 1] - 1.0) <= 1e-7);
        }

        [TestMethod]
        public void RandomUniform()
        {
            var t = Tensor.RandomUniform(10, 5, 5, 1234);
            DeviceManager.CheckDeviceSanity();

            var _t = t.Get<float>();
            foreach (var iter in _t)
                Assert.IsTrue(iter >= 0.0 && iter <= 1.0);
        }

        [TestMethod]
        public void RandomGaussian()
        {
            var t = Tensor.RandomGaussian(10, 5, 5, 1234);
            DeviceManager.CheckDeviceSanity();

            var _t = t.Get<float>();
            for (int i = 0; i < _t.Count / 2; ++i)
                Assert.IsTrue(System.Math.Abs(_t[2 * i] + _t[2 * i + 1]) <= 1e-7);
        }
    }
}
