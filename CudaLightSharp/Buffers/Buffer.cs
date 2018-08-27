using CudaLightSharp.CudaEnumerators;
using CudaLightSharp.CudaStructures;
using CudaLightSharp.Manager.CudaAPI;
using System;
using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra;
using System.Runtime.InteropServices;

namespace CudaLightSharp.Buffers
{
    public unsafe class Buffer : IDisposable
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
                Debug.Assert(buffer.pointer != default(UIntPtr));
                return;
            }
            Debug.Assert(buffer.size > 0);

            Alloc(buffer);
        }

        internal void Alloc(MemoryBuffer buffer)
        {
            Debug.Assert(buffer.pointer == default(UIntPtr));

            switch (memorySpace)
            {
                case MemorySpace.Null:
                    throw new ArgumentNullException();
                case MemorySpace.Host:
                    DeviceApi.AllocHost(buffer);
                    break;
                case MemorySpace.Device:
                    DeviceApi.Alloc(buffer);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        internal void Alloc<T>(int bufferSize, T value) where T : struct, IEquatable<T>, IFormattable
        {
            buffer = new MemoryBuffer(default(UIntPtr), bufferSize, memorySpace, mathDomain);
            Alloc(buffer);

            Set(value);
        }

        #region ReadFrom

        internal void ReadFrom(Buffer rhs)
        {
            Debug.Assert(buffer.pointer != default(UIntPtr));
            Debug.Assert(rhs.buffer.pointer != default(UIntPtr));
            DeviceApi.AutoCopy(buffer, rhs.buffer);
        }

        public void ReadFrom<T>(T[] rhs) where T : struct, IEquatable<T>, IFormattable
        {
            Debug.Assert((mathDomain == MathDomain.Double && typeof(T) == typeof(double)) ||
                         (mathDomain == MathDomain.Float && typeof(T) == typeof(float)) ||
                         (mathDomain == MathDomain.Int && typeof(T) == typeof(int)));
            ReadFrom(rhs, rhs.Length);
        }

        public void ReadFrom<T>(Vector<T> rhs) where T : struct, IEquatable<T>, IFormattable
        {
            Debug.Assert((mathDomain == MathDomain.Double && typeof(T) == typeof(double)) ||
                         (mathDomain == MathDomain.Float && typeof(T) == typeof(float)) ||
                         (mathDomain == MathDomain.Int && typeof(T) == typeof(int)));
            ReadFrom(rhs.AsArray(), rhs.Count);
        }

        private void ReadFrom(object rhs, int nElements)
        {
            MemoryBuffer rhsBuf = null;
            switch (mathDomain)
            {
                case MathDomain.Null:
                    throw new ArgumentNullException();
                case MathDomain.Int:
                    fixed (int* rhsPtr = (int[])rhs)
                    {
                        Debug.Assert(rhsPtr != null);
                        rhsBuf = new MemoryBuffer((UIntPtr)rhsPtr, nElements, memorySpace, mathDomain);
                    }
                    break;
                case MathDomain.Float:
                    fixed (float* rhsPtr = (float[])rhs)
                    {
                        Debug.Assert(rhsPtr != null);
                        rhsBuf = new MemoryBuffer((UIntPtr)rhsPtr, nElements, memorySpace, mathDomain);
                    }
                    break;
                case MathDomain.Double:
                    fixed (double* rhsPtr = (double[])rhs)
                    {
                        Debug.Assert(rhsPtr != null);
                        rhsBuf = new MemoryBuffer((UIntPtr)rhsPtr, nElements, memorySpace, mathDomain);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            DeviceApi.AutoCopy(buffer, rhsBuf);
        }

        #endregion

        #region Buffer Initialization

        public void Set<T>(T value) where T : struct, IEquatable<T>, IFormattable
        {
            Debug.Assert(buffer.pointer != default(UIntPtr));
            BufferInitializerApi.Initialize(buffer, Convert.ToDouble(value));
        }

        public void LinSpace<T>(T x0, T x1) where T : struct, IEquatable<T>, IFormattable
        {
            Debug.Assert(buffer.pointer != default(UIntPtr));
            BufferInitializerApi.LinSpace(buffer, Convert.ToDouble(x0), Convert.ToDouble(x1));
        }

        public void RandomUniform(int seed = 1234)
        {
            Debug.Assert(buffer.pointer != default(UIntPtr));
            BufferInitializerApi.RandUniform(buffer, seed);
        }

        public void RandomGaussian(int seed = 1234)
        {
            Debug.Assert(buffer.pointer != default(UIntPtr));
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

            Debug.Assert(buffer.pointer != default(UIntPtr));
            switch (memorySpace)
            {
                case MemorySpace.Null:
                    throw new ArgumentNullException();
                case MemorySpace.Host:
                    DeviceApi.FreeHost(buffer);
                    break;
                case MemorySpace.Device:
                    DeviceApi.Free(buffer);
                    break;
                default:
                    throw new NotImplementedException();
            }

            GC.SuppressFinalize(this);
        }

        #endregion

        #region Get underlying data

        public T[] GetRaw<T>() where T : struct, IEquatable<T>, IFormattable
        {
            Debug.Assert(buffer.pointer != default(UIntPtr));

            // prepare a buffer host-side
            MemoryBuffer hostBuffer = new MemoryBuffer(default(UIntPtr), (int)buffer.size, MemorySpace.Host, mathDomain);
            DeviceApi.AllocHost(hostBuffer);
            DeviceApi.AutoCopy(hostBuffer, buffer);

            object ret = null;

            switch (mathDomain)
            {
                case MathDomain.Null:
                    throw new ArgumentNullException();
                case MathDomain.Int:
                    ret = new int[hostBuffer.size];
                    Marshal.Copy((IntPtr)hostBuffer.pointer.ToPointer(), (int[])ret, 0, (int)hostBuffer.size);
                    break;
                case MathDomain.Float:
                    ret = new float[hostBuffer.size];
                    Marshal.Copy((IntPtr)hostBuffer.pointer.ToPointer(), (float[])ret, 0, (int)hostBuffer.size);
                    break;
                case MathDomain.Double:
                    ret = new double[hostBuffer.size];
                    Marshal.Copy((IntPtr)hostBuffer.pointer.ToPointer(), (double[])ret, 0, (int)hostBuffer.size);
                    break;
                default:
                    throw new NotImplementedException();
            }


            return (T[])Convert.ChangeType(ret, typeof(T));
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
                        var thisVec = Get<int>();
                        var thatVec = rhs.Get<int>();
                        return thisVec == thatVec;
                    }
                case MathDomain.Float:
                    {
                        var thisVec = Get<float>();
                        var thatVec = rhs.Get<float>();
                        return thisVec == thatVec;
                    }
                case MathDomain.Double:
                    {
                        var thisVec = Get<double>();
                        var thatVec = rhs.Get<double>();
                        return thisVec == thatVec;
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

        public static Buffer operator +(Buffer lhs, Buffer rhs)
        {
            Debug.Assert(lhs.Size == rhs.Size);
            Debug.Assert(lhs.memorySpace == rhs.memorySpace);
            Debug.Assert(lhs.mathDomain == rhs.mathDomain);
            Debug.Assert(lhs.buffer.pointer != default(UIntPtr));
            Debug.Assert(rhs.buffer.pointer != default(UIntPtr));

            Buffer tmp = new Buffer(true, lhs.memorySpace, lhs.mathDomain);
            tmp.buffer = new MemoryBuffer(default(UIntPtr), lhs.Size, lhs.memorySpace, lhs.mathDomain);
            tmp.Alloc(tmp.buffer);

            DeviceApi.AutoCopy(tmp.buffer, lhs.buffer);
            CuBlasApi.AddEqual(tmp.buffer, rhs.buffer, 1.0);
            return tmp;
        }

        public void AddEqual(Buffer rhs, double alpha = 1.0)
        {
            Debug.Assert(Size == rhs.Size);
            Debug.Assert(memorySpace == rhs.memorySpace);
            Debug.Assert(mathDomain == rhs.mathDomain);
            Debug.Assert(buffer.pointer != default(UIntPtr));
            Debug.Assert(rhs.buffer.pointer != default(UIntPtr));

            CuBlasApi.AddEqual(buffer, rhs.buffer, alpha);
        }

        public static Buffer operator -(Buffer lhs, Buffer rhs)
        {
            Debug.Assert(lhs.Size == rhs.Size);
            Debug.Assert(lhs.memorySpace == rhs.memorySpace);
            Debug.Assert(lhs.mathDomain == rhs.mathDomain);
            Debug.Assert(lhs.buffer.pointer != default(UIntPtr));
            Debug.Assert(rhs.buffer.pointer != default(UIntPtr));

            Buffer tmp = new Buffer(true, lhs.memorySpace, lhs.mathDomain);
            tmp.buffer = new MemoryBuffer(default(UIntPtr), lhs.Size, lhs.memorySpace, lhs.mathDomain);
            tmp.Alloc(tmp.buffer);

            DeviceApi.AutoCopy(tmp.buffer, lhs.buffer);
            CuBlasApi.SubtractEqual(tmp.buffer, rhs.buffer);
            return tmp;
        }

        public static Buffer operator %(Buffer lhs, Buffer rhs)
        {
            Debug.Assert(lhs.Size == rhs.Size);
            Debug.Assert(lhs.memorySpace == rhs.memorySpace);
            Debug.Assert(lhs.mathDomain == rhs.mathDomain);
            Debug.Assert(lhs.buffer.pointer != default(UIntPtr));
            Debug.Assert(rhs.buffer.pointer != default(UIntPtr));

            Buffer tmp = new Buffer(true, lhs.memorySpace, lhs.mathDomain);
            tmp.buffer = new MemoryBuffer(default(UIntPtr), lhs.Size, lhs.memorySpace, lhs.mathDomain);
            tmp.Alloc(tmp.buffer);

            DeviceApi.AutoCopy(tmp.buffer, lhs.buffer);
            CuBlasApi.ElementwiseProduct(tmp.buffer, lhs.buffer, rhs.buffer, 1.0);
            return tmp;
        }

        public void Scale(double alpha)
        {
            Debug.Assert(buffer.pointer != default(UIntPtr));
            CuBlasApi.Scale(buffer, alpha);
        }

        #endregion

        public int Size => (int)buffer.size;

        public readonly MemorySpace memorySpace;
        public readonly MathDomain mathDomain;

        internal MemoryBuffer buffer = null;
        protected readonly bool isOwner;

        private bool disposed = false;
    }
}
