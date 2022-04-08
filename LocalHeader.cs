using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zip
{
    public class LocalHeader
    {
        private readonly uint magicNumber = 0x04034b50;
        private readonly ushort version = 20;
        private readonly ushort flag = 0;
        private ushort method;
        private ushort time {
            get
            {
                return 0;
            }
        }
        private ushort date
        {
            get
            {
                return 0;
            }
        }
        private uint crc32 = 0;
        private uint compressedSize = 0;
        private uint uncompressedSize = 0;
        private ushort fileNameSize
        {
            get => (ushort)name.Length;
        }
        private readonly ushort extraField = 0;
        private string name;

        public LocalHeader(ushort method,uint crc32,uint compressedSize,uint uncompressedSize,string name)
        {
            this.method=method;
            this.crc32 = crc32;
            this.compressedSize = compressedSize;
            this.uncompressedSize = uncompressedSize;
            this.name = name;
        }
        public void Write(BinaryWriter w)
        {
            w.Write(magicNumber);
            w.Write(version);
            w.Write(flag);
            w.Write(method);
            w.Write(time);
            w.Write(date);
            w.Write(crc32);
            w.Write(compressedSize);
            w.Write(uncompressedSize);
            w.Write(fileNameSize);
            w.Write(extraField);
            w.Write(Encoding.UTF8.GetBytes(name));
        }
    }
}
