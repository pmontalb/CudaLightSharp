using CudaLightSharp.CudaEnumerators;
using CudaLightSharp.CudaStructures;
using CudaLightSharp.Manager.CudaAPI;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if FORCE_32_BIT
    using PtrT = System.UInt32;
#else
    using PtrT = System.UInt64;
#endif

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
            _buffer = new MemoryBuffer(0, (uint)size, memorySpace, mathDomain);
            ctor(buffer);
        }

        public Vector(int size, double value, MemorySpace memorySpace = MemorySpace.Device, MathDomain mathDomain = MathDomain.Float)
            : this(size, memorySpace, mathDomain)
        {
            Set(value);
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

        internal Vector(MemoryBuffer buffer)
            : base(false, buffer.memorySpace, buffer.mathDomain)
        {
            _buffer = buffer;
        }

        #region Linear Algebra

        public static Vector operator +(Vector lhs, Vector rhs)
        {
            Debug.Assert(lhs.Size == rhs.Size);
            Debug.Assert(lhs.memorySpace == rhs.memorySpace);
            Debug.Assert(lhs.mathDomain == rhs.mathDomain);
            Debug.Assert(lhs.buffer.pointer != 0);
            Debug.Assert(rhs.buffer.pointer != 0);

            Vector tmp = new Vector(lhs);
            Add(tmp.buffer, rhs.buffer);

            return tmp;
        }

        public static Vector operator -(Vector lhs, Vector rhs)
        {
            Debug.Assert(lhs.Size == rhs.Size);
            Debug.Assert(lhs.memorySpace == rhs.memorySpace);
            Debug.Assert(lhs.mathDomain == rhs.mathDomain);
            Debug.Assert(lhs.buffer.pointer != 0);
            Debug.Assert(rhs.buffer.pointer != 0);

            Vector tmp = new Vector(lhs);
            Subtract(tmp.buffer, rhs.buffer);

            return tmp;
        }

        public static Vector operator %(Vector lhs, Vector rhs)
        {
            Debug.Assert(lhs.Size == rhs.Size);
            Debug.Assert(lhs.memorySpace == rhs.memorySpace);
            Debug.Assert(lhs.mathDomain == rhs.mathDomain);
            Debug.Assert(lhs.buffer.pointer != 0);
            Debug.Assert(rhs.buffer.pointer != 0);

            Vector tmp = new Vector(lhs);
            ElementWiseProduct(tmp.buffer, rhs.buffer);

            return tmp;
        }

        #endregion

        public static Vector LinSpace(int size, double x0, double x1, MemorySpace memorySpace = MemorySpace.Device, MathDomain mathDomain = MathDomain.Float)
        {
            var vec = new Vector(size, memorySpace, mathDomain);
            vec.LinSpace(x0, x1);

            return vec;
        }

        public static Vector RandomUniform(int size, int seed, MemorySpace memorySpace = MemorySpace.Device, MathDomain mathDomain = MathDomain.Float)
        {
            var vec = new Vector(size, memorySpace, mathDomain);
            vec.RandomUniform(seed);

            return vec;
        }

        public static Vector RandomGaussian(int size, int seed, MemorySpace memorySpace = MemorySpace.Device, MathDomain mathDomain = MathDomain.Float)
        {
            var vec = new Vector(size, memorySpace, mathDomain);
            vec.RandomGaussian(seed);

            return vec;
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

        private MemoryBuffer _buffer;
        internal override MemoryBuffer buffer => _buffer;
    }
}
