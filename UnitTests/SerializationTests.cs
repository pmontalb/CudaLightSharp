using Microsoft.VisualStudio.TestTools.UnitTesting;
using CudaLightSharp.Buffers;
using CudaLightSharp.Manager;
using CudaLightSharp.CudaEnumerators;
using MathNet.Numerics.LinearAlgebra;
using System;

namespace UnitTests
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void VectorSerializationInversion()
        {
            Vector v = new Vector(10, 5.0);
            v.ToBinaryFile<float>("v.zf");

            Vector u = new Vector("v.zf", MathDomain.Float);
            Assert.AreEqual(u, v);
        }

        [TestMethod]
        public void MatrixSerializationInversion()
        {
            ColumnWiseMatrix A = new ColumnWiseMatrix(10, 20, 5.0);
            A.ToBinaryFile<float>("A.zf");

            ColumnWiseMatrix B = new ColumnWiseMatrix("A.zf", MathDomain.Float);
            Assert.AreEqual(A, B);
        }
    }
}
