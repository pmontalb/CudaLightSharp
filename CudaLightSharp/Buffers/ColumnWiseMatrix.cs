using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using System.Diagnostics;
using CudaLightSharp.CudaEnumerators;
using CudaLightSharp.CudaStructures;
using CudaLightSharp.Manager.CudaAPI;

#if FORCE_32_BIT
    using PtrT = System.UInt32;
#else
    using PtrT = System.UInt64;
#endif

namespace CudaLightSharp.Buffers
{
    public static class MatrixExtension
    {
        public static void Print<T>(this Matrix<T> matrix, string label = "") where T : struct, IEquatable<T>, IFormattable
        {
            Print(matrix.ToArray(), matrix.RowCount, matrix.ColumnCount, label);
        }
        public static void Print<T>(this T[,] matrix, int nRows, int nCols, string label = "") where T : struct, IEquatable<T>, IFormattable
        {
            Debug.Assert(nRows * nCols == matrix.Length);
            Console.WriteLine("************** " + label + " **************");
            for (uint j = 0; j < nCols; ++j)
            {
                for (uint i = 0; i < nRows; ++i)
                    Console.Write(String.Format("m[{0},{1}]={2} ", i, j, matrix[i, j]));
                Console.WriteLine();
            }
        }
        public static void Print<T>(this T[] matrix, int nRows, int nCols, string label = "") where T : struct, IEquatable<T>, IFormattable
        {
            Debug.Assert(nRows * nCols == matrix.Length);
            Console.WriteLine("************** " + label + " **************");
            for (uint j = 0; j < nCols; ++j)
            {
                for (uint i = 0; i < nRows; ++i)
                    Console.Write(String.Format("m[{0},{1}]={2} ", i, j, matrix[i + j * nRows]));
                Console.WriteLine();
            }
        }
    }

    public unsafe class ColumnWiseMatrix : Buffer
    {
        public ColumnWiseMatrix(int nRows, int nCols, MemorySpace memorySpace = MemorySpace.Device, MathDomain mathDomain = MathDomain.Float)
            : base(true, memorySpace, mathDomain)
        {
            _buffer = new MemoryTile(0, (uint)nRows, (uint)nCols, memorySpace, mathDomain);
            ctor(_buffer);

            columns = new Vector[nCols];
            uint shift = _buffer.ElementarySize();

            for (int i = 0; i < nCols; i++)
            {
                MemoryBuffer columnBuffer = new MemoryBuffer(_buffer.pointer + (PtrT)(i * nRows * shift), (uint)nRows, memorySpace, mathDomain);
                columns[i] = new Vector(columnBuffer);
            }
        }

        public ColumnWiseMatrix(int nRows, int nCols, double value, MemorySpace memorySpace = MemorySpace.Device, MathDomain mathDomain = MathDomain.Float)
            : this(nRows, nCols, memorySpace, mathDomain)
        {
            Set(value);
        }

        public ColumnWiseMatrix(ColumnWiseMatrix rhs)
            : this(rhs.nRows, rhs.nCols, rhs.memorySpace, rhs.mathDomain)
        {
            ReadFrom(rhs);
        }

        public ColumnWiseMatrix(Vector rhs)
            : this(rhs.Size, 1, rhs.memorySpace, rhs.mathDomain)
        {
            ReadFrom(rhs);
        }

        internal ColumnWiseMatrix(MemoryTile buffer)
            : base(false, buffer.memorySpace, buffer.mathDomain)
        {
            _buffer = buffer;
        }

        #region Read from double

        public ColumnWiseMatrix(double[] rhs)
            : this(rhs.Length, 1, MemorySpace.Device, MathDomain.Double)
        {
            ReadFrom(rhs, rhs.Length);
        }

        public ColumnWiseMatrix(double[,] rhs)
            : this(rhs.GetLength(0), rhs.GetLength(1), MemorySpace.Device, MathDomain.Double)
        {
            ReadFrom(rhs);
        }

