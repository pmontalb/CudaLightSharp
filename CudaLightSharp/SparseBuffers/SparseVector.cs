﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CudaLightSharp.Buffers;
using CudaLightSharp.CudaStructures;
using CudaLightSharp.CudaEnumerators;
using System.Diagnostics;
using CudaLightSharp.Manager.CudaAPI;

namespace CudaLightSharp.SparseBuffers
{
    class SparseVector : Buffers.Buffer
    {
        public SparseVector(int denseSize, Vector nonZeroIndices, MathDomain mathDomain)
            : base(false, // SparseVector doesn't allocate its memory in its buffer, but it uses the convenience vector this.values
                  nonZeroIndices.memorySpace, mathDomain)
        {
            Debug.Assert(denseSize > nonZeroIndices.Size);

            this.denseSize = denseSize;

            values = new Vector(nonZeroIndices.Size, nonZeroIndices.memorySpace, mathDomain);
            this.nonZeroIndices = nonZeroIndices;

            _buffer = new SparseMemoryBuffer(0, (uint)nonZeroIndices.Size, 0, memorySpace, mathDomain);
            SyncPointers();
        }

        public SparseVector(int denseSize, int nNonZeroIndices, MemorySpace memorySpace, MathDomain mathDomain)
            : this(denseSize, new Vector(nNonZeroIndices, memorySpace, MathDomain.Int), mathDomain)
        {
        }

        public SparseVector(int denseSize, Vector nonZeroIndices, double value, MathDomain mathDomain)
            : this(denseSize, nonZeroIndices, mathDomain)
        {
            values.Set(value);
        }

        /// <summary>
        /// Copy denseVector to host, numerically finds the non-zero indices, and then copy back to device
        /// </summary>
        public SparseVector(Vector denseVector)
        {
            this.denseSize = denseVector.Size;

            switch (denseVector.mathDomain)
            {
                case MathDomain.Int:
                    Compress<int>(denseVector);
                    break;
                case MathDomain.Float:
                    Compress<float>(denseVector);
                    break;
                case MathDomain.Double:
                    Compress<double>(denseVector);
                    break;
                default:
                    break;
            }

            SyncPointers();
        }

        private void Compress<T>(Vector denseVector) where T : struct, IEquatable<T>, IFormattable
        {
            var hostDenseVector = denseVector.GetRaw<T>();

            List<int> nonZeroIndices = new List<int>();
            List<T> nonZeroValues = new List<T>();
            for (int i = 0; i < hostDenseVector.Length; ++i)
            {
                if (Math.Abs(Convert.ToDouble(hostDenseVector[i])) > 1e-7)
                {
                    nonZeroIndices.Add(i);
                    nonZeroValues.Add(hostDenseVector[i]);
                }
            }

            values = new Vector(nonZeroIndices.Count, memorySpace, mathDomain);
            values.ReadFrom(nonZeroValues, nonZeroValues.Count);

            this.nonZeroIndices = new Vector(nonZeroIndices.Count, memorySpace, MathDomain.Int);
            this.nonZeroIndices.ReadFrom(nonZeroIndices, nonZeroIndices.Count);
        }

        public SparseVector(SparseVector rhs)
            : this(rhs.denseSize, rhs.nonZeroIndices, rhs.mathDomain)
        {
            this.values = rhs.values;
            SyncPointers();
        }

        /// <summary>
        /// buffer.pointer<- values.pointer
        /// buffer.indices<- nonZeroIndices.pointer
        /// </summary>
        private void SyncPointers()
        {
            _buffer.pointer = values.buffer.pointer;
            _buffer.indices = nonZeroIndices.buffer.pointer;
        }

