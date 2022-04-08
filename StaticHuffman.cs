using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zip
{
    public static class StaticHuffman
    {
        #region Consts
        private static int[] deflate_length_slot_base = {
            3,    4,    5,    6,    7,    8,    9,    10,
            11,   13,   15,   17,   19,   23,   27,   31,
            35,   43,   51,   59,   67,   83,   99,   115,
            131,  163,  195,  227,  258,
        };
        private static int[] deflate_extra_length_bits = {
            0,    0,    0,    0,    0,    0,    0,    0,
            1,    1,    1,    1,    2,    2,    2,    2,
            3,    3,    3,    3,    4,    4,    4,    4,
            5,    5,    5,    5,    0,
        };
        private static int[] deflate_offset_slot_base = {
            1,     2,     3,     4,     5,     7,     9,     13,
            17,    25,    33,    49,    65,    97,    129,   193,
            257,   385,   513,   769,   1025,  1537,  2049,  3073,
            4097,  6145,  8193,  12289, 16385, 24577,
        };
        private static int[] deflate_extra_offset_bits = {
            0,     0,     0,     0,     1,     1,     2,     2,
            3,     3,     4,     4,     5,     5,     6,     6,
            7,     7,     8,     8,     9,     9,     10,    10,
            11,    11,    12,    12,    13,    13,
        };
        private static int[] deflate_length_slot = {
            0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12, 12,
            12, 13, 13, 13, 13, 14, 14, 14, 14, 15, 15, 15, 15, 16, 16, 16, 16, 16,
            16, 16, 16, 17, 17, 17, 17, 17, 17, 17, 17, 18, 18, 18, 18, 18, 18, 18,
            18, 19, 19, 19, 19, 19, 19, 19, 19, 20, 20, 20, 20, 20, 20, 20, 20, 20,
            20, 20, 20, 20, 20, 20, 20, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21,
            21, 21, 21, 21, 21, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22,
            22, 22, 22, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23,
            23, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24,
            24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 25, 25, 25,
            25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25,
            25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 26, 26, 26, 26, 26, 26, 26,
            26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26,
            26, 26, 26, 26, 26, 26, 26, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27,
            27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27,
            27, 27, 28,
        };

        #endregion
        public static int[] ToBinary(int x, int n)
        {
            var res = new List<int>();
            while (x > 0)
            {
                res.Add(x & 1);
                if (res.Count > n)
                    throw new Exception();
                x >>= 1;
            }
            while (res.Count < n)
                res.Add(0);
            //res.ForEach(x => Console.Write($" {x}"));
            //Console.WriteLine();
            res.Reverse();
            Debug.Assert(res.Count == n);
            return res.ToArray();
        }
        public static int[] Literal(int literal)
        {
            if (literal >= 0 && literal <= 143)
                return ToBinary(0b00110000 + literal, 8);
            else if (literal >= 144 && literal <= 255)
                return ToBinary(0b110010000 + (literal - 144), 9);
            else if (literal == 256)
                return new int[7];
            else
                throw new Exception();
        }
        public static int[] Offset(int offset)
        {
            int slot = 0;
            while(slot< deflate_offset_slot_base.Length - 1 && deflate_offset_slot_base[slot + 1] <= offset)
                slot++;
            Console.WriteLine($"offset {offset} slot {slot}");
            int extra=offset - deflate_offset_slot_base[slot];
            var extraBits = ToBinary(extra, deflate_extra_offset_bits[slot]);
            var res=ToBinary(slot, 5).Concat(extraBits.Reverse()).ToArray();
            return res;
        }
        public static int[] Length(int length)
        {
            int slot = deflate_length_slot[length];
            int code = slot + 257;
            int extra = length - deflate_length_slot_base[slot];
            var extraBits = ToBinary(extra, deflate_extra_length_bits[slot]).Reverse();
            if (257 <= code && code <= 279)
            {
                return ToBinary(code - 256, 7).Concat(extraBits).ToArray();
            }
            else if (280 <= code && code <= 287)
            {
                return ToBinary(0b11000000 + code - 280, 8).Concat(extraBits).ToArray();
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