        public ColumnWiseMatrix(Matrix<double> rhs)
            : this(rhs.RowCount, rhs.ColumnCount, MemorySpace.Device, MathDomain.Double)
        {
            ReadFrom(rhs);
        }

        public ColumnWiseMatrix(Vector<double> rhs)
            : this(rhs.ToArray())
        {
        }

        #endregion

        #region Read from float

        public ColumnWiseMatrix(float[] rhs)
            : this(rhs.Length, 1, MemorySpace.Device, MathDomain.Float)
        {
            ReadFrom(rhs, rhs.Length);
        }

        public ColumnWiseMatrix(float[,] rhs)
            : this(rhs.GetLength(0), rhs.GetLength(1), MemorySpace.Device, MathDomain.Float)
        {
            ReadFrom(rhs);
        }

        public ColumnWiseMatrix(Matrix<float> rhs)
            : this(rhs.RowCount, rhs.ColumnCount, MemorySpace.Device, MathDomain.Float)
        {
            ReadFrom(rhs);
        }

        public ColumnWiseMatrix(Vector<float> rhs)
            : this(rhs.ToArray())
        {
        }

        #endregion

        #region Read from int

        public ColumnWiseMatrix(int[] rhs)
            : this(rhs.Length, 1, MemorySpace.Device, MathDomain.Int)
        {
            ReadFrom(rhs, rhs.Length);
        }

        public ColumnWiseMatrix(int[,] rhs)
            : this(rhs.GetLength(0), rhs.GetLength(1), MemorySpace.Device, MathDomain.Int)
        {
            ReadFrom(rhs);
        }

        #endregion

        #region Linear Algebra

        public static ColumnWiseMatrix operator +(ColumnWiseMatrix lhs, ColumnWiseMatrix rhs)
        {
            Debug.Assert(lhs.nRows == rhs.nRows);
            Debug.Assert(lhs.nCols == rhs.nCols);
            Debug.Assert(lhs.memorySpace == rhs.memorySpace);
            Debug.Assert(lhs.mathDomain == rhs.mathDomain);
            Debug.Assert(lhs.buffer.pointer != 0);
            Debug.Assert(rhs.buffer.pointer != 0);

            ColumnWiseMatrix tmp = new ColumnWiseMatrix(lhs);
            CuBlasApi.AddEqualMatrix(tmp._buffer, rhs._buffer, MatrixOperation.None, MatrixOperation.None, 1.0);

            return tmp;
        }

        public static ColumnWiseMatrix operator -(ColumnWiseMatrix lhs, ColumnWiseMatrix rhs)
        {
            Debug.Assert(lhs.nRows == rhs.nRows);
            Debug.Assert(lhs.nCols == rhs.nCols);
            Debug.Assert(lhs.memorySpace == rhs.memorySpace);
            Debug.Assert(lhs.mathDomain == rhs.mathDomain);
            Debug.Assert(lhs.buffer.pointer != 0);
            Debug.Assert(rhs.buffer.pointer != 0);

            ColumnWiseMatrix tmp = new ColumnWiseMatrix(lhs);
            CuBlasApi.AddEqualMatrix(tmp._buffer, rhs._buffer, MatrixOperation.None, MatrixOperation.None, -1.0);

            return tmp;
        }

        public static ColumnWiseMatrix operator %(ColumnWiseMatrix lhs, ColumnWiseMatrix rhs)
        {
            Debug.Assert(lhs.nRows == rhs.nRows);
            Debug.Assert(lhs.nCols == rhs.nCols);
            Debug.Assert(lhs.memorySpace == rhs.memorySpace);
            Debug.Assert(lhs.mathDomain == rhs.mathDomain);
            Debug.Assert(lhs.buffer.pointer != 0);
            Debug.Assert(rhs.buffer.pointer != 0);

            ColumnWiseMatrix tmp = new ColumnWiseMatrix(lhs);
            ElementWiseProduct(tmp.buffer, rhs.buffer);

            return tmp;
        }

