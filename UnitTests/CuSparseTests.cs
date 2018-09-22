using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using CudaLightSharp.Buffers;
using CudaLightSharp.Manager;
using CudaLightSharp.SparseBuffers;
using CudaLightSharp.CudaEnumerators;

namespace UnitTests
{
    [TestClass]
    public class CuSparseTests
    {
        [TestMethod]
        public void Add()
        {
            int[] indices = new int[] { 0, 5, 10, 50, 75 };
            Vector gpuIndices = new Vector(indices);

            SparseVector v1 = new SparseVector(100, gpuIndices, 1.2345, MathDomain.Float);
            DeviceManager.CheckDeviceSanity();
            var _v1 = v1.Get<float>();

            Vector v2 = Vector.RandomUniform(v1.denseSize, 1234, MemorySpace.Device, MathDomain.Float);
            DeviceManager.CheckDeviceSanity();
            var _v2 = v2.Get<float>();

            var v3 = v1 + v2;
            DeviceManager.CheckDeviceSanity();
            var _v3 = v3.Get<float>();

            for (int i = 0; i < v1.Size; ++i)
                Assert.IsTrue(Math.Abs(_v3[i] - _v1[i] - _v2[i]) <= 1e-7);
        }

        [TestMethod]
        public void Multiply()
        {
            int[] _NonZeroCols = new int[] { 0, 1, 1, 3, 2, 3, 4, 5 };
            Vector gpuNonZeroCols = new Vector(_NonZeroCols);

            int[] _NonZeroRows = new int[] { 0, 2, 4, 7, 8 };
            Vector gpuNonZeroRows = new Vector(_NonZeroRows);

            CompressedSparseRowMatrix m1 = new CompressedSparseRowMatrix(4, 6, gpuNonZeroCols, gpuNonZeroRows, 1.2345f, MathDomain.Float);
            DeviceManager.CheckDeviceSanity();
            var _m1 = m1.GetMatrix<float>();

            ColumnWiseMatrix m2 = new ColumnWiseMatrix(6, 8, 9.8765f, MemorySpace.Device, MathDomain.Float);
            DeviceManager.CheckDeviceSanity();
            var _m2 = m2.GetMatrix<float>();

            var m3 = m1 * m2;
            DeviceManager.CheckDeviceSanity();
            var _m3 = m3.GetMatrix<float>();

            for (int i = 0; i < m1.nRows; ++i)
            {
                for (int j = 0; j < m2.nCols; ++j)
                {
                    double m1m2 = 0.0;
                    for (int k = 0; k < m1.nCols; ++k)
                        m1m2 += _m1[i , k] * _m2[k, j];
                    Assert.IsTrue(Math.Abs(m1m2 - _m3[i, j]) <= 5e-5);
                }
            }
        }
    }
}