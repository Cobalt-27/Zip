using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zip
{
    public static class StaticHuffman
    {
        public static int[] ToBinary(int x, int n)
        {
            var res = new List<int>();
            while (x > 0)
            {
                res.Add(x & 1);
                x >>= 1;
            }
            while (res.Count < n)
                res.Add(0);
            //res.ForEach(x => Console.Write($" {x}"));
            Console.WriteLine();
            res.Reverse();
            return res.ToArray();
        }
        public static int[] Literal(int literal)
        {
            if (literal >= 0 && literal <= 143)
            {
                var code = 0b00110000 + literal;
                return ToBinary(code, 8);
            }
            else if (literal >= 144 && literal <= 255)
            {
                var code = 0b110010000 + (literal - 144);
                return ToBinary(code, 9);
            }
            else if (literal == 256)
            {
                return new int[7];
            }
            else
                throw new Exception();
        }
        public static int[] Distance(int distance)
        {
            throw new NotImplementedException();
        }
        public static int[] Length(int length)
        {
            throw new NotImplementedException();
        }
    }
}
