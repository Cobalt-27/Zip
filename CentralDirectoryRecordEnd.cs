using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zip
{
    public class CentralDirectoryRecordEnd
    {
        private readonly uint magicNumber = 0x06054b50;
        private readonly ushort numberOfThisDisk = 0;
        private readonly ushort numberOfTheDiskWithTheStartOfTheCentralDirectory = 0;
        private readonly ushort totalNumberOfEntriesOnThisDisk = 1;
        private readonly ushort totalNumberOfEntries = 1;
        private readonly uint sizeOfTheCentralDirectory;
        private readonly uint startOfCentralDirectory;

        public CentralDirectoryRecordEnd(uint size,uint start)
        {
            sizeOfTheCentralDirectory = size;
            startOfCentralDirectory = start;
        }

        public void Write(BinaryWriter w)
        {
            w.Write(magicNumber);
            w.Write(numberOfThisDisk);
            w.Write(numberOfTheDiskWithTheStartOfTheCentralDirectory);
            w.Write(totalNumberOfEntriesOnThisDisk);
            w.Write(totalNumberOfEntries);
            w.Write(sizeOfTheCentralDirectory);
            w.Write(startOfCentralDirectory);
        }
    }
}
