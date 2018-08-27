using CudaLightSharp.CudaEnumerators;
using CudaLightSharp.CudaStructures;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CudaLightSharp.Buffers
{
    public static class VectorExtension
    {
        public static void Print<T>(this Vector<T> vector, string label = "") where T : struct, IEquatable<T>, IFormattable
        {
            Print(vector.AsArray(), label);
        }
        public static void Print<T>(this T[] vector, string label = "") where T : struct, IEquatable<T>, IFormattable
        {
            Console.WriteLine("************** " + label + " **************");
            for (uint i = 0; i < vector.Length; i++)
                Console.WriteLine("v[" + i + "] = " + vector[i]);
        }
    }

    public unsafe class Vector: Buffer
    {
        public Vector(int size, MemorySpace memorySpace = MemorySpace.Device, MathDomain mathDomain = MathDomain.Float)
            : base(true, memorySpace, mathDomain)
        {
            buffer = new MemoryBuffer(default(UIntPtr), size, memorySpace, mathDomain);
            ctor(buffer);
        }

        public Vector(int size, double value, MemorySpace memorySpace = MemorySpace.Device, MathDomain mathDomain = MathDomain.Float)
            : this(size, memorySpace, mathDomain)
        {
            Alloc(size, value);
        }

        public Vector(Vector rhs)
            : this(rhs.Size, rhs.memorySpace, rhs.mathDomain)
        {
            ReadFrom(rhs);
        }

        public Vector(double[] rhs)
            : this(rhs.Length, MemorySpace.Device, MathDomain.Double)
        {
            ReadFrom(rhs);
        }
        public Vector(Vector<double> rhs)
            : this(rhs.AsArray())
        {
        }

        public Vector(float[] rhs)
            : this(rhs.Length, MemorySpace.Device, MathDomain.Float)
        {
            ReadFrom(rhs);
        }
        public Vector(Vector<float> rhs)
            : this(rhs.AsArray())
        {
        }

        public Vector(int[] rhs)
            : this(rhs.Length, MemorySpace.Device, MathDomain.Int)
        {
            ReadFrom(rhs);
        }
        public Vector(Vector<int> rhs)
            : this(rhs.AsArray())
        {
        }

        public void Print(string label = "") 
        {
            switch (mathDomain)
            {
                case MathDomain.Null:
                    throw new ArgumentNullException();
                case MathDomain.Int:
                    {
                        var thisVec = GetRaw<int>();
                        thisVec.Print(label);
                        break;
                    }
                case MathDomain.Float:
                    {
                        var thisVec = GetRaw<float>();
                        thisVec.Print(label);
                        break;
                    }
                case MathDomain.Double:
                    {
                        var thisVec = GetRaw<double>();
                        thisVec.Print(label);
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
