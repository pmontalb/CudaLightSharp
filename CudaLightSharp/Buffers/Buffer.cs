using CudaLightSharp.CudaEnumerators;
using CudaLightSharp.CudaStructures;
using CudaLightSharp.Manager.CudaAPI;
using System;
using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using ZeroFormatter;
using ZeroFormatter.Formatters;
using System.IO;

#if FORCE_32_BIT
    using PtrT = System.UInt32;
#else
using PtrT = System.UInt64;
#endif

namespace CudaLightSharp.Buffers
{
    [Serializable]
    public unsafe abstract class ContiguousMemoryBuffer : IDisposable
    {
        internal ContiguousMemoryBuffer(bool isOwner = true, MemorySpace memorySpace = MemorySpace.Device, MathDomain mathDomain = MathDomain.Float)
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
            Alloc(Buffer);

            Set(value);
        }

        #region ReadFrom

        internal void ReadFrom(ContiguousMemoryBuffer rhs)
        {
            Debug.Assert(Buffer.pointer != 0);
            Debug.Assert(rhs.Buffer.pointer != 0);
            MemoryManagerApi.AutoCopy(Buffer, rhs.Buffer);
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

            T[] data = new T[rhs.Length];
            int nRows = rhs.GetLength(0);
            int nCols = rhs.GetLength(1);
            for (int i = 0; i < nRows; i++)
                for (int j = 0; j < nCols; j++)
                    data[i + nRows * j] = rhs[i, j];
            ReadFromImpl(data, data.Length);
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
            if (nElements != Buffer.size)
            {
                // need to reallocate device memory
                if (Buffer.pointer != 0)
                    Free(Buffer);

                Buffer.size = (uint)nElements;
                Alloc(Buffer);
            }

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

            MemoryManagerApi.AutoCopy(Buffer, rhsBuf);
        }

        #endregion

        #region Buffer Initialization

        public void Set<T>(T value) where T : struct, IEquatable<T>, IFormattable
        {
            Debug.Assert(Buffer.pointer != 0);
            BufferInitializerApi.Initialize(Buffer, Convert.ToDouble(value));
        }

        public void LinSpace<T>(T x0, T x1) where T : struct, IEquatable<T>, IFormattable
        {
            Debug.Assert(Buffer.pointer != 0);
            BufferInitializerApi.LinSpace(Buffer, Convert.ToDouble(x0), Convert.ToDouble(x1));
        }

        public void RandomUniform(int seed = 1234)
        {
            Debug.Assert(Buffer.pointer != 0);
            BufferInitializerApi.RandUniform(Buffer, seed);
        }

        public void RandomGaussian(int seed = 1234)
        {
            Debug.Assert(Buffer.pointer != 0);
            BufferInitializerApi.RandNormal(Buffer, seed);
        }

        #endregion

        #region Dispose

        ~ContiguousMemoryBuffer()
        {
            Dispose(true);
        }

        protected void Dispose(bool isDisposing)
        {
            if (isDisposing && !disposed)
            {
                if (isOwner)
                    Free(Buffer);
                disposed = true;
            }

            if (!isDisposing && !disposed)
                throw new SystemException("Resource not successfully freed");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private static void Free(MemoryBuffer buffer)
        {
            Debug.Assert(buffer.pointer != 0);
            switch (buffer.memorySpace)
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

            buffer.pointer = 0;
        }

        #endregion

        #region Serialization

        public virtual void ReadFromTextFile<T>(string filePath) where T : struct, IEquatable<T>, IFormattable
        {
            string[] lines = File.ReadAllLines(filePath);
            T[] data = new T[lines.Length];

            for (int i = 0; i < lines.Length; i++)
                data[i] = (T)Convert.ChangeType(lines[i], typeof(T));

            ReadFrom(data, data.Length);
        }

        public virtual void ReadFromBinaryFile<T>(string filePath) where T : struct, IEquatable<T>, IFormattable
        {
            byte[] bytes = File.ReadAllBytes(filePath);
            T[] data = ZeroFormatterSerializer.Deserialize<T[]>(bytes);
            ReadFrom(data, data.Length);
        }

        public virtual void ToBinaryFile<T>(string filePath) where T : struct, IEquatable<T>, IFormattable
        {
            var data = GetRaw<T>();
            byte[] bytes = ZeroFormatterSerializer.Serialize(data);
            File.WriteAllBytes(filePath, bytes);
        }

        #endregion

        #region Get underlying data

        public virtual T[] GetRaw<T>() where T : struct, IEquatable<T>, IFormattable
        {
            Debug.Assert(Buffer.pointer != 0);

            // prepare a buffer host-side
            MemoryBuffer hostBuffer = new MemoryBuffer(0, (uint)Buffer.size, MemorySpace.Host, mathDomain);
            MemoryManagerApi.AllocHost(hostBuffer);
            MemoryManagerApi.AutoCopy(hostBuffer, Buffer);

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

            ContiguousMemoryBuffer buf = obj as ContiguousMemoryBuffer;
            if (buf == null)
                return false;

            return Equals(buf);
        }

        public bool Equals(ContiguousMemoryBuffer rhs)
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

        public static bool operator ==(ContiguousMemoryBuffer lhs, ContiguousMemoryBuffer rhs)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(lhs, rhs))
                return true;

