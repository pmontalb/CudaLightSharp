using CudaLightSharp.Buffers;
using CudaLightSharp.CudaEnumerators;
using CudaLightSharp.CudaStructures;
using CudaLightSharp.Manager.CudaAPI;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CudaLightSharp.SparseBuffers
{
    public class CompressedSparseRowMatrix : Buffers.ContiguousMemoryBuffer
    {
        public CompressedSparseRowMatrix(int nRows, int nCols, Vector nonZeroColumnIndices, Vector nNonZeroRows, MathDomain mathDomain)
            : base(false, // SparseVector doesn't allocate its memory in its buffer, but it uses the convenience vector this.values
                  nonZeroColumnIndices.memorySpace, mathDomain)
        {
            _buffer = new SparseMemoryTile(0, (uint)nonZeroColumnIndices.Size, 0, 0, (uint)nRows, (uint)nCols, nonZeroColumnIndices.memorySpace, mathDomain);
            Debug.Assert(denseSize > nonZeroColumnIndices.Size);

            values = new Vector(nonZeroColumnIndices.Size, nonZeroColumnIndices.memorySpace, mathDomain);
            this.nonZeroColumnIndices = nonZeroColumnIndices;
            this.nNonZeroRows = nNonZeroRows;
            
            SyncPointers();
        }

        public CompressedSparseRowMatrix(int nRows, int nCols, Vector nonZeroColumnIndices, Vector nNonZeroRows, double value, MathDomain mathDomain)
            : this(nRows, nCols, nonZeroColumnIndices, nNonZeroRows, mathDomain)
        {
            values.Set(value);
        }

        /// <summary>
        /// Copy denseVector to host, numerically finds the non-zero indices, and then copy back to device
        /// </summary>
        public CompressedSparseRowMatrix(ColumnWiseMatrix denseMatrix)
            : base(false, denseMatrix.memorySpace, denseMatrix.mathDomain)
        {
            _buffer = new SparseMemoryTile(0, 0, 0, 0, (uint)denseMatrix.nRows, (uint)denseMatrix.nCols, denseMatrix.memorySpace, denseMatrix.mathDomain);

            switch (denseMatrix.mathDomain)
            {
                case MathDomain.Int:
                    Compress<int>(denseMatrix);
                    break;
                case MathDomain.Float:
                    Compress<float>(denseMatrix);
                    break;
                case MathDomain.Double:
                    Compress<double>(denseMatrix);
                    break;
                default:
                    break;
            }

            SyncPointers();
        }

        private void Compress<T>(ColumnWiseMatrix denseMatrix) where T : struct, IEquatable<T>, IFormattable
        {
            var hostDenseMatrix = denseMatrix.GetMatrix<T>();

            List<T> nonZeroValues = new List<T>();
            List<int> nonZeroColumnIndices = new List<int>();
            List<int> nNonZeroRows = new List<int>();
            int nNonZeros = 0;
            for (int i = 0; i < denseMatrix.nRows; i++)
            {
                for (int j = 0; j < denseMatrix.nCols; j++)
                {
                    if (Math.Abs(Convert.ToDouble(hostDenseMatrix[i, j])) > 1e-7)
                    {
                        nNonZeros++;
                        nonZeroValues.Add(hostDenseMatrix[i, j]);
                        nonZeroColumnIndices.Add(j);
                    }
                }

                nNonZeroRows.Add(nNonZeros);
            }

            Buffer.size = (uint)nNonZeros;

            values = new Vector(nonZeroValues.Count, memorySpace, mathDomain);
            values.ReadFrom(nonZeroValues, nonZeroValues.Count);

            this.nonZeroColumnIndices = new Vector(nonZeroColumnIndices.Count, memorySpace, MathDomain.Int);
            this.nonZeroColumnIndices.ReadFrom(nonZeroColumnIndices, nonZeroColumnIndices.Count);

            this.nNonZeroRows = new Vector(nNonZeroRows.Count, memorySpace, MathDomain.Int);
            this.nNonZeroRows.ReadFrom(nNonZeroRows, nNonZeroRows.Count);
        }

        public CompressedSparseRowMatrix(CompressedSparseRowMatrix rhs)
            : this(rhs.nRows, rhs.nCols, rhs.nonZeroColumnIndices, rhs.nNonZeroRows, rhs.mathDomain)
        {
            this.values = new Vector(rhs.values);
            SyncPointers();
        }

        /// <summary>
		/// buffer.pointer<- values.pointer
        /// buffer.nonZeroColumnIndices<- nonZeroColumnIndices.pointer
        /// buffer.nNonZeroRows<- nNonZeroRows.pointer
        /// </summary>
        private void SyncPointers()
        {
            _buffer.pointer = values.Buffer.pointer;
            _buffer.nonZeroColumnIndices = nonZeroColumnIndices.Buffer.pointer;
            _buffer.nNonZeroRows = nNonZeroRows.Buffer.pointer;
        }

        public override T[] GetRaw<T>()
        {
            var values = this.values.GetRaw<T>();
            var nonZeroColumnIndices = this.nonZeroColumnIndices.GetRaw<int>();
            var nNonZeroRows = this.nNonZeroRows.GetRaw<int>();

            T[] ret = new T[denseSize];
            int nz = 0;
            for (int i = 0; i < nRows; i++)
            {
                int nNonZeroInRow = nNonZeroRows[i + 1] - nNonZeroRows[i];
                for (int j = 0; j < nNonZeroInRow; j++)
                {
                    ret[i + nonZeroColumnIndices[nz] * nRows] = values[nz];
                    nz++;
                }
            }

            return ret;
        }

        public Matrix<T> GetMatrix<T>() where T : struct, IEquatable<T>, IFormattable
        {
            return Matrix<T>.Build.DenseOfColumnMajor(nRows, nCols, Get<T>());
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

        #region Linear Algebra

        public static ColumnWiseMatrix operator *(CompressedSparseRowMatrix lhs, ColumnWiseMatrix rhs)
        {
            Debug.Assert(rhs.nRows == lhs.nCols);
            Debug.Assert(lhs.memorySpace == rhs.memorySpace);
            Debug.Assert(lhs.mathDomain == rhs.mathDomain);
            Debug.Assert(lhs.Buffer.pointer != 0);
            Debug.Assert(rhs.Buffer.pointer != 0);

            ColumnWiseMatrix ret = new ColumnWiseMatrix(lhs.nRows, rhs.nCols, lhs.memorySpace, lhs.mathDomain);
            CuSparseApi.SparseMultiply(ret.Buffer as MemoryTile, lhs._buffer, rhs.Buffer as MemoryTile, lhs.nRows, rhs.nRows, MatrixOperation.None, MatrixOperation.None, 1.0);

            return ret;
        }

        public static Vector operator *(CompressedSparseRowMatrix lhs, Vector rhs)
        {
            Debug.Assert(rhs.Size == lhs.nCols);
            Debug.Assert(lhs.memorySpace == rhs.memorySpace);
            Debug.Assert(lhs.mathDomain == rhs.mathDomain);
            Debug.Assert(lhs.Buffer.pointer != 0);
            Debug.Assert(rhs.Buffer.pointer != 0);

            Vector ret = new Vector(rhs.Size, lhs.memorySpace, lhs.mathDomain);
            CuSparseApi.SparseDot(ret.Buffer, lhs._buffer, rhs.Buffer, MatrixOperation.None, 1.0);

            return ret;
        }

        public ColumnWiseMatrix Multiply(ColumnWiseMatrix rhs, MatrixOperation lhsOperation = MatrixOperation.None, MatrixOperation rhsOperation = MatrixOperation.None, double alpha = 1.0)
        {
            ColumnWiseMatrix ret = new ColumnWiseMatrix(nRows, rhs.nCols, memorySpace, rhs.mathDomain);
            Multiply(ret, rhs, lhsOperation, rhsOperation, alpha);

            return ret;
        }

        /// <summary>
        /// Same version as above, but gives the possibility of reusing the output buffer
        /// </summary>
        /// <param name="output"></param>
        /// <param name="rhs"></param>
        /// <param name="lhsOperation"></param>
        /// <param name="rhsOperation"></param>
        /// <param name="alpha"></param>
        public void Multiply(ColumnWiseMatrix output, ColumnWiseMatrix rhs, MatrixOperation lhsOperation = MatrixOperation.None, MatrixOperation rhsOperation = MatrixOperation.None, double alpha = 1.0)
        {
            Debug.Assert(rhs.nRows == nCols);
            Debug.Assert(output.nRows == nRows);
            Debug.Assert(output.nCols == rhs.nCols);
            Debug.Assert(memorySpace == rhs.memorySpace);
            Debug.Assert(memorySpace == output.memorySpace);
            Debug.Assert(mathDomain == rhs.mathDomain);
            Debug.Assert(mathDomain == output.mathDomain);
            Debug.Assert(Buffer.pointer != 0);
            Debug.Assert(rhs.Buffer.pointer != 0);
            Debug.Assert(output.Buffer.pointer != 0);

            CuSparseApi.SparseMultiply(output.Buffer as MemoryTile, _buffer, rhs.Buffer as MemoryTile, nRows, rhs.nRows, lhsOperation, rhsOperation, alpha);
        }

        public Vector Dot(Vector rhs, MatrixOperation lhsOperation, double alpha)
        {
            Vector ret = new Vector(rhs.Size, memorySpace, rhs.mathDomain);
            Dot(ret, rhs, lhsOperation, alpha);

            return ret;
        }

        /// <summary>
        /// Same version as above, but gives the possibility of reusing the output buffer
        /// </summary>
        /// <param name="output"></param>
        /// <param name="rhs"></param>
        /// <param name="lhsOperation"></param>
        /// <param name="alpha"></param>
        public void Dot(Vector output, Vector rhs, MatrixOperation lhsOperation = MatrixOperation.None, double alpha = 1.0)
        {
            Debug.Assert(rhs.Size == nCols);
            Debug.Assert(output.Size == rhs.Size);
            Debug.Assert(memorySpace == rhs.memorySpace);
            Debug.Assert(memorySpace == output.memorySpace);
            Debug.Assert(mathDomain == rhs.mathDomain);
            Debug.Assert(mathDomain == output.mathDomain);
            Debug.Assert(Buffer.pointer != 0);
            Debug.Assert(rhs.Buffer.pointer != 0);
            Debug.Assert(output.Buffer.pointer != 0);

            CuSparseApi.SparseDot(output.Buffer, _buffer, rhs.Buffer, lhsOperation, alpha);
        }


        #endregion

        public int nRows => (int)_buffer.nRows;
        public int nCols => (int)_buffer.nCols;
        public int denseSize => nRows * nCols;

        // convenience vector to be used in place of _buffer
        private Vector values;
        private Vector nonZeroColumnIndices;
        private Vector nNonZeroRows;

        private readonly SparseMemoryTile _buffer;
        public override MemoryBuffer Buffer => _buffer;
    }
}
