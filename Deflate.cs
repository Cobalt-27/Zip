using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zip
{
    public class Deflate
    {
        private IList<byte> data;
        private Dictionary<(byte, byte, byte), int> dict = new Dictionary<(byte, byte, byte), int>();
        public Deflate(IList<byte> data)
        {
            this.data = data;
        }
        private (byte, byte, byte) Hash(int idx)
        {
            Debug.Assert(idx + 2 < data.Count);
            return (data[idx], data[idx + 1], data[idx + 2]);
        }
        private int GetLength(int idx1, int idx2)
        {
            int res = 0;
            int initial2 = idx2;
            while (idx1 < initial2 && idx2 < data.Count&& data[idx1] == data[idx2] && res<250)
            {
                res++;
                idx1++;
                idx2++;
            }
            return res;
        }
        public void Write(BitWriter w)
        {
            w.WriteBits(new int[] { 1, 1, 0 });
            int idx = 0;
            while (idx < data.Count)
            {
                if (idx>=data.Count-4)
                {
                    w.WriteBits(StaticHuffman.Literal(data[idx]));
                    idx++;
                    continue;
                }
                var hash = Hash(idx);
                int offset = dict.ContainsKey(hash)?idx - dict[hash]:int.MaxValue;
                dict[hash] = idx;
                if (offset>3&&offset <= 32768)
                {
                    int length = 3 + GetLength(idx-offset+ 3, idx + 3);
                    //Console.WriteLine($"idx {idx} pre {idx - offset} offset {offset} length {length}");
                    w.WriteBits(StaticHuffman.Length(length));
                    w.WriteBits(StaticHuffman.Offset(offset));
                    for(int j=idx+1;j< idx+length; j++)
                        dict[Hash(j)] = j;
                    idx += length;
                    continue;
                }
                w.WriteBits(StaticHuffman.Literal(data[idx]));
                idx++;
            }
            w.WriteBits(StaticHuffman.Literal(256));
            w.Emit();
        }
    }
}
