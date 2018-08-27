using CudaLightSharp.CudaEnumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

#if FORCE_32_BIT
    using PtrT = System.UInt32;
#else
    using PtrT = System.UInt64;
#endif

namespace CudaLightSharp.CudaStructures
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class MemoryBuffer
    {
        public PtrT pointer;
        public MemorySpace memorySpace;
        public MathDomain mathDomain;
        public uint size;

        public MemoryBuffer(PtrT pointer = 0,
                            uint size = 0,
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

        public uint ElementarySize()
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

        public uint TotalSize => size * (uint)ElementarySize();
    }
}
