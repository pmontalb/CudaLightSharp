﻿using System;
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

        [TestMethod]
        public void AddMatrix()
        {
            ColumnWiseMatrix m1 = ColumnWiseMatrix.LinSpace(100, 20, -1.0, 1.0);
            DeviceManager.CheckDeviceSanity();
            var _m1 = m1.GetMatrix<float>();

            ColumnWiseMatrix m2 = ColumnWiseMatrix.RandomGaussian(m1.nRows, m1.nCols, 1234);
            DeviceManager.CheckDeviceSanity();
            var _m2 = m2.GetMatrix<float>();

            var m3 = m1 + m2;
            DeviceManager.CheckDeviceSanity();
            var _m3 = m3.GetMatrix<float>();

            for (int i = 0; i < m1.nRows; ++i)
                for (int j = 0; j < m1.nCols; ++j)
                    Assert.IsTrue(Math.Abs(_m3[i, j] - _m1[i, j] - _m2[i, j]) <= 3e-7, String.Format("i({0}) j({1}) err({2})", i, j, Math.Abs(_m3[i, j] - _m1[i, j] - _m2[i, j])));

            var m4 = ColumnWiseMatrix.Add(m1, m2, alpha: 2.0);
            DeviceManager.CheckDeviceSanity();
            var _m4 = m4.GetMatrix<float>();

            for (int i = 0; i < m1.nRows; ++i)
                for (int j = 0; j < m1.nCols; ++j)
                    Assert.IsTrue(Math.Abs(_m4[i, j] - _m1[i, j] - 2.0 * _m2[i, j]) <= 5e-7, String.Format("i({0}) j({1}) err({2})", i, j, Math.Abs(_m4[i, j] - _m1[i, j] - 2.0 * _m2[i, j])));
        }

        [TestMethod]
        public void Scale()
        {
            var v1 = Vector.LinSpace(100, -1.0, 1.0);
            DeviceManager.CheckDeviceSanity();
            var _v1 = v1.Get<float>();

            v1.Scale(2.0);
            var _v2 = v1.Get<float>();

            for (int i = 0; i < v1.Size; ++i)
                Assert.IsTrue(Math.Abs(2.0 * _v1[i] - _v2[i]) <= 1e-7);
        }

        [TestMethod]
        public void ElementWiseProduct()
        {
            var v1 = Vector.LinSpace(100, -1.0, 1.0);
            DeviceManager.CheckDeviceSanity();
            var _v1 = v1.Get<float>();

            var v2 = Vector.LinSpace(100, -2.0, 2.0);
            DeviceManager.CheckDeviceSanity();
            var _v2 = v2.Get<float>();

            var v3 = v1 % v2;
            var _v3 = v3.Get<float>();

            for (int i = 0; i < v1.Size; ++i)
                Assert.IsTrue(Math.Abs(_v3[i] - _v1[i] * _v2[i]) <= 1e-7);
        }

        [TestMethod]
        public void Multiply()
        {
            ColumnWiseMatrix m1 = new ColumnWiseMatrix(10, 10, 1.2345f);
            DeviceManager.CheckDeviceSanity();
            var _m1 = m1.GetMatrix<float>();

            ColumnWiseMatrix m2 = new ColumnWiseMatrix(10, 10, 9.8765f);
            DeviceManager.CheckDeviceSanity();
            var _m2 = m2.GetMatrix<float>();

            var m3 = m1 * m2;
            DeviceManager.CheckDeviceSanity();
            var _m3 = m3.GetMatrix<float>();

            for (int i = 0; i < m1.nRows; ++i)
            {
                for (int j = 0; j < m1.nCols; ++j)
                {
                    double m1m2 = 0.0;
                    for (int k = 0; k < m1.nCols; ++k)
                        m1m2 += _m1[i, k] * _m2[k, j];
                    Assert.IsTrue(Math.Abs(m1m2 - _m3[i, j]) <= 5e-5);
                }
            }
        }
    }
}