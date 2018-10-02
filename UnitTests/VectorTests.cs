using Microsoft.VisualStudio.TestTools.UnitTesting;
using CudaLightSharp.Buffers;
using CudaLightSharp.Manager;
using CudaLightSharp.CudaEnumerators;

namespace UnitTests
{
    [TestClass]
    public class VectorTests
    {
        [TestMethod]
        public void Allocation()
        {
            var v1 = new Vector(10, 1, MemorySpace.Device, MathDomain.Int);
            v1.Print();
            DeviceManager.CheckDeviceSanity();

            var v2 = new Vector(10, 1.234f, MemorySpace.Device, MathDomain.Float);
            v2.Print();
            DeviceManager.CheckDeviceSanity();

            var v3 = new Vector(10, 1.2345, MemorySpace.Device, MathDomain.Double);
            v3.Print();
            DeviceManager.CheckDeviceSanity();

            var v4 = new Vector(10, 1, MemorySpace.Host, MathDomain.Int);
            v4.Print();
            DeviceManager.CheckDeviceSanity();

            var v5 = new Vector(10, 1.234f, MemorySpace.Host, MathDomain.Float);
            v5.Print();
            DeviceManager.CheckDeviceSanity();

            var v6 = new Vector(10, 1.2345, MemorySpace.Host, MathDomain.Double);
            v6.Print();
            DeviceManager.CheckDeviceSanity();
        }

        [TestMethod]
        public void Copy()
        {
            var v1 = new Vector(10, 1, MemorySpace.Device, MathDomain.Int);
            v1.Print();
            var v1Copy = new Vector(v1);
            DeviceManager.CheckDeviceSanity();
            v1Copy.Print();
            Assert.AreEqual(v1, v1Copy);

            var v2 = new Vector(10, 1.234f, MemorySpace.Device, MathDomain.Float);
            v2.Print();
            var v2Copy = new Vector(v2);
            DeviceManager.CheckDeviceSanity();
            v2Copy.Print();
            Assert.AreEqual(v2, v2Copy);

            var v3 = new Vector(10, 1.2345, MemorySpace.Device, MathDomain.Double);
            v3.Print();
            var v3Copy = new Vector(v3);
            DeviceManager.CheckDeviceSanity();
            v3Copy.Print();
            Assert.AreEqual(v3, v3Copy);

            var v4 = new Vector(10, 1, MemorySpace.Host, MathDomain.Int);
            v4.Print();
            var v4Copy = new Vector(v4);
            DeviceManager.CheckDeviceSanity();
            v4Copy.Print();
            Assert.AreEqual(v4, v4Copy);

            var v5 = new Vector(10, 1.234f, MemorySpace.Host, MathDomain.Float);
            v5.Print();
            var v5Copy = new Vector(v5);
            DeviceManager.CheckDeviceSanity();
            v5Copy.Print();
            Assert.AreEqual(v5, v5Copy);

            var v6 = new Vector(10, 1.2345, MemorySpace.Host, MathDomain.Double);
            v6.Print();
            var v6Copy = new Vector(v6);
            DeviceManager.CheckDeviceSanity();
            v6Copy.Print();
            Assert.AreEqual(v6, v6Copy);
        }

        [TestMethod]
        public void LinSpace()
        {
            var v = Vector.LinSpace(10, 0.0, 1.0);
            DeviceManager.CheckDeviceSanity();

            var _v = v.Get<float>();
            Assert.IsTrue(System.Math.Abs(_v[0] - 0.0) <= 1e-7);
            Assert.IsTrue(System.Math.Abs(_v[_v.Count - 1] - 1.0) <= 1e-7);
        }

        [TestMethod]
        public void RandomUniform()
        {
            var v = Vector.RandomUniform(11, 1234);
            DeviceManager.CheckDeviceSanity();

            var _v = v.Get<float>();
            foreach (var iter in _v)
                Assert.IsTrue(iter >= 0.0 && iter <= 1.0);
        }

        [TestMethod]
        public void RandomGaussian()
        {
            var v = Vector.RandomGaussian(11, 1234);
            DeviceManager.CheckDeviceSanity();

            var _v = v.Get<float>();
            for (int i = 0; i < _v.Count / 2; ++i)
                Assert.IsTrue(System.Math.Abs(_v[2 * i] + _v[2 * i + 1]) <= 1e-7);
        }
    }
}