            // If one is null, but not both, return false.
            if (((object)lhs == null) || ((object)rhs == null))
                return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(ContiguousMemoryBuffer lhs, ContiguousMemoryBuffer rhs)
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

        public void AddEqual(ContiguousMemoryBuffer rhs, double alpha = 1.0)
        {
            Debug.Assert(Size == rhs.Size);
            Debug.Assert(memorySpace == rhs.memorySpace);
            Debug.Assert(mathDomain == rhs.mathDomain);
            Debug.Assert(Buffer.pointer != 0);
            Debug.Assert(rhs.Buffer.pointer != 0);

            CuBlasApi.AddEqual(Buffer, rhs.Buffer, alpha);
        }

        internal static MemoryBuffer Subtract(MemoryBuffer lhs, MemoryBuffer rhs)
        {
            CuBlasApi.SubtractEqual(lhs, rhs);
            return lhs;
        }

        public void SubtractEqual(ContiguousMemoryBuffer rhs)
        {
            Debug.Assert(Size == rhs.Size);
            Debug.Assert(memorySpace == rhs.memorySpace);
            Debug.Assert(mathDomain == rhs.mathDomain);
            Debug.Assert(Buffer.pointer != 0);
            Debug.Assert(rhs.Buffer.pointer != 0);

            CuBlasApi.SubtractEqual(Buffer, rhs.Buffer);
        }

        internal static MemoryBuffer ElementWiseProduct(MemoryBuffer lhs, MemoryBuffer rhs)
        {
            return ElementWiseProduct(lhs, lhs, rhs);
        }

        internal static MemoryBuffer ElementWiseProduct(MemoryBuffer output, MemoryBuffer lhs, MemoryBuffer rhs)
        {
            CuBlasApi.ElementwiseProduct(output, lhs, rhs, 1.0);

            return output;
        }

        public void Scale(double alpha)
        {
            Debug.Assert(Buffer.pointer != 0);
            CuBlasApi.Scale(Buffer, alpha);
        }

        public int AbsoluteMinimumIndex()
        {
            Debug.Assert(Buffer.pointer != 0);
            return CuBlasApi.AbsoluteMinimumIndex(Buffer);
        }

        public int AbsoluteMaximumIndex()
        {
            Debug.Assert(Buffer.pointer != 0);
            return CuBlasApi.AbsoluteMaximumIndex(Buffer);
        }

        public double Minimum()
        {
            Debug.Assert(Buffer.pointer != 0);
            return CubApi.Min(Buffer);
        }

        public double AbsoluteMinimum()
        {
            Debug.Assert(Buffer.pointer != 0);
            return CubApi.AbsMin(Buffer);
        }

        public double Maximum()
        {
            Debug.Assert(Buffer.pointer != 0);
            return CubApi.Max(Buffer);
        }

        public double AbsoluteMaximum()
        {
            Debug.Assert(Buffer.pointer != 0);
            return CubApi.AbsMax(Buffer);
        }

        public double Sum()
        {
            Debug.Assert(Buffer.pointer != 0);
            return CubApi.Sum(Buffer);
        }

        public int CountEquals(ContiguousMemoryBuffer rhs)
        {
            MemoryBuffer cache = new MemoryBuffer();
            return CountEquals(rhs, cache);
        }

        public int CountEquals(ContiguousMemoryBuffer rhs, MemoryBuffer cache)
        {
            Debug.Assert(Buffer.pointer != 0);
            Debug.Assert(rhs.Buffer.pointer != 0);
            Debug.Assert(Size == rhs.Size);

            bool needToFreeCache = cache.pointer == 0;
            if (needToFreeCache)
            {
                cache.memorySpace = memorySpace;
                cache.mathDomain = mathDomain;
                cache.size = (uint)Size;
                Alloc(cache);
            }

            // calculate the difference
            CuBlasApi.Subtract(cache, Buffer, rhs.Buffer);

            // calculate how many non-zeros, overriding cache
            CuBlasApi.IsNonZero(cache, cache);

            double ret = CubApi.Sum(cache);

            // we are counting the zero entries
            ret = Size - ret;

            if (needToFreeCache)
                Free(cache);

            return (int)ret;
        }

        #endregion

        public int Size => (int)Buffer.size;

        public readonly MemorySpace memorySpace;
        public readonly MathDomain mathDomain;

        abstract public MemoryBuffer Buffer { get; }
        protected readonly bool isOwner;

        private bool disposed = false;
    }
}
