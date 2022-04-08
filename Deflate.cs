using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zip
{
    public class Deflate
    {
        private IList<byte> data;
        private Dictionary<(byte, byte, byte, byte), int> dict = new Dictionary<(byte, byte, byte, byte), int>();
        public Deflate(IList<byte> data)
        {
            this.data = data;
        }
        private (byte, byte, byte, byte) Hash(int idx)
        {
            if (idx + 4 > data.Count)
                throw new ArgumentException();
            return (data[idx], data[idx + 1], data[idx + 2], data[idx + 3]);
        }
        private int GetLength(int idx1, int idx2)
        {
            int res = 0;
            int initial2 = idx2;
            while (idx1 < initial2 && data[idx1] == data[idx2] && idx2 < data.Count && res<250)
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
                if (true)
                {
                    w.WriteBits(StaticHuffman.Literal(data[idx]));
                    idx++;
                    continue;
                }
                var hash = Hash(idx);
                int distance = dict.ContainsKey(hash)?idx - dict[hash]:int.MaxValue;
                dict[hash] = idx;
                if (distance <= 32768)
                {
                    int length = 4 + GetLength(dict[hash] + 4, idx + 4);
                    w.WriteBits(StaticHuffman.Distance(distance));
                    w.WriteBits(StaticHuffman.Length(length));
                    for(int j=idx+1;j< idx+length; j++)
                        dict[Hash(j)] = j;
                    idx += length;
                }
                else
                {
                    w.WriteBits(StaticHuffman.Literal(data[idx]));
                    idx++;
                }
            }
            w.WriteBits(StaticHuffman.Literal(256));
            w.Emit();
        }
    }
}
