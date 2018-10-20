using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using CudaLightSharp.Buffers;
using CudaLightSharp.Manager;

namespace UnitTests
{
    [TestClass]
    public class CubTests
    {
        [TestMethod]
        public void CountEquals()
        {
            Vector v1 = Vector.RandomGaussian(100, 1234);
            Assert.AreEqual(v1.Get<float>().Sum(), v1.Sum());
        }
    }
}