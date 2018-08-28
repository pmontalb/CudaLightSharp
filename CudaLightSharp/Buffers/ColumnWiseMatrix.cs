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

namespace CudaLightSharp.Buffers
{
    public static class MatrixExtension
    {
        public static void Print<T>(this Matrix<T> matrix, string label = "") where T : struct, IEquatable<T>, IFormattable
        {
            Print(matrix.AsArray(), matrix.RowCount, matrix.ColumnCount, label);
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
            this.nRows = nRows;
            this.nCols = nCols;

            buffer = new MemoryTile(0, (uint)nRows, (uint)nCols, memorySpace, mathDomain);
            ctor(buffer);
        }

        public ColumnWiseMatrix(int nRows, int nCols, double value, MemorySpace memorySpace = MemorySpace.Device, MathDomain mathDomain = MathDomain.Float)
            : this(nRows, nCols, memorySpace, mathDomain)
        {
            Alloc(Size, value);
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

        #region Read from double

        public ColumnWiseMatrix(double[] rhs)
            : this(rhs.Length, 1, MemorySpace.Device, MathDomain.Double)
        {
            ReadFrom(rhs);
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
            : this(rhs.AsArray())
        {
        }

        #endregion

        #region Read from float

        public ColumnWiseMatrix(float[] rhs)
            : this(rhs.Length, 1, MemorySpace.Device, MathDomain.Float)
        {
            ReadFrom(rhs);
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
            : this(rhs.AsArray())
        {
        }

        #endregion

        #region Read from int

        public ColumnWiseMatrix(int[] rhs)
            : this(rhs.Length, 1, MemorySpace.Device, MathDomain.Int)
        {
            ReadFrom(rhs);
        }

        public ColumnWiseMatrix(int[,] rhs)
            : this(rhs.GetLength(0), rhs.GetLength(1), MemorySpace.Device, MathDomain.Int)
        {
            ReadFrom(rhs);
        }

        #endregion

        #region Linear Algebra

        public static ColumnWiseMatrix operator *(ColumnWiseMatrix lhs, ColumnWiseMatrix rhs)
        {
            Debug.Assert(rhs.nRows == lhs.nCols);
            Debug.Assert(lhs.memorySpace == rhs.memorySpace);
            Debug.Assert(lhs.mathDomain == rhs.mathDomain);
            Debug.Assert(lhs.buffer.pointer != 0);
            Debug.Assert(rhs.buffer.pointer != 0);

            ColumnWiseMatrix ret = new ColumnWiseMatrix(lhs.nRows, rhs.nCols, lhs.memorySpace, rhs.mathDomain);
            CuBlasApi.Multiply(ret.buffer as MemoryTile, lhs.buffer as MemoryTile, rhs.buffer as MemoryTile, lhs.nRows, rhs.nRows, MatrixOperation.None, MatrixOperation.None, 1.0);

            return ret;
        }

        public ColumnWiseMatrix Multiply(ColumnWiseMatrix rhs, MatrixOperation lhsOperation = MatrixOperation.None, MatrixOperation rhsOperation = MatrixOperation.None, double alpha = 1.0)
        {
            Debug.Assert(rhs.nRows == nCols);
            Debug.Assert(memorySpace == rhs.memorySpace);
            Debug.Assert(mathDomain == rhs.mathDomain);
            Debug.Assert(buffer.pointer != 0);
            Debug.Assert(rhs.buffer.pointer != 0);

            ColumnWiseMatrix ret = new ColumnWiseMatrix(nRows, rhs.nCols, memorySpace, rhs.mathDomain);
            CuBlasApi.Multiply(ret.buffer as MemoryTile, buffer as MemoryTile, rhs.buffer as MemoryTile, nRows, rhs.nRows, lhsOperation, rhsOperation, alpha);

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
            CuBlasApi.Dot(ret.buffer, lhs.buffer as MemoryTile, rhs.buffer, MatrixOperation.None, 1.0);

            return ret;
        }

        public Vector Dot(Vector rhs, MatrixOperation lhsOperation, double alpha)
        {
            Debug.Assert(rhs.Size == nCols);
            Debug.Assert(memorySpace == rhs.memorySpace);
            Debug.Assert(mathDomain == rhs.mathDomain);
            Debug.Assert(buffer.pointer != 0);
            Debug.Assert(rhs.buffer.pointer != 0);

            Vector ret = new Vector(rhs.Size, memorySpace, rhs.mathDomain);
            CuBlasApi.Dot(ret.buffer, buffer as MemoryTile, rhs.buffer, lhsOperation, alpha);

            return ret;
        }

        public void Invert(MatrixOperation operation = MatrixOperation.None)
        {
            Debug.Assert(nRows == nCols);
            Debug.Assert(buffer.pointer != 0);
            
            CuBlasApi.Invert(buffer as MemoryTile, operation);            
        }

        public void Solve(ColumnWiseMatrix rhs, MatrixOperation lhsOperation = MatrixOperation.None)
        {
            Debug.Assert(nRows == nCols);
            Debug.Assert(buffer.pointer != 0);

            CuBlasApi.Solve(buffer as MemoryTile, rhs.buffer as MemoryTile, lhsOperation);
        }

        public void MakeIdentity()
        {
            CuBlasApi.Eye(buffer as MemoryTile);
        }

        public static ColumnWiseMatrix Eye(int nRows, MemorySpace memorySpace = MemorySpace.Device, MathDomain mathDomain = MathDomain.Float)
        {
            ColumnWiseMatrix I = new ColumnWiseMatrix(nRows, nRows, memorySpace, mathDomain);
            I.MakeIdentity();

            return I;
        }

        #endregion

        public Matrix<T> GetMatrix<T>() where T : struct, IEquatable<T>, IFormattable
        {
            return Matrix<T>.Build.DenseOfColumnMajor(nRows, nCols, Get<T>());
        }

        public static ColumnWiseMatrix LinSpace(int nRows, int nCols, double x0, double x1, MemorySpace memorySpace = MemorySpace.Device, MathDomain mathDomain = MathDomain.Float)
        {
            var vec = new ColumnWiseMatrix(nRows, nCols, memorySpace, mathDomain);
            vec.LinSpace(x0, x1);

            return vec;
        }

        public static ColumnWiseMatrix RandomUniform(int nRows, int nCols, int seed, MemorySpace memorySpace = MemorySpace.Device, MathDomain mathDomain = MathDomain.Float)
        {
            var vec = new ColumnWiseMatrix(nRows, nCols, memorySpace, mathDomain);
            vec.RandomUniform(seed);

            return vec;
        }

        public static ColumnWiseMatrix RandomGaussian(int nRows, int nCols, int seed, MemorySpace memorySpace = MemorySpace.Device, MathDomain mathDomain = MathDomain.Float)
        {
            var vec = new ColumnWiseMatrix(nRows, nCols, memorySpace, mathDomain);
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

        public readonly int nRows;
        public readonly int nCols;
    }
}
