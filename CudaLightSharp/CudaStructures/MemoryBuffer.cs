using CudaLightSharp.CudaEnumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CudaLightSharp.CudaStructures
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal class MemoryBuffer
    {
        public UIntPtr pointer;
        public MemorySpace memorySpace;
        public MathDomain mathDomain;
        public int size;

        public MemoryBuffer(UIntPtr pointer = default(UIntPtr),
                            int size = 0,
                            MemorySpace memorySpace = MemorySpace.Null,
                            MathDomain mathDomain = MathDomain.Null)
        {
            this.pointer = pointer;
            this.memorySpace = memorySpace;
            this.mathDomain = mathDomain;
            this.size = size;
        }

        public MemoryBuffer(MemoryBuffer rhs)
            : this(rhs.pointer, rhs.size, rhs.memorySpace, rhs.mathDomain)
        {
        }

        public int ElementarySize()
        {
            switch (mathDomain)
            {
                case MathDomain.Double:
                    return sizeof(double);
                case MathDomain.Float:
                    return sizeof(float);
                case MathDomain.Int:
                    return sizeof(int);
                default:
                    return 0;
            }
        }

        public int TotalSize => size * ElementarySize();
    }
}
