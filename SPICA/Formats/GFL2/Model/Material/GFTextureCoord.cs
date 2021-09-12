using SPICA.Formats.Common;
using SPICA.Math3D;
using SPICA.Misc;
using System.IO;
using System.Numerics;

namespace SPICA.Formats.GFL2.Model.Material
{
    public struct GFTextureCoord : INamed
    {
        public string Name { get; set; }

        public byte UnitIndex;

        public GFTextureMappingType MappingType;

        public Vector2 Scale;
        public float Rotation;
        public Vector2 Translation;

        public GFTextureWrap WrapU;
        public GFTextureWrap WrapV;

        public GFMagFilter MagFilter;
        public GFMinFilter MinFilter;

        public uint MinLOD;

        public GFTextureCoord(ref StreamWriter outputFile, LogReader Reader)
        {
            Name = new GFHashName(ref outputFile, Reader).Name;

            UnitIndex = Reader.ReadByte(ref outputFile);

            MappingType = (GFTextureMappingType)Reader.ReadByte();

            Scale = Reader.ReadVector2(ref outputFile);
            Rotation = Reader.ReadSingle(ref outputFile);
            Translation = Reader.ReadVector2(ref outputFile);

            WrapU = (GFTextureWrap)Reader.ReadUInt32(ref outputFile);
            WrapV = (GFTextureWrap)Reader.ReadUInt32(ref outputFile);

            MagFilter = (GFMagFilter)Reader.ReadUInt32(ref outputFile); //Not sure
            MinFilter = (GFMinFilter)Reader.ReadUInt32(ref outputFile); //Not sure

            MinLOD = Reader.ReadUInt32(ref outputFile); //Not sure
        }

        public Matrix3x4 GetTransform()
        {
            return TextureTransform.GetTransform(
                Scale,
                Rotation,
                Translation,
                TextureTransformType.DccMaya);
        }

        public void Write(BinaryWriter Writer)
        {
            new GFHashName(Name).Write(Writer);

            Writer.Write(UnitIndex);

            Writer.Write((byte)MappingType);

            Writer.Write(Scale);
            Writer.Write(Rotation);
            Writer.Write(Translation);

            Writer.Write((uint)WrapU);
            Writer.Write((uint)WrapV);

            Writer.Write((uint)MagFilter);
            Writer.Write((uint)MinFilter);

            Writer.Write(MinLOD);
        }
    }
}