        public override T[] GetRaw<T>()
        {
            var _values = values.GetRaw<T>();
            int[] _indices = nonZeroIndices.GetRaw<int>();

            T[] ret = new T[denseSize];
            for (int i = 0; i < denseSize; i++)
                ret[_indices[i]] = _values[i];

            return ret;
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

        #region Dense-Sparse Linear Algebra

        public static Vector operator +(Vector lhs, SparseVector rhs)
        {
            Debug.Assert(lhs.Size == rhs.Size);
            Debug.Assert(lhs.memorySpace == rhs.memorySpace);
            Debug.Assert(lhs.mathDomain == rhs.mathDomain);
            Debug.Assert(lhs.buffer.pointer != 0);
            Debug.Assert(rhs.buffer.pointer != 0);

            Vector ret = new Vector(lhs.Size);
            CuSparseApi.SparseAdd(ret.buffer, rhs._buffer, lhs.buffer, 1.0);

            return ret;
        }

        public static Vector operator -(Vector lhs, SparseVector rhs)
        {
            Debug.Assert(lhs.Size == rhs.Size);
            Debug.Assert(lhs.memorySpace == rhs.memorySpace);
            Debug.Assert(lhs.mathDomain == rhs.mathDomain);
            Debug.Assert(lhs.buffer.pointer != 0);
            Debug.Assert(rhs.buffer.pointer != 0);

            Vector ret = new Vector(lhs.Size);
            CuSparseApi.SparseAdd(ret.buffer, rhs._buffer, lhs.buffer, -1.0);

            return ret;
        }

        public Vector Add(Vector rhs, double alpha = 1.0)
        {
            Debug.Assert(Size == rhs.Size);
            Debug.Assert(memorySpace == rhs.memorySpace);
            Debug.Assert(mathDomain == rhs.mathDomain);
            Debug.Assert(buffer.pointer != 0);
            Debug.Assert(rhs.buffer.pointer != 0);

            Vector ret = new Vector(rhs.Size);
            CuSparseApi.SparseAdd(ret.buffer, _buffer, rhs.buffer, alpha);

            return ret;
        }

        #endregion

        #region Sparse Linear Algebra

        /**
        * WARNING: this assumes the same sparsity pattern between operands
        * NB: buffer.pointer is the same as values.pointer, so it doesn't require any additional care
        */

        public static SparseVector operator +(SparseVector lhs, SparseVector rhs)
        {
            Debug.Assert(lhs.Size == rhs.Size);
            Debug.Assert(lhs.memorySpace == rhs.memorySpace);
            Debug.Assert(lhs.mathDomain == rhs.mathDomain);
            Debug.Assert(lhs.buffer.pointer != 0);
            Debug.Assert(rhs.buffer.pointer != 0);

            SparseVector ret = new SparseVector(lhs);
            CuBlasApi.AddEqual(ret.buffer, rhs.buffer, 1.0);

            return ret;
        }

        public static SparseVector operator -(SparseVector lhs, SparseVector rhs)
        {
            Debug.Assert(lhs.Size == rhs.Size);
            Debug.Assert(lhs.memorySpace == rhs.memorySpace);
            Debug.Assert(lhs.mathDomain == rhs.mathDomain);
            Debug.Assert(lhs.buffer.pointer != 0);
            Debug.Assert(rhs.buffer.pointer != 0);

            SparseVector ret = new SparseVector(lhs);
            CuBlasApi.AddEqual(ret.buffer, rhs.buffer, -1.0);

            return ret;
        }

        public static SparseVector operator %(SparseVector lhs, SparseVector rhs)
        {
            Debug.Assert(lhs.Size == rhs.Size);
            Debug.Assert(lhs.memorySpace == rhs.memorySpace);
            Debug.Assert(lhs.mathDomain == rhs.mathDomain);
            Debug.Assert(lhs.buffer.pointer != 0);
            Debug.Assert(rhs.buffer.pointer != 0);

            SparseVector ret = new SparseVector(lhs);
            ElementWiseProduct(ret.buffer, rhs.buffer);

            return ret;
        }

        #endregion

        public int denseSize;  // used only when converting to dense

        // convenience vector to be used in place of _buffer
        private Vector values;
        private Vector nonZeroIndices;
        

        private readonly SparseMemoryBuffer _buffer;
        internal override MemoryBuffer buffer => _buffer;
    }
}