using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using CudaLightSharp.Buffers;
using CudaLightSharp.Manager;
using CudaLightSharp.CudaEnumerators;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class CubTests
    {
        [TestMethod]
        public void AbsoluteMinMax()
        {
            Vector x = Vector.LinSpace(128, -1, 1);
            var _x = x.Get<float>();

            double min = x.AbsoluteMinimum();
            double max = x.AbsoluteMaximum();

            double _min = 1e9, _max = 0.0;
            for (int i = 0; i < x.Size; i++)
            {
                if (Math.Abs(_x[i]) < Math.Abs(_min))
                    _min = Math.Abs(_x[i]);
                if (Math.Abs(_x[i]) > Math.Abs(_max))
                    _max = Math.Abs(_x[i]);
            }

            Assert.IsTrue(Math.Abs(min - _min) <= 1e-7);
            Assert.IsTrue(Math.Abs(max - _max) <= 1e-7);
        }

        [TestMethod]
        public void MinMax()
        {
            Vector x = Vector.LinSpace(128, -1, 1);
            var _x = x.Get<float>();

            double min = x.Minimum();
            double max = x.Maximum();

            double _min = 1e9, _max = 0.0;
            for (int i = 0; i < x.Size; i++)
            {
                if (Math.Abs(_x[i]) < _min)
                    _min = _x[i];
                if (Math.Abs(_x[i]) > _max)
                    _max = _x[i];
            }

            Assert.IsTrue(Math.Abs(min - _min) <= 1e-7);
            Assert.IsTrue(Math.Abs(max - _max) <= 1e-7);
        }

        [TestMethod]
        public void Sum()
        {
            Vector v1 = Vector.RandomGaussian(100, 1234);
            Assert.IsTrue(Math.Abs(v1.Get<float>().Sum() - v1.Sum()) <= 1e-7);

            Vector v2 = Vector.RandomGaussian(100, 2345, MemorySpace.Device, MathDomain.Double);
            Assert.IsTrue(Math.Abs(v2.Get<double>().Sum() - v2.Sum()) <= 1e-12);

            Vector v3 = new Vector(123, 3, MemorySpace.Device, MathDomain.Int);
            Assert.IsTrue(Math.Abs(v3.GetRaw<int>().Sum() - v3.Sum()) == 0);
        }
    }
}