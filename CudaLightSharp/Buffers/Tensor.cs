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
    public static class TensorExtension
    {
        public static void Print<T>(this T[,,] tensor, int nRows, int nCols, int nCubes, string label = "") where T : struct, IEquatable<T>, IFormattable
        {
            Debug.Assert(nRows * nCols * nCubes == tensor.Length);
            Console.WriteLine("************** " + label + " **************");
            for (uint k = 0; k < nCubes; ++k)
            {
                for (uint j = 0; j < nCols; ++j)
                {
                    for (uint i = 0; i < nRows; ++i)
                        Console.Write(String.Format("m[{0},{1},{2}]={3} ", i, j, k, tensor[i, j, k]));
                    Console.WriteLine();
                }
            }
        }
        public static void Print<T>(this T[] tensor, int nRows, int nCols, int nCubes, string label = "") where T : struct, IEquatable<T>, IFormattable
        {
            Debug.Assert(nRows * nCols * nCubes == tensor.Length);
            Console.WriteLine("************** " + label + " **************");
            for (uint k = 0; k < nCubes; ++k)
            {
                for (uint j = 0; j < nCols; ++j)
                {
                    for (uint i = 0; i < nRows; ++i)
                        Console.Write(String.Format("m[{0},{1}]={2} ", i, j, tensor[i + j * nRows + k * nRows * nCols]));
                    Console.WriteLine();
                }
            }
        }
    }

    public unsafe class Tensor : Buffer
    {
        public Tensor(int nRows, int nCols, int nCubes, MemorySpace memorySpace = MemorySpace.Device, MathDomain mathDomain = MathDomain.Float)
            : base(true, memorySpace, mathDomain)
        {
            _buffer = new MemoryCube(0, (uint)nRows, (uint)nCols, (uint)nCubes, memorySpace, mathDomain);
            ctor(_buffer);

            cubes = new ColumnWiseMatrix[nCubes];
            uint shift = _buffer.ElementarySize();

            for (int i = 0; i < nCubes; i++)
            {
                MemoryTile columnBuffer = new MemoryTile(_buffer.pointer + (PtrT)(i * nRows * nCols * shift), (uint)nRows, (uint)nCols, memorySpace, mathDomain);
                cubes[i] = new ColumnWiseMatrix(columnBuffer);
            }
        }

        public Tensor(int nRows, int nCols, int nCubes, double value, MemorySpace memorySpace = MemorySpace.Device, MathDomain mathDomain = MathDomain.Float)
            : this(nRows, nCols, nCubes, memorySpace, mathDomain)
        {
            Set(value);
        }

        public Tensor(Tensor rhs)
            : this(rhs.nRows, rhs.nCols, rhs.nCubes, rhs.memorySpace, rhs.mathDomain)
        {
            ReadFrom(rhs);
        }

        public Tensor(ColumnWiseMatrix rhs)
            : this(rhs.nRows, rhs.nCols, 1, rhs.memorySpace, rhs.mathDomain)
        {
            ReadFrom(rhs);
        }

        public Tensor(Vector rhs)
            : this(rhs.Size, 1, 1, rhs.memorySpace, rhs.mathDomain)
        {
            ReadFrom(rhs);
        }

        #region Read from double

        public Tensor(double[] rhs)
            : this(rhs.Length, 1, 1, MemorySpace.Device, MathDomain.Double)
        {
            ReadFrom(rhs, rhs.Length);
        }

        public Tensor(double[,] rhs)
            : this(rhs.GetLength(0), rhs.GetLength(1), 1, MemorySpace.Device, MathDomain.Double)
        {
            ReadFrom(rhs);
        }

        public Tensor(double[,,] rhs)
            : this(rhs.GetLength(0), rhs.GetLength(1), rhs.GetLength(2), MemorySpace.Device, MathDomain.Double)
        {
            ReadFrom(rhs);
        }

        public Tensor(Matrix<double>[] rhs)
            : this(rhs[0].RowCount, rhs[0].ColumnCount, rhs.Length, MemorySpace.Device, MathDomain.Double)
        {
            Vector<double> _rhs = Vector<double>.Build.Dense(Size);
            for (int i = 0; i < rhs.Length; i++)
                _rhs.SetSubVector(i * nRows * nCols, nRows * nCols, Vector<double>.Build.DenseOfArray(rhs[i].ToColumnMajorArray()));
                
            ReadFrom(_rhs);
        }

        public Tensor(Vector<double> rhs)
            : this(rhs.ToArray())
        {
        }

        #endregion

        #region Read from float

        public Tensor(float[] rhs)
            : this(rhs.Length, 1, 1, MemorySpace.Device, MathDomain.Float)
        {
            ReadFrom(rhs, rhs.Length);
        }

        public Tensor(float[,] rhs)
            : this(rhs.GetLength(0), rhs.GetLength(1), 1, MemorySpace.Device, MathDomain.Float)
        {
            ReadFrom(rhs);
        }

        public Tensor(float[,,] rhs)
            : this(rhs.GetLength(0), rhs.GetLength(1), rhs.GetLength(2), MemorySpace.Device, MathDomain.Float)
        {
            ReadFrom(rhs);
        }

        public Tensor(Matrix<float>[] rhs)
            : this(rhs[0].RowCount, rhs[0].ColumnCount, rhs.Length, MemorySpace.Device, MathDomain.Float)
        {
            Vector<float> _rhs = Vector<float>.Build.Dense(Size);
            for (int i = 0; i < rhs.Length; i++)
                _rhs.SetSubVector(i * nRows * nCols, nRows * nCols, Vector<float>.Build.DenseOfArray(rhs[i].ToColumnMajorArray()));

            ReadFrom(_rhs);
        }

        public Tensor(Vector<float> rhs)
            : this(rhs.ToArray())
        {
        }

        #endregion

        #region Read from int

        public Tensor(int[] rhs)
            : this(rhs.Length, 1, 1, MemorySpace.Device, MathDomain.Int)
        {
            ReadFrom(rhs, rhs.Length);
        }

        public Tensor(int[,] rhs)
            : this(rhs.GetLength(0), rhs.GetLength(1), 1, MemorySpace.Device, MathDomain.Int)
        {
            ReadFrom(rhs);
        }

        public Tensor(int[,,] rhs)
            : this(rhs.GetLength(0), rhs.GetLength(1), rhs.GetLength(2), MemorySpace.Device, MathDomain.Int)
        {
            ReadFrom(rhs);
        }

        public Tensor(Matrix<int>[] rhs)
            : this(rhs[0].RowCount, rhs[0].ColumnCount, rhs.Length, MemorySpace.Device, MathDomain.Int)
        {
            Vector<int> _rhs = Vector<int>.Build.Dense(Size);
            for (int i = 0; i < rhs.Length; i++)
                _rhs.SetSubVector(i * nRows * nCols, nRows * nCols, Vector<int>.Build.DenseOfArray(rhs[i].ToColumnMajorArray()));

            ReadFrom(_rhs);
        }

        public Tensor(Vector<int> rhs)
            : this(rhs.ToArray())
        {
        }

        #endregion

        public void Set(ColumnWiseMatrix matrix, int row)
        {
            cubes[row].ReadFrom(matrix);
        }

        public void Set(Vector vector, int row, int col)
        {
            Column(row, col).ReadFrom(vector);
        }

        public void Set<T>(uint row, uint col, T value) where T : struct, IEquatable<T>, IFormattable
        {
            Column((int)row, (int)col).Set(value);
        }

        public void Set<T>(uint col, T value) where T : struct, IEquatable<T>, IFormattable
        {
            cubes[col].Set(value);
        }

        public Vector<T> Get<T>(int row, int col) where T : struct, IEquatable<T>, IFormattable
        {
            return Column(row, col).Get<T>();
        }

        public Matrix<T> GetMatrix<T>(int row) where T : struct, IEquatable<T>, IFormattable
        {
            return cubes[row].GetMatrix<T>();
        }

        public Matrix<T>[] GetCube<T>() where T : struct, IEquatable<T>, IFormattable
        {
            var ret = new Matrix<T>[nCubes];
            for (int i = 0; i < ret.Length; i++)
                ret[i] = GetMatrix<T>(i);

            return ret;
        }

        public static Tensor LinSpace(int nRows, int nCols, int nCubes, double x0, double x1, MemorySpace memorySpace = MemorySpace.Device, MathDomain mathDomain = MathDomain.Float)
        {
            var tensor = new Tensor(nRows, nCols, nCubes, memorySpace, mathDomain);
            tensor.LinSpace(x0, x1);

            return tensor;
        }

        public static Tensor RandomUniform(int nRows, int nCols, int nCubes, int seed, MemorySpace memorySpace = MemorySpace.Device, MathDomain mathDomain = MathDomain.Float)
        {
            var tensor = new Tensor(nRows, nCols, nCubes, memorySpace, mathDomain);
            tensor.RandomUniform(seed);

            return tensor;
        }

        public static Tensor RandomGaussian(int nRows, int nCols, int nCubes, int seed, MemorySpace memorySpace = MemorySpace.Device, MathDomain mathDomain = MathDomain.Float)
        {
            var tensor = new Tensor(nRows, nCols, nCubes, memorySpace, mathDomain);
            tensor.RandomGaussian(seed);

            return tensor;
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
                        thisVec.Print(nRows, nCols, nCubes, label);
                        break;
                    }
                case MathDomain.Float:
                    {
                        var thisVec = GetRaw<float>();
                        thisVec.Print(nRows, nCols, nCubes, label);
                        break;
                    }
                case MathDomain.Double:
                    {
                        var thisVec = GetRaw<double>();
                        thisVec.Print(nRows, nCols, nCubes, label);
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        public int nRows => (int)_buffer.nRows;
        public int nCols => (int)_buffer.nCols;
        public int nCubes => (int)_buffer.nCubes;

        public ColumnWiseMatrix Cube(int i) { return cubes[i]; }

        public Matrix<T> GetCube<T>(int i) where T : struct, IEquatable<T>, IFormattable
        {
            return Cube(i).GetMatrix<T>();
        }

        public Vector Column(int i, int j) { return Cube(i).columns[j]; }

        public Vector<T> GetColumn<T>(int i, int j) where T : struct, IEquatable<T>, IFormattable
        {
            return Cube(i).GetColumn<T>(j);
        }

        MemoryCube _buffer;
        internal override MemoryBuffer buffer => _buffer;
        internal readonly ColumnWiseMatrix[] cubes;
    }
}
