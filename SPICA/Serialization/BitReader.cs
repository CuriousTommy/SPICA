using SPICA.Misc;
using System.IO;

namespace SPICA.Serialization
{
    class BitReader
    {
        private LogReader Reader;

        private uint Bools;
        private int Index;

        public BitReader(LogReader Reader)
        {
            this.Reader = Reader;
        }

        public bool ReadBit(ref StreamWriter outputFile)
        {
            if ((Index++ & 0x1f) == 0)
            {
                Bools = Reader.ReadUInt32(ref outputFile);
            }

            bool Value = (Bools & 1) != 0;

            Bools >>= 1;

            return Value;
        }
    }
}