        public static ColumnWiseMatrix Add(ColumnWiseMatrix lhs, ColumnWiseMatrix rhs, MatrixOperation lhsOperation = MatrixOperation.None, MatrixOperation rhsOperation = MatrixOperation.None, double alpha = 1.0)
        {
            ColumnWiseMatrix ret = new ColumnWiseMatrix(lhs);
            CuBlasApi.AddEqualMatrix(ret._buffer, rhs._buffer, lhsOperation, rhsOperation, alpha);

            return ret;
        }

        public static ColumnWiseMatrix Subtract(ColumnWiseMatrix lhs, ColumnWiseMatrix rhs, MatrixOperation lhsOperation = MatrixOperation.None, MatrixOperation rhsOperation = MatrixOperation.None)
        {
            ColumnWiseMatrix ret = new ColumnWiseMatrix(lhs);
            CuBlasApi.AddEqualMatrix(ret._buffer, rhs._buffer, lhsOperation, rhsOperation, -1.0);

            return ret;
        }

        public static ColumnWiseMatrix ElementWiseProduct(ColumnWiseMatrix lhs, ColumnWiseMatrix rhs)
        {
            ColumnWiseMatrix ret = lhs % rhs;
            return ret;
        }

        public static ColumnWiseMatrix operator *(ColumnWiseMatrix lhs, ColumnWiseMatrix rhs)
        {
            Debug.Assert(rhs.nRows == lhs.nCols);
            Debug.Assert(lhs.memorySpace == rhs.memorySpace);
            Debug.Assert(lhs.mathDomain == rhs.mathDomain);
            Debug.Assert(lhs.buffer.pointer != 0);
            Debug.Assert(rhs.buffer.pointer != 0);

            ColumnWiseMatrix ret = new ColumnWiseMatrix(lhs.nRows, rhs.nCols, lhs.memorySpace, rhs.mathDomain);
            CuBlasApi.Multiply(ret._buffer, lhs._buffer, rhs._buffer, lhs.nRows, rhs.nRows, MatrixOperation.None, MatrixOperation.None, 1.0);

            return ret;
        }

        public static Vector operator *(ColumnWiseMatrix lhs, Vector rhs)
        {
            Debug.Assert(rhs.Size == lhs.nCols);
            Debug.Assert(lhs.memorySpace == rhs.memorySpace);
            Debug.Assert(lhs.mathDomain == rhs.mathDomain);
            Debug.Assert(lhs.buffer.pointer != 0);
            Debug.Assert(rhs.buffer.pointer != 0);

            Vector ret = new Vector(rhs.Size, lhs.memorySpace, rhs.mathDomain);
            CuBlasApi.Dot(ret.buffer, lhs._buffer, rhs.buffer, MatrixOperation.None, 1.0);

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
            Debug.Assert(buffer.pointer != 0);
            Debug.Assert(rhs.buffer.pointer != 0);
            Debug.Assert(output.buffer.pointer != 0);

            CuBlasApi.Multiply(output._buffer, _buffer, rhs._buffer, nRows, rhs.nRows, lhsOperation, rhsOperation, alpha);
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
            Debug.Assert(buffer.pointer != 0);
            Debug.Assert(rhs.buffer.pointer != 0);
            Debug.Assert(output.buffer.pointer != 0);

            CuBlasApi.Dot(output.buffer, _buffer, rhs.buffer, lhsOperation, alpha);
        }

        /// <summary>
        /// Invert inplace - WARNING, use Solve for higher performance
        /// </summary>
        /// <param name="operation"></param>
        public void Invert(MatrixOperation operation = MatrixOperation.None)
        {
            Debug.Assert(nRows == nCols);
            Debug.Assert(buffer.pointer != 0);
            
            CuBlasApi.Invert(_buffer, operation);            
        }

