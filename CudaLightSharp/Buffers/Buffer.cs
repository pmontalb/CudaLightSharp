using CudaLightSharp.CudaEnumerators;
using CudaLightSharp.CudaStructures;
using CudaLightSharp.Manager.CudaAPI;
using System;
using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;

#if FORCE_32_BIT
    using PtrT = System.UInt32;
#else
using PtrT = System.UInt64;
#endif

namespace CudaLightSharp.Buffers
{
    public unsafe abstract class Buffer : IDisposable
    {
        internal Buffer(bool isOwner = true, MemorySpace memorySpace = MemorySpace.Device, MathDomain mathDomain = MathDomain.Float)
        {
            this.isOwner = isOwner;
            this.memorySpace = memorySpace;
            this.mathDomain = mathDomain;
        }

        internal void ctor(MemoryBuffer buffer)
        {
            // if is not the owner it has already been allocated!
            if (!isOwner)
            {
                Debug.Assert(buffer.pointer != 0);
                return;
            }
            Debug.Assert(buffer.size > 0);

            Alloc(buffer);
        }

        internal static void Alloc(MemoryBuffer buffer)
        {
            Debug.Assert(buffer.pointer == 0);

            switch (buffer.memorySpace)
            {
                case MemorySpace.Null:
                    throw new ArgumentNullException();
                case MemorySpace.Host:
                    MemoryManagerApi.AllocHost(buffer);
                    break;
                case MemorySpace.Device:
                    MemoryManagerApi.Alloc(buffer);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        internal void Alloc<T>(int bufferSize, T value) where T : struct, IEquatable<T>, IFormattable
        {
            Alloc(buffer);

            Set(value);
        }

        #region ReadFrom

        internal void ReadFrom(Buffer rhs)
        {
            Debug.Assert(buffer.pointer != 0);
            Debug.Assert(rhs.buffer.pointer != 0);
            MemoryManagerApi.AutoCopy(buffer, rhs.buffer);
        }

        public void ReadFrom<T>(IEnumerable<T> rhs, int size) where T : struct, IEquatable<T>, IFormattable
        {
            Debug.Assert((mathDomain == MathDomain.Double && typeof(T) == typeof(double)) ||
                         (mathDomain == MathDomain.Float && typeof(T) == typeof(float)) ||
                         (mathDomain == MathDomain.Int && typeof(T) == typeof(int)));
            ReadFromImpl(rhs.ToArray(), size);
        }

        public void ReadFrom<T>(Vector<T> rhs) where T : struct, IEquatable<T>, IFormattable
        {
            Debug.Assert((mathDomain == MathDomain.Double && typeof(T) == typeof(double)) ||
                         (mathDomain == MathDomain.Float && typeof(T) == typeof(float)) ||
                         (mathDomain == MathDomain.Int && typeof(T) == typeof(int)));
            ReadFromImpl(rhs.ToArray(), rhs.Count);
        }

        public void ReadFrom<T>(T[,] rhs) where T : struct, IEquatable<T>, IFormattable
        {
            Debug.Assert((mathDomain == MathDomain.Double && typeof(T) == typeof(double)) ||
                         (mathDomain == MathDomain.Float && typeof(T) == typeof(float)) ||
                         (mathDomain == MathDomain.Int && typeof(T) == typeof(int)));
            ReadFromImpl(rhs, rhs.Length);
        }

        public void ReadFrom<T>(T[,,] rhs) where T : struct, IEquatable<T>, IFormattable
        {
            Debug.Assert((mathDomain == MathDomain.Double && typeof(T) == typeof(double)) ||
                         (mathDomain == MathDomain.Float && typeof(T) == typeof(float)) ||
                         (mathDomain == MathDomain.Int && typeof(T) == typeof(int)));
            ReadFromImpl(rhs, rhs.Length);
        }

        public void ReadFrom<T>(Matrix<T> rhs) where T : struct, IEquatable<T>, IFormattable
        {
            Debug.Assert((mathDomain == MathDomain.Double && typeof(T) == typeof(double)) ||
                         (mathDomain == MathDomain.Float && typeof(T) == typeof(float)) ||
                         ( mathDomain == MathDomain.Int && typeof(T) == typeof(int)));
            ReadFromImpl(rhs.ToColumnMajorArray(), rhs.RowCount * rhs.ColumnCount);
        }

        private void ReadFromImpl(object rhs, int nElements)
        {
            MemoryBuffer rhsBuf;
            switch (mathDomain)
            {
                case MathDomain.Null:
                    throw new ArgumentNullException();
                case MathDomain.Int:
                    fixed (int* rhsPtr = (int[])rhs)
                    {
                        Debug.Assert(rhsPtr != null);
                        rhsBuf = new MemoryBuffer((PtrT)rhsPtr, (uint)nElements, memorySpace, mathDomain);
                    }
                    break;
                case MathDomain.Float:
                    fixed (float* rhsPtr = (float[])rhs)
                    {
                        Debug.Assert(rhsPtr != null);
                        rhsBuf = new MemoryBuffer((PtrT)rhsPtr, (uint)nElements, memorySpace, mathDomain);
                    }
                    break;
                case MathDomain.Double:
                    fixed (double* rhsPtr = (double[])rhs)
                    {
                        Debug.Assert(rhsPtr != null);
                        rhsBuf = new MemoryBuffer((PtrT)rhsPtr, (uint)nElements, memorySpace, mathDomain);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            MemoryManagerApi.AutoCopy(buffer, rhsBuf);
        }

        #endregion

        #region Buffer Initialization

        public void Set<T>(T value) where T : struct, IEquatable<T>, IFormattable
        {
            Debug.Assert(buffer.pointer != 0);
            BufferInitializerApi.Initialize(buffer, Convert.ToDouble(value));
        }

        public void LinSpace<T>(T x0, T x1) where T : struct, IEquatable<T>, IFormattable
        {
            Debug.Assert(buffer.pointer != 0);
            BufferInitializerApi.LinSpace(buffer, Convert.ToDouble(x0), Convert.ToDouble(x1));
        }

        public void RandomUniform(int seed = 1234)
        {
            Debug.Assert(buffer.pointer != 0);
            BufferInitializerApi.RandUniform(buffer, seed);
        }

        public void RandomGaussian(int seed = 1234)
        {
            Debug.Assert(buffer.pointer != 0);
            BufferInitializerApi.RandNormal(buffer, seed);
        }

        #endregion

        #region Dispose

        ~Buffer()
        {
            if (!disposed)
                Console.WriteLine(String.Format("Not-disposed warning: {0}", GetType()));
        }

        public void Dispose()
        {
            if (!isOwner)
                return;
            if (disposed)
                return;

            Debug.WriteLine(String.Format("{0:G}, {1}: {2}[{3}]", DateTime.Now, "Disposing", GetType(), buffer.pointer));

            Debug.Assert(buffer.pointer != 0);
            switch (memorySpace)
            {
                case MemorySpace.Null:
                    throw new ArgumentNullException();
                case MemorySpace.Host:
                    MemoryManagerApi.FreeHost(buffer);
                    break;
                case MemorySpace.Device:
                    MemoryManagerApi.Free(buffer);
                    break;
                default:
                    throw new NotImplementedException();
            }

            GC.SuppressFinalize(this);
        }

        #endregion

        #region Get underlying data

        public virtual T[] GetRaw<T>() where T : struct, IEquatable<T>, IFormattable
        {
            Debug.Assert(buffer.pointer != 0);

            // prepare a buffer host-side
            MemoryBuffer hostBuffer = new MemoryBuffer(0, (uint)buffer.size, MemorySpace.Host, mathDomain);
            MemoryManagerApi.AllocHost(hostBuffer);
            MemoryManagerApi.AutoCopy(hostBuffer, buffer);

            object ret = null;

            switch (mathDomain)
            {
                case MathDomain.Null:
                    throw new ArgumentNullException();
                case MathDomain.Int:
                    ret = new int[hostBuffer.size];
                    Marshal.Copy((IntPtr)hostBuffer.pointer, (int[])ret, 0, (int)hostBuffer.size);
                    break;
                case MathDomain.Float:
                    ret = new float[hostBuffer.size];
                    Marshal.Copy((IntPtr)hostBuffer.pointer, (float[])ret, 0, (int)hostBuffer.size);
                    break;
                case MathDomain.Double:
                    ret = new double[hostBuffer.size];
                    Marshal.Copy((IntPtr)hostBuffer.pointer, (double[])ret, 0, (int)hostBuffer.size);
                    break;
                default:
                    throw new NotImplementedException();
            }


            return (T[])Convert.ChangeType(ret, typeof(T[]));
        }

        public Vector<T> Get<T>() where T : struct, IEquatable<T>, IFormattable
        {
            return Vector<T>.Build.DenseOfArray(GetRaw<T>());
        }

        #endregion

        #region Equality operators

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Buffer buf = obj as Buffer;
            if (buf == null)
                return false;

            return Equals(buf);
        }

        public bool Equals(Buffer rhs)
        {
            if (mathDomain != rhs.mathDomain)
                return false;
            if (memorySpace != rhs.memorySpace)
                return false;

            switch (mathDomain)
            {
                case MathDomain.Null:
                    throw new ArgumentNullException();
                case MathDomain.Int:
                    {
                        var thisVec = GetRaw<int>();
                        var thatVec = rhs.GetRaw<int>();
                        if (thisVec.Length != thatVec.Length)
                            return false;

                        for (int i =0; i < thisVec.Length; ++i)
                        {
                            if (thisVec[i] != thatVec[i])
                                return false;
                        }

                        return true;
                    }
                case MathDomain.Float:
                    {
                        var thisVec = Get<float>();
                        var thatVec = rhs.Get<float>();
                        return (thisVec - thatVec).AbsoluteMaximum() <= 1e-7;
                    }
                case MathDomain.Double:
                    {
                        var thisVec = Get<double>();
                        var thatVec = rhs.Get<double>();
                        return (thisVec - thatVec).AbsoluteMaximum() <= 1e-15;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        public static bool operator ==(Buffer lhs, Buffer rhs)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(lhs, rhs))
                return true;

            // If one is null, but not both, return false.
            if (((object)lhs == null) || ((object)rhs == null))
                return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(Buffer lhs, Buffer rhs)
        {
            return !(lhs == rhs);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region Linear Algebra

        internal static MemoryBuffer Add(MemoryBuffer lhs, MemoryBuffer rhs)
        {
            CuBlasApi.AddEqual(lhs, rhs, 1.0);
            return lhs;
        }

        public void AddEqual(Buffer rhs, double alpha = 1.0)
        {
            Debug.Assert(Size == rhs.Size);
            Debug.Assert(memorySpace == rhs.memorySpace);
            Debug.Assert(mathDomain == rhs.mathDomain);
            Debug.Assert(buffer.pointer != 0);
            Debug.Assert(rhs.buffer.pointer != 0);

            CuBlasApi.AddEqual(buffer, rhs.buffer, alpha);
        }

        internal static MemoryBuffer Subtract(MemoryBuffer lhs, MemoryBuffer rhs)
        {
            CuBlasApi.SubtractEqual(lhs, rhs);
            return lhs;
        }

        public void SubtractEqual(Buffer rhs)
        {
            Debug.Assert(Size == rhs.Size);
            Debug.Assert(memorySpace == rhs.memorySpace);
            Debug.Assert(mathDomain == rhs.mathDomain);
            Debug.Assert(buffer.pointer != 0);
            Debug.Assert(rhs.buffer.pointer != 0);

            CuBlasApi.SubtractEqual(buffer, rhs.buffer);
        }

        internal static MemoryBuffer ElementWiseProduct(MemoryBuffer lhs, MemoryBuffer rhs)
        {
            MemoryBuffer buffer = new MemoryBuffer(0, (uint)lhs.size, lhs.memorySpace, lhs.mathDomain);
            Alloc(buffer);

            MemoryManagerApi.AutoCopy(buffer, lhs);
            CuBlasApi.ElementwiseProduct(lhs, buffer, rhs, 1.0);

            return lhs;
        }

        public void Scale(double alpha)
        {
            Debug.Assert(buffer.pointer != 0);
            CuBlasApi.Scale(buffer, alpha);
        }

        #endregion

        public int Size => (int)buffer.size;

        public readonly MemorySpace memorySpace;
        public readonly MathDomain mathDomain;

        abstract internal MemoryBuffer buffer { get; }
        protected readonly bool isOwner;

        private bool disposed = false;
    }
}
