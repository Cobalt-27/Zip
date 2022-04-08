// See https://aka.ms/new-console-template for more information
using Zip;
using Force.Crc32;

string name = "test.txt";
string dst = "test.zip";
var data = new List<byte>();

using (var stream = File.Open(name, FileMode.Open))
{
    var ms= new MemoryStream();
    stream.CopyTo(ms);
    data = ms.ToArray().ToList();
}

using (var stream = File.Open(dst, FileMode.Create))
{
    using (var w = new BinaryWriter(stream))
    {
        uint crc32 = Crc32Algorithm.Compute(data.ToArray(), 0, data.Count);
        uint uncompressedSize = (uint)data.Count;
        uint compressedSize = (uint)data.Count;
        var localh = new LocalHeader(0, crc32, compressedSize, uncompressedSize, name);
        localh.Write(w);

        w.Write(data.ToArray());

        uint start=(uint)w.BaseStream.Position;
        Console.WriteLine($"Start is {start}");
        var record = new CentralDirectoryRecord(0, crc32, compressedSize, uncompressedSize, name);
        record.Write(w);

        var end = new CentralDirectoryRecordEnd(record.Size, start);
        end.Write(w);
    }
}
