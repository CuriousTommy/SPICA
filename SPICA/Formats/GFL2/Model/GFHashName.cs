using SPICA.Formats.Common;
using SPICA.Misc;
using System.IO;

namespace SPICA.Formats.GFL2.Model
{
    public struct GFHashName
    {
        public uint Hash;
        public string Name;

        public GFHashName(string Name)
        {
            GFNV1 FNV = new GFNV1();

            FNV.Hash(Name);

            Hash = FNV.HashCode;

            this.Name = Name;
        }

        public GFHashName(ref StreamWriter outputFile, LogReader Reader)
        {
            Hash = Reader.ReadUInt32(ref outputFile);
            Name = Reader.ReadByteLengthString();
        }

        public void Write(BinaryWriter Writer)
        {
            Writer.Write(Hash);
            Writer.WriteByteLengthString(Name);
        }
    }
}
