using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zip
{
    public class BitWriter
    {
        public MemoryStream MemoryStream { get; private set; }
        private int value;
        private int len = 0;
        public BitWriter(MemoryStream stream) => MemoryStream = stream;
        public void WriteBits(IList<int> bits) => bits.ToList().ForEach(x => WriteBit(x));
        public void WriteBit(int x)
        {
            Debug.Assert(x == 1 || x == 2);
            value |= x << len;
            if (++len >= 8)
                Emit();
        }
        public void Emit()
        {
            if (len != 0)
                MemoryStream.WriteByte((byte)value);
            value = 0;
            len = 0;
        }
    }
}
