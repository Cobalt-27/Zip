// See https://aka.ms/new-console-template for more information
using Zip;
using Force.Crc32;

string name = "Mars.jpg";
string dst = "mars.zip";
var data = new List<byte>();
using (var stream = File.Open(name, FileMode.Open))
{
    var ms= new MemoryStream();
    stream.CopyTo(ms);
    data = ms.ToArray().ToList();
}

using (var stream = File.Open(dst, FileMode.OpenOrCreate))
{
    using (var w = new BinaryWriter(stream))
    {
        var timeStart = DateTime.UtcNow;
        var ms=new MemoryStream();
        var bw=new BitWriter(ms);
        var deflate = new Deflate(data);
        ushort method = 8;
        deflate.Write(bw);
        uint compressedSize = (uint)bw.MemoryStream.Length;
        Console.WriteLine($"Compressed size in bytes: {compressedSize}");
        uint crc32 = Crc32Algorithm.Compute(data.ToArray(), 0, data.Count);
        uint uncompressedSize = (uint)data.Count;

        var localh = new LocalHeader(8, crc32, compressedSize, uncompressedSize, name);
        localh.Write(w);

        if (method == 0)
            w.Write(data.ToArray());
        else
            w.Write(bw.MemoryStream.ToArray());

        uint start=(uint)w.BaseStream.Position;
        var record = new CentralDirectoryRecord(8, crc32, compressedSize, uncompressedSize, name);
        record.Write(w);

        var end = new CentralDirectoryRecordEnd(record.Size, start);
        end.Write(w);

        Console.WriteLine($"Time used: {(DateTime.UtcNow - timeStart).TotalSeconds}s");
    }
}
