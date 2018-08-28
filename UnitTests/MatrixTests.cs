using Microsoft.VisualStudio.TestTools.UnitTesting;
using CudaLightSharp.Buffers;
using CudaLightSharp.Manager;
using CudaLightSharp.CudaEnumerators;
using MathNet.Numerics.LinearAlgebra;

namespace UnitTests
{
    [TestClass]
    public class MatrixTests
    {
        [TestMethod]
        public void Allocation()
        {
            var m1 = new ColumnWiseMatrix(10, 5, 2, MemorySpace.Device, MathDomain.Int);
            m1.Print();
            DeviceManager.CheckDeviceSanity();

            var m2 = new ColumnWiseMatrix(10, 5, 1.234f, MemorySpace.Device, MathDomain.Float);
            m2.Print();
            DeviceManager.CheckDeviceSanity();

            var m3 = new ColumnWiseMatrix(10, 5, 1.2345, MemorySpace.Device, MathDomain.Double);
            m3.Print();
            DeviceManager.CheckDeviceSanity();

            var m4 = new ColumnWiseMatrix(10, 5, 1, MemorySpace.Host, MathDomain.Int);
            m4.Print();
            DeviceManager.CheckDeviceSanity();

            var m5 = new ColumnWiseMatrix(10, 5, 1.234f, MemorySpace.Host, MathDomain.Float);
            m5.Print();
            DeviceManager.CheckDeviceSanity();

            var m6 = new ColumnWiseMatrix(10, 5, 1.2345, MemorySpace.Host, MathDomain.Double);
            m6.Print();
            DeviceManager.CheckDeviceSanity();
        }

        [TestMethod]
        public void Copy()
        {
            var m1 = new ColumnWiseMatrix(10, 5, 1, MemorySpace.Device, MathDomain.Int);
            m1.Print();
            var m1Copy = new ColumnWiseMatrix(m1);
            DeviceManager.CheckDeviceSanity();
            m1Copy.Print();
            Assert.AreEqual(m1, m1Copy);

            var m2 = new ColumnWiseMatrix(10, 5, 1.234f, MemorySpace.Device, MathDomain.Float);
            m2.Print();
            var m2Copy = new ColumnWiseMatrix(m2);
            DeviceManager.CheckDeviceSanity();
            m2Copy.Print();
            Assert.AreEqual(m2, m2Copy);

            var m3 = new ColumnWiseMatrix(10, 5, 1.2345, MemorySpace.Device, MathDomain.Double);
            m3.Print();
            var m3Copy = new ColumnWiseMatrix(m3);
            DeviceManager.CheckDeviceSanity();
            m3Copy.Print();
            Assert.AreEqual(m3, m3Copy);

            var m4 = new ColumnWiseMatrix(10, 5, 1, MemorySpace.Host, MathDomain.Int);
            m4.Print();
            var m4Copy = new ColumnWiseMatrix(m4);
            DeviceManager.CheckDeviceSanity();
            m4Copy.Print();
            Assert.AreEqual(m4, m4Copy);

            var m5 = new ColumnWiseMatrix(10, 5, 1.234f, MemorySpace.Host, MathDomain.Float);
            m5.Print();
            var m5Copy = new ColumnWiseMatrix(m5);
            DeviceManager.CheckDeviceSanity();
            m5Copy.Print();
            Assert.AreEqual(m5, m5Copy);

            var m6 = new ColumnWiseMatrix(10, 5, 1.2345, MemorySpace.Host, MathDomain.Double);
            m6.Print();
            var m6Copy = new ColumnWiseMatrix(m6);
            DeviceManager.CheckDeviceSanity();
            m6Copy.Print();
            Assert.AreEqual(m6, m6Copy);
        }

        [TestMethod]
        public void Eye()
        {
            var m = ColumnWiseMatrix.Eye(10);
            m.Print();
            DeviceManager.CheckDeviceSanity();

            var _m = m.GetMatrix<float>();
            for (int i = 0; i < m.nRows; ++i)
            {
                for (int j = 0; j < m.nCols; ++j)
                    Assert.AreEqual(j == i ? 1.0 : 0.0, _m[i, j]);
            }
        }

        [TestMethod]
        public void LinSpace()
        {
            var m = ColumnWiseMatrix.LinSpace(10, 5, 0.0, 1.0);
            DeviceManager.CheckDeviceSanity();

            var _m = m.Get<float>();
            Assert.IsTrue(System.Math.Abs(_m[0] - 0.0) <= 1e-7);
            Assert.IsTrue(System.Math.Abs(_m[_m.Count - 1] - 1.0) <= 1e-7);
        }

        [TestMethod]
        public void RandomUniform()
        {
            var m = ColumnWiseMatrix.RandomUniform(10, 5, 1234);
            DeviceManager.CheckDeviceSanity();

            var _m = m.Get<float>();
            foreach (var iter in _m)
                Assert.IsTrue(iter >= 0.0 && iter <= 1.0);
        }

        [TestMethod]
        public void RandomGaussian()
        {
            var m = ColumnWiseMatrix.RandomGaussian(10, 5, 1234);
            DeviceManager.CheckDeviceSanity();

            var _m = m.Get<float>();
            for (int i = 0; i < _m.Count / 2; ++i)
                Assert.IsTrue(System.Math.Abs(_m[2 * i] + _m[2 * i + 1]) <= 1e-7);
        }
    }
}
