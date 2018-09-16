using Microsoft.VisualStudio.TestTools.UnitTesting;
using CudaLightSharp.Buffers;
using CudaLightSharp.Manager;
using CudaLightSharp.CudaEnumerators;
using CudaLightSharp.SparseBuffers;
using System;

namespace UnitTests
{
    [TestClass]
    public class SparseVectorTests
    {
        [TestMethod]
        public void Allocation()
        {
            int[] indices = new int[]{ 0, 5 };
            Vector gpuIndices = new Vector(indices);

            SparseVector v1 = new SparseVector(10, gpuIndices, 1.2345f, MathDomain.Float);
            DeviceManager.CheckDeviceSanity();

            SparseVector v2 = new SparseVector(10, gpuIndices, 1.2345, MathDomain.Double);
            DeviceManager.CheckDeviceSanity();

            SparseVector v3 = new SparseVector(10, gpuIndices, 1, MathDomain.Int);
            DeviceManager.CheckDeviceSanity();
        }

        [TestMethod]
        public void Copy()
        {
            int[] indices = new int[] { 0, 5 };
            Vector gpuIndices = new Vector(indices);

            SparseVector v1 = new SparseVector(10, gpuIndices, 1.2345f, MathDomain.Float);
            DeviceManager.CheckDeviceSanity();

            SparseVector v2 = new SparseVector(v1);
            DeviceManager.CheckDeviceSanity();

            Assert.AreEqual(v1, v2);

            SparseVector v3 = new SparseVector(10, gpuIndices, 1.2345, MathDomain.Double);
            DeviceManager.CheckDeviceSanity();

            SparseVector v4 = new SparseVector(v3);
            DeviceManager.CheckDeviceSanity();

            Assert.AreEqual(v3, v4);

            SparseVector v5 = new SparseVector(10, gpuIndices, 10, MathDomain.Int);
            DeviceManager.CheckDeviceSanity();

            SparseVector v6 = new SparseVector(v5);
            DeviceManager.CheckDeviceSanity();

            Assert.AreEqual(v5, v6);
        }

        [TestMethod]
        public void ReadFromDense()
        {
            float[] denseVector = new float[50];
            denseVector[10] = 2.7182f;
            denseVector[20] = 3.1415f;
            denseVector[30] = 1.6180f;

            Vector dv = new Vector(denseVector);
            SparseVector sv = new SparseVector(dv);

            var _dv = dv.Get<float>();
            var _sv = dv.Get<float>();
            Assert.AreEqual(_dv.Count, _sv.Count);

            for (int i = 0; i < _dv.Count; ++i)
            {
                Assert.IsTrue(Math.Abs(_dv[i] - _sv[i]) <= 1e-7);
            }
        }
    }
}
