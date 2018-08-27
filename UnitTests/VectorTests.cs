using Microsoft.VisualStudio.TestTools.UnitTesting;
using CudaLightSharp.Buffers;

namespace UnitTests
{
    [TestClass]
    public class VectorTests
    {
        [TestMethod]
        public void BufferAllocation()
        {
            var a = new Vector(10);
            a.Print("aa");
        }
    }
}
