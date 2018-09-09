using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using CudaLightSharp.Buffers;
using CudaLightSharp.Manager;

namespace UnitTests
{
    [TestClass]
    public class CuBlasTests
    {
        static ColumnWiseMatrix GetInvertibleMatrix(int nRows, int seed = 1234)
        {
            var A = ColumnWiseMatrix.RandomUniform(nRows, nRows, seed);
            var _A = A.GetMatrix<float>();

            for (int i = 0; i < nRows; ++i)
                _A[i, i] += 2;

            A.ReadFrom(_A);
            return A;
        }

        [TestMethod]
        public void Add()
        {
            Vector v1 = Vector.LinSpace(100, -1.0, 1.0);
            DeviceManager.CheckDeviceSanity();
            var _v1 = v1.Get<float>();

            Vector v2 = Vector.RandomGaussian(v1.Size, 1234);
            DeviceManager.CheckDeviceSanity();
            var _v2 = v2.Get<float>();

            var v3 = v1 + v2;
            DeviceManager.CheckDeviceSanity();
            var _v3 = v3.Get<float>();

            for (int i = 0; i < v1.Size; ++i)
                Assert.IsTrue(Math.Abs(_v3[i] - _v1[i] - _v2[i]) <= 3e-7, String.Format("i({0}) err({1})", i, Math.Abs(_v3[i] - _v1[i] - _v2[i])));

            var v4 = Vector.Add(v1, v2, 2.0);
            DeviceManager.CheckDeviceSanity();
            var _v4 = v4.Get<float>();

            for (int i = 0; i < v1.Size; ++i)
                Assert.IsTrue(Math.Abs(_v4[i] - _v1[i] - 2.0 * _v2[i]) <= 5e-7, String.Format("i({0}) err({1})", i, Math.Abs(_v4[i] - _v1[i] - 2.0 * _v2[i])));
        }
    }
}