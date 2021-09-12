using SPICA.Math3D;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SPICA.Misc
{
    public class LogReader : BinaryReader
    {
        public LogReader(Stream BaseStream) : base(BaseStream)
        { }



        public ulong ReadUInt64(ref StreamWriter outputFile)
        {
            long position = this.BaseStream.Position;
            ulong value = this.ReadUInt64();
            outputFile.WriteLine(String.Format("{0} | UINT64: {1}", position, value));
            outputFile.Flush();
            return value;
        }

        public uint ReadUInt32(ref StreamWriter outputFile)
        {
            long position = this.BaseStream.Position;
            uint value = this.ReadUInt32();
            outputFile.WriteLine(String.Format("{0} | UINT32: {1}", position, value));
            outputFile.Flush();
            return value;
        }

        public ushort ReadUInt16(ref StreamWriter outputFile)
        {
            long position = this.BaseStream.Position;
            ushort value = this.ReadUInt16();
            outputFile.WriteLine(String.Format("{0} | UINT16: {1}", position, value));
            outputFile.Flush();
            return value;
        }

        public byte ReadByte(ref StreamWriter outputFile)
        {
            long position = this.BaseStream.Position;
            byte value = this.ReadByte();
            outputFile.WriteLine(String.Format("{0} | UINT8: {1}", position, value));
            outputFile.Flush();
            return value;
        }


        public long ReadInt64(ref StreamWriter outputFile)
        {
            long position = this.BaseStream.Position;
            long value = this.ReadInt64();
            outputFile.WriteLine(String.Format("{0} | INT64: {1}", position, value));
            outputFile.Flush();
            return value;
        }

        public int ReadInt32(ref StreamWriter outputFile)
        {
            long position = this.BaseStream.Position;
            int value = this.ReadInt32();
            outputFile.WriteLine(String.Format("{0} | INT32: {1}", position, value));
            outputFile.Flush();
            return value;
        }

        public short ReadInt16(ref StreamWriter outputFile)
        {
            long position = this.BaseStream.Position;
            short value = this.ReadInt16();
            outputFile.WriteLine(String.Format("{0} | INT16: {1}", position, value));
            outputFile.Flush();
            return value;
        }

        public sbyte ReadSByte(ref StreamWriter outputFile)
        {
            long position = this.BaseStream.Position;
            sbyte value = this.ReadSByte();
            outputFile.WriteLine(String.Format("{0} | INT8: {1}", position, value));
            outputFile.Flush();
            return value;
        }



        public float ReadSingle(ref StreamWriter outputFile)
        {
            long position = this.BaseStream.Position;
            float value = this.ReadSingle();
            outputFile.WriteLine(String.Format("{0} | FLOAT32: {1}", position, value));
            outputFile.Flush();
            return value;
        }

        public double ReadDouble(ref StreamWriter outputFile)
        {
            long position = this.BaseStream.Position;
            double value = this.ReadDouble();
            outputFile.WriteLine(String.Format("{0} | FLOAT64: {1}", position, value));
            outputFile.Flush();
            return value;
        }



        public string ReadString(ref StreamWriter outputFile)
        {
            long position = this.BaseStream.Position;
            StringBuilder SB = new StringBuilder();

            for (char Chr; (Chr = this.ReadChar()) != '\0';)
            {
                SB.Append(Chr);
            }

            outputFile.WriteLine(String.Format("{0} | STRING: {1}", position, SB.ToString()));
            outputFile.Flush();
            return SB.ToString();
        }



        public bool ReadBoolUInt32(ref StreamWriter outputFile)
        {
            long position = this.BaseStream.Position;
            uint temp = this.ReadUInt32();
            bool value = temp != 0;
            outputFile.WriteLine(String.Format("{0} | BOOL (UINT32): {1} ({2})", position, value, temp));
            outputFile.Flush();
            return value;
        }



        public Vector2 ReadVector2(ref StreamWriter outputFile)
        {
            long position = this.BaseStream.Position;

            float x = this.ReadSingle();
            float y = this.ReadSingle();
            Vector2 result = new Vector2(x, y);

            outputFile.WriteLine(String.Format("{0} | VECTOR2: {1}", position, result));
            outputFile.Flush();
            return result;
        }

        public Vector3 ReadVector3(ref StreamWriter outputFile)
        {
            long position = this.BaseStream.Position;

            float x = this.ReadSingle();
            float y = this.ReadSingle();
            float z = this.ReadSingle();
            Vector3 result = new Vector3(x, y, z);

            outputFile.WriteLine(String.Format("{0} | VECTOR3: {1}", position, result));
            outputFile.Flush();
            return result;
        }

        public Vector4 ReadVector4(ref StreamWriter outputFile)
        {
            long position = this.BaseStream.Position;

            float x = this.ReadSingle();
            float y = this.ReadSingle();
            float z = this.ReadSingle();
            float w = this.ReadSingle();
            Vector4 result = new Vector4(x, y, z, w);

            outputFile.WriteLine(String.Format("{0} | VECTOR4: {1}", position, result));
            outputFile.Flush();
            return result;
        }

        public Quaternion ReadQuaternion(ref StreamWriter outputFile)
        {
            long position = this.BaseStream.Position;

            float x = this.ReadSingle();
            float y = this.ReadSingle();
            float z = this.ReadSingle();
            float w = this.ReadSingle();
            Quaternion result = new Quaternion(x, y, z, w);

            outputFile.WriteLine(String.Format("{0} | QUATERNION: {1}", position, result));
            outputFile.Flush();
            return result;
        }



        public Matrix3x3 ReadMatrix3x3(ref StreamWriter outputFile)
        {
            long position = this.BaseStream.Position;
            Matrix3x3 result = new Matrix3x3()
            {
                M11 = this.ReadSingle(),
                M21 = this.ReadSingle(),
                M31 = this.ReadSingle(),
                M12 = this.ReadSingle(),
                M22 = this.ReadSingle(),
                M32 = this.ReadSingle(),
                M13 = this.ReadSingle(),
                M23 = this.ReadSingle(),
                M33 = this.ReadSingle()
            };

            outputFile.WriteLine(String.Format("{0} | MATRIX3x3: {1}", position, result));
            outputFile.Flush();
            return result;
        }

        public Matrix3x4 ReadMatrix3x4(ref StreamWriter outputFile)
        {
            long position = this.BaseStream.Position;
            Matrix3x4 result = new Matrix3x4()
            {
                M11 = this.ReadSingle(),
                M21 = this.ReadSingle(),
                M31 = this.ReadSingle(),
                M41 = this.ReadSingle(),
                M12 = this.ReadSingle(),
                M22 = this.ReadSingle(),
                M32 = this.ReadSingle(),
                M42 = this.ReadSingle(),
                M13 = this.ReadSingle(),
                M23 = this.ReadSingle(),
                M33 = this.ReadSingle(),
                M43 = this.ReadSingle()
            };

            outputFile.WriteLine(String.Format("{0} | MATRIX3x4: {1}", position, result));
            outputFile.Flush();
            return result;
        }

        public Matrix4x4 ReadMatrix4x4(ref StreamWriter outputFile)
        {
            long position = this.BaseStream.Position;
            Matrix4x4 result = new Matrix4x4()
            {
                M11 = this.ReadSingle(),
                M21 = this.ReadSingle(),
                M31 = this.ReadSingle(),
                M41 = this.ReadSingle(),
                M12 = this.ReadSingle(),
                M22 = this.ReadSingle(),
                M32 = this.ReadSingle(),
                M42 = this.ReadSingle(),
                M13 = this.ReadSingle(),
                M23 = this.ReadSingle(),
                M33 = this.ReadSingle(),
                M43 = this.ReadSingle(),
                M14 = this.ReadSingle(),
                M24 = this.ReadSingle(),
                M34 = this.ReadSingle(),
                M44 = this.ReadSingle()
            };

            outputFile.WriteLine(String.Format("{0} | MATRIX4x4: {1}", position, result));
            outputFile.Flush();
            return result;
        }
    }
}
