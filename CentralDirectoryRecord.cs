using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zip
{
    public class CentralDirectoryRecord
    {
        private readonly uint magicNumber = 0x02014b50;
        private readonly ushort versionMadeBy = 10;
        private readonly ushort version = 0x0014;
        private readonly ushort flag = 0;
        private ushort method = 0x0008;
        private ushort time { get => 0; }
        private ushort date { get => 0; }
        private uint crc32 = 0;
        private uint compressedSize = 0;
        private uint uncompressedSize = 0;
        private ushort fileNameSize { get => (ushort)name.Length; }
        private readonly ushort extraField = 0;
        private readonly ushort fileCommentLength = 0;
        private readonly ushort diskNumberStart = 0;
        private readonly ushort internalFileAttributes = 0;
        private readonly uint externalFileAttributes = 0;
        private readonly uint relativeOffsetOfLocalHeader = 0;
        private string name;
        public uint Size { get; private set; }

        public CentralDirectoryRecord(ushort method,uint crc32,uint compressedSize,uint uncompressedSize,string name)
        {
            this.method = method;
            this.crc32 = crc32;
            this.compressedSize = compressedSize;
            this.uncompressedSize = uncompressedSize;
            this.name = name;
        }

        public void Write(BinaryWriter w)
        {
            var start = w.BaseStream.Position;
            w.Write(magicNumber);
            w.Write(versionMadeBy);
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
            w.Write(fileCommentLength);
            w.Write(diskNumberStart);
            w.Write(internalFileAttributes);
            w.Write(externalFileAttributes);
            w.Write(relativeOffsetOfLocalHeader);
            w.Write(Encoding.UTF8.GetBytes(name));
            Size=(uint)(w.BaseStream.Position-start);
            Console.WriteLine($"Size of Directory: {Size}");
        }
    }
}
