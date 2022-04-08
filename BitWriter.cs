using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zip
{
    public class BitWriter
    {
        public MemoryStream MemoryStream { get; private set; }
        private int value;
        private int len=0;
        public BitWriter(MemoryStream stream)
        {
            MemoryStream = stream;
        }
        public void WriteBits(IEnumerable<int> bits)
        {
            bits.ToList().ForEach(x =>WriteBit(x));
        }
        public void WriteBit(int x)
        {
            if (x != 0 && x !=1)
                throw new ArgumentException();
            value |= x<<len;
            if (++len >= 8)
                Emit();
        }
        public void Emit()
        {
            //Console.WriteLine($"write byte {value}");
            MemoryStream.WriteByte((byte)value);
            value = 0;
            len = 0;
        }
    }
}