        /// <summary>
        /// Solve A * X = B, B is overwritten
        /// </summary>
        /// <param name="rhs"></param>
        /// <param name="lhsOperation"></param>
        public void Solve(ColumnWiseMatrix rhs, MatrixOperation lhsOperation = MatrixOperation.None)
        {
            Debug.Assert(nRows == nCols);
            Debug.Assert(buffer.pointer != 0);

            CuBlasApi.Solve(_buffer, rhs._buffer, lhsOperation);
        }

        public void MakeIdentity()
        {
            CuBlasApi.Eye(_buffer);
        }

        public static ColumnWiseMatrix Eye(int nRows, MemorySpace memorySpace = MemorySpace.Device, MathDomain mathDomain = MathDomain.Float)
        {
            ColumnWiseMatrix I = new ColumnWiseMatrix(nRows, nRows, memorySpace, mathDomain);
            I.MakeIdentity();

            return I;
        }

        #endregion

        public void Set(Vector vector, int col)
        {
            columns[col].ReadFrom(vector);
        }

        public void Set<T>(uint col, T value) where T : struct, IEquatable<T>, IFormattable
        {
            columns[col].Set(value);
        }

        public Vector<T> Get<T>(int col) where T : struct, IEquatable<T>, IFormattable
        {
            return columns[col].Get<T>();
        }

        public Matrix<T> GetMatrix<T>() where T : struct, IEquatable<T>, IFormattable
        {
            return Matrix<T>.Build.DenseOfColumnMajor(nRows, nCols, Get<T>());
        }

        public void ColumnLinSpace<T>(int col, T x0, T x1) where T : struct, IEquatable<T>, IFormattable, IComparable
        {
            if (x0.CompareTo(x1) < 0)
                columns[col].LinSpace(x0, x1);
            else if (x0.CompareTo(x1) > 0)
                columns[col].LinSpace(x1, x0);
            else
                throw new NotSupportedException();
        }

        public void ColumnRandomUniform(uint col, int seed = 1234)
        {
            columns[col].RandomUniform(seed);
        }

        public void ColumnRandomGaussian(uint col, int seed = 1234)
        {
            columns[col].RandomGaussian(seed);
        }

        public static ColumnWiseMatrix LinSpace(int nRows, int nCols, double x0, double x1, MemorySpace memorySpace = MemorySpace.Device, MathDomain mathDomain = MathDomain.Float)
        {
            var mat = new ColumnWiseMatrix(nRows, nCols, memorySpace, mathDomain);
            mat.LinSpace(x0, x1);

            return mat;
        }

        public static ColumnWiseMatrix RandomUniform(int nRows, int nCols, int seed, MemorySpace memorySpace = MemorySpace.Device, MathDomain mathDomain = MathDomain.Float)
        {
            var mat = new ColumnWiseMatrix(nRows, nCols, memorySpace, mathDomain);
            mat.RandomUniform(seed);

            return mat;
        }

        public static ColumnWiseMatrix RandomGaussian(int nRows, int nCols, int seed, MemorySpace memorySpace = MemorySpace.Device, MathDomain mathDomain = MathDomain.Float)
        {
            var mat = new ColumnWiseMatrix(nRows, nCols, memorySpace, mathDomain);
            mat.RandomGaussian(seed);

            return mat;
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
                        thisVec.Print(nRows, nCols, label);
                        break;
                    }
                case MathDomain.Float:
                    {
                        var thisVec = GetRaw<float>();
                        thisVec.Print(nRows, nCols, label);
                        break;
                    }
                case MathDomain.Double:
                    {
                        var thisVec = GetRaw<double>();
                        thisVec.Print(nRows, nCols, label);
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        public int nRows => (int)_buffer.nRows;
        public int nCols => (int)_buffer.nCols;

        public Vector Column(int i) { return columns[i]; }

        public Vector<T> GetColumn<T>(int i) where T : struct, IEquatable<T>, IFormattable
        {
            return columns[i].Get<T>();
        }

        private readonly MemoryTile _buffer;
        internal override MemoryBuffer buffer => _buffer;
        internal readonly Vector[] columns;
    }
}
