﻿using SPICA.Formats.Common;
using SPICA.Serialization;
using SPICA.Serialization.Attributes;
using System.IO;

namespace SPICA.Formats.CtrGfx
{
    public class GfxStringUtf16LE : ICustomSerialization
    {
        [Ignore] private string Str;

        public GfxStringUtf16LE() { }

        public GfxStringUtf16LE(string Str)
        {
            this.Str = Str;
        }

        public override string ToString()
        {
            return Str ?? string.Empty;
        }

        void ICustomSerialization.Deserialize(ref StreamWriter OutputFile, BinaryDeserializer Deserializer)
        {
            Str = Deserializer.Reader.ReadNullTerminatedStringUtf16LE();
        }

        bool ICustomSerialization.Serialize(BinarySerializer Serializer)
        {
            Serializer.Writer.WriteNullTerminatedStringUtf16LE(Str);

            return true;
        }
    }
}
