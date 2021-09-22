using SPICA.Formats.Common;
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



        public ulong ReadUInt64(ref StreamWriter outputFile, String classRef = "")
        {
            long position = this.BaseStream.Position;
            ulong value = this.ReadUInt64();

            outputFile.WriteLine(String.Format("{0} | UINT64: {1}", position, value));
            if (!classRef.Equals(""))
            {
                outputFile.WriteLine(String.Format("{0} = {1}", classRef, value));
            }
            
            outputFile.Flush();
            return value;
        }

        public uint ReadUInt32(ref StreamWriter outputFile, String classRef = "")
        {
            long position = this.BaseStream.Position;
            uint value = this.ReadUInt32();
            outputFile.WriteLine(String.Format("{0} | UINT32: {1}", position, value));
            if (!classRef.Equals(""))
            {
                outputFile.WriteLine(String.Format("{0} = {1}", classRef, value));
            }

            outputFile.Flush();
            return value;
        }

        private uint ReadUInt24(ref StreamWriter outputFile, String classRef = "")
        {
            long position = this.BaseStream.Position;
            uint value = this.ReadUInt24();
            outputFile.WriteLine(String.Format("{0} | UINT24: {1}", position, value));
            if (!classRef.Equals(""))
            {
                outputFile.WriteLine(String.Format("{0} = {1}", classRef, value));
            }

            outputFile.Flush();
            return value;
        }

        public ushort ReadUInt16(ref StreamWriter outputFile, String classRef = "")
        {
            long position = this.BaseStream.Position;
            ushort value = this.ReadUInt16();
            outputFile.WriteLine(String.Format("{0} | UINT16: {1}", position, value));
            if (!classRef.Equals(""))
            {
                outputFile.WriteLine(String.Format("{0} = {1}", classRef, value));
            }

            outputFile.Flush();
            return value;
        }

        public byte ReadByte(ref StreamWriter outputFile, String classRef = "")
        {
            long position = this.BaseStream.Position;
            byte value = this.ReadByte();
            outputFile.WriteLine(String.Format("{0} | UINT8: {1}", position, value));
            if (!classRef.Equals(""))
            {
                outputFile.WriteLine(String.Format("{0} = {1}", classRef, value));
            }

            outputFile.Flush();
            return value;
        }


        public long ReadInt64(ref StreamWriter outputFile, String classRef = "")
        {
            long position = this.BaseStream.Position;
            long value = this.ReadInt64();
            outputFile.WriteLine(String.Format("{0} | INT64: {1}", position, value));
            if (!classRef.Equals(""))
            {
                outputFile.WriteLine(String.Format("{0} = {1}", classRef, value));
            }

            outputFile.Flush();
            return value;
        }

        public int ReadInt32(ref StreamWriter outputFile, String classRef = "")
        {
            long position = this.BaseStream.Position;
            int value = this.ReadInt32();
            outputFile.WriteLine(String.Format("{0} | INT32: {1}", position, value));
            if (!classRef.Equals(""))
            {
                outputFile.WriteLine(String.Format("{0} = {1}", classRef, value));
            }

            outputFile.Flush();
            return value;
        }

        public short ReadInt16(ref StreamWriter outputFile, String classRef = "")
        {
            long position = this.BaseStream.Position;
            short value = this.ReadInt16();
            outputFile.WriteLine(String.Format("{0} | INT16: {1}", position, value));
            if (!classRef.Equals(""))
            {
                outputFile.WriteLine(String.Format("{0} = {1}", classRef, value));
            }

            outputFile.Flush();
            return value;
        }

        public sbyte ReadSByte(ref StreamWriter outputFile, String classRef = "")
        {
            long position = this.BaseStream.Position;
            sbyte value = this.ReadSByte();
            outputFile.WriteLine(String.Format("{0} | INT8: {1}", position, value));
            if (!classRef.Equals(""))
            {
                outputFile.WriteLine(String.Format("{0} = {1}", classRef, value));
            }

            outputFile.Flush();
            return value;
        }



        public float ReadSingle(ref StreamWriter outputFile, String classRef = "")
        {
            long position = this.BaseStream.Position;
            float value = this.ReadSingle();
            outputFile.WriteLine(String.Format("{0} | FLOAT32: {1}", position, value));
            if (!classRef.Equals(""))
            {
                outputFile.WriteLine(String.Format("{0} = {1}", classRef, value));
            }

            outputFile.Flush();
            return value;
        }

        public double ReadDouble(ref StreamWriter outputFile, String classRef = "")
        {
            long position = this.BaseStream.Position;
            double value = this.ReadDouble();
            outputFile.WriteLine(String.Format("{0} | FLOAT64: {1}", position, value));
            if (!classRef.Equals(""))
            {
                outputFile.WriteLine(String.Format("{0} = {1}", classRef, value));
            }

            outputFile.Flush();
            return value;
        }



        public string ReadString(ref StreamWriter outputFile, String classRef = "")
        {
            long position = this.BaseStream.Position;
            StringBuilder SB = new StringBuilder();

            for (char Chr; (Chr = this.ReadChar()) != '\0';)
            {
                SB.Append(Chr);
            }

            String value = SB.ToString();
            outputFile.WriteLine(String.Format("{0} | STRING: {1}", position, SB.ToString()));
            if (!classRef.Equals(""))
            {
                outputFile.WriteLine(String.Format("{0} = {1}", classRef, value));
            }

            outputFile.Flush();
            return value;
        }

        public string ReadInt32LengthString(ref StreamWriter outputFile, String classRef = "")
        {
            long position = this.BaseStream.Position;
            String value = StringUtils.ReadPaddedString(this, ReadInt32(ref outputFile, "LogReader.ReadInt32LengthString(...) | [string length]"));

            outputFile.WriteLine(String.Format("{0} | STRING32: {1}", position, value));
            if (!classRef.Equals(""))
            {
                outputFile.WriteLine(String.Format("{0} = {1}", classRef, value));
            }

            return value;
        }



        public bool ReadBoolUInt32(ref StreamWriter outputFile, String classRef = "")
        {
            long position = this.BaseStream.Position;
            uint temp = this.ReadUInt32();
            bool value = temp != 0;
            outputFile.WriteLine(String.Format("{0} | BOOL (UINT32): {1} ({2})", position, value, temp));
            if (!classRef.Equals(""))
            {
                outputFile.WriteLine(String.Format("{0} = {1}", classRef, value));
            }

            outputFile.Flush();
            return value;
        }



        public Vector2 ReadVector2(ref StreamWriter outputFile, String classRef = "")
        {
            long position = this.BaseStream.Position;

            float x = this.ReadSingle();
            float y = this.ReadSingle();
            Vector2 value = new Vector2(x, y);

            outputFile.WriteLine(String.Format("{0} | VECTOR2: {1}", position, value));
            if (!classRef.Equals(""))
            {
                outputFile.WriteLine(String.Format("{0} = {1}", classRef, value));
            }

            outputFile.Flush();
            return value;
        }

        public Vector3 ReadVector3(ref StreamWriter outputFile, String classRef = "")
        {
            long position = this.BaseStream.Position;

            float x = this.ReadSingle();
            float y = this.ReadSingle();
            float z = this.ReadSingle();
            Vector3 value = new Vector3(x, y, z);

            outputFile.WriteLine(String.Format("{0} | VECTOR3: {1}", position, value));
            if (!classRef.Equals(""))
            {
                outputFile.WriteLine(String.Format("{0} = {1}", classRef, value));
            }

            outputFile.Flush();
            return value;
        }

        public Vector4 ReadVector4(ref StreamWriter outputFile, String classRef = "")
        {
            long position = this.BaseStream.Position;

            float x = this.ReadSingle();
            float y = this.ReadSingle();
            float z = this.ReadSingle();
            float w = this.ReadSingle();
            Vector4 value = new Vector4(x, y, z, w);

            outputFile.WriteLine(String.Format("{0} | VECTOR4: {1}", position, value));
            if (!classRef.Equals(""))
            {
                outputFile.WriteLine(String.Format("{0} = {1}", classRef, value));
            }

            outputFile.Flush();
            return value;
        }

        public Quaternion ReadQuaternion(ref StreamWriter outputFile, String classRef = "")
        {
            long position = this.BaseStream.Position;

            float x = this.ReadSingle();
            float y = this.ReadSingle();
            float z = this.ReadSingle();
            float w = this.ReadSingle();
            Quaternion value = new Quaternion(x, y, z, w);

            outputFile.WriteLine(String.Format("{0} | QUATERNION: {1}", position, value));
            if (!classRef.Equals(""))
            {
                outputFile.WriteLine(String.Format("{0} = {1}", classRef, value));
            }

            outputFile.Flush();
            return value;
        }



        public Matrix3x3 ReadMatrix3x3(ref StreamWriter outputFile, String classRef = "")
        {
            long position = this.BaseStream.Position;
            Matrix3x3 value = new Matrix3x3()
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

            outputFile.WriteLine(String.Format("{0} | MATRIX3x3: {1}", position, value));
            if (!classRef.Equals(""))
            {
                outputFile.WriteLine(String.Format("{0} = {1}", classRef, value));
            }

            outputFile.Flush();
            return value;
        }

        public Matrix3x4 ReadMatrix3x4(ref StreamWriter outputFile, String classRef = "")
        {
            long position = this.BaseStream.Position;
            Matrix3x4 value = new Matrix3x4()
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

            outputFile.WriteLine(String.Format("{0} | MATRIX3x4: {1}", position, value));
            if (!classRef.Equals(""))
            {
                outputFile.WriteLine(String.Format("{0} = {1}", classRef, value));
            }

            outputFile.Flush();
            return value;
        }

        public Matrix4x4 ReadMatrix4x4(ref StreamWriter outputFile, String classRef = "")
        {
            long position = this.BaseStream.Position;
            Matrix4x4 value = new Matrix4x4()
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

            outputFile.WriteLine(String.Format("{0} | MATRIX4x4: {1}", position, value));
            if (!classRef.Equals(""))
            {
                outputFile.WriteLine(String.Format("{0} = {1}", classRef, value));
            }

            outputFile.Flush();
            return value;
        }

        public KeyFrame ReadStepLinear32(ref StreamWriter outputFile, String classRef = "")
        {
            long position = this.BaseStream.Position;

            uint FrameVal = this.ReadUInt32(ref outputFile, "LogReader.ReadUnifiedHermite32(...) | FrameVal");

            KeyFrame value = new KeyFrame(
                (FrameVal >> 0) & 0xfff,
                (FrameVal >> 12) & 0xfffff);

            outputFile.WriteLine(String.Format("{0} | STEPLINEAR32: {1}", position, value));
            if (!classRef.Equals(""))
            {
                outputFile.WriteLine(String.Format("{0} = {1}", classRef, value));
            }

            outputFile.Flush();
            return value;
        }

        public KeyFrame ReadStepLinear64(ref StreamWriter outputFile, String classRef = "")
        {
            long position = this.BaseStream.Position;

            KeyFrame value = new KeyFrame(
                this.ReadSingle(),
                this.ReadSingle());

            outputFile.WriteLine(String.Format("{0} | STEPLINEAR64: {1}", position, value));
            if (!classRef.Equals(""))
            {
                outputFile.WriteLine(String.Format("{0} = {1}", classRef, value));
            }

            outputFile.Flush();
            return value;
        }



        public KeyFrame ReadUnifiedHermite32(ref StreamWriter outputFile, String classRef = "")
        {
            long position = this.BaseStream.Position;


            byte Frame = this.ReadByte(ref outputFile, "LogReader.ReadUnifiedHermite32(...) | Frame");
            uint ValSlope = this.ReadUInt24(ref outputFile, "LogReader.ReadUnifiedHermite32(...) | ValSlope");

            int Value = ((int)ValSlope >> 0) & 0xfff;
            int Slope = ((int)ValSlope << 8) >> 20;

            KeyFrame value = new KeyFrame(
                Frame,
                Value,
                Slope * KeyFrameQuantizationHelper.FP_1_6_5);

            
            outputFile.WriteLine(String.Format("{0} | UNIFIEDHERMITE32: {1}", position, value));
            if (!classRef.Equals(""))
            {
                outputFile.WriteLine(String.Format("{0} = {1}", classRef, value));
            }

            outputFile.Flush();
            return value;
        }

        public KeyFrame ReadUnifiedHermite48(ref StreamWriter outputFile, String classRef = "")
        {
            long position = this.BaseStream.Position;


            ushort Frame = this.ReadUInt16(ref outputFile, "LogReader.ReadUnifiedHermite48(...) | Frame");
            ushort Value = this.ReadUInt16(ref outputFile, "LogReader.ReadUnifiedHermite48(...) | Value");
            short Slope = this.ReadInt16(ref outputFile, "LogReader.ReadUnifiedHermite48(...) | Slopes");

            KeyFrame value = new KeyFrame(
                Frame * KeyFrameQuantizationHelper.FP_1_10_5,
                Value,
                Slope * KeyFrameQuantizationHelper.FP_1_7_8);

            
            outputFile.WriteLine(String.Format("{0} | UNIFIEDHERMITE48: {1}", position, value));
            if (!classRef.Equals(""))
            {
                outputFile.WriteLine(String.Format("{0} = {1}", classRef, value));
            }

            outputFile.Flush();
            return value;
        }


        public KeyFrame ReadUnifiedHermite96(ref StreamWriter outputFile, String classRef = "")
        {
            long position = this.BaseStream.Position;
            KeyFrame value = new KeyFrame(
                this.ReadSingle(),
                this.ReadSingle(),
                this.ReadSingle());

            outputFile.WriteLine(String.Format("{0} | UNIFIEDHERMITE96: {1}", position, value));
            if (!classRef.Equals(""))
            {
                outputFile.WriteLine(String.Format("{0} = {1}", classRef, value));
            }

            outputFile.Flush();
            return value;
        }



        public KeyFrame ReadHermite48(ref StreamWriter outputFile, String classRef = "")
        {
            byte Frame = this.ReadByte(ref outputFile, "LogReader.ReadHermite48(...) | Frame");
            ushort Value = this.ReadUInt16(ref outputFile, "LogReader.ReadHermite48(...) | Value");
            uint Slopes = this.ReadUInt24(ref outputFile, "LogReader.ReadHermite48(...) | Slopes");

            int InSlope = ((int)Slopes << 20) >> 20;
            int OutSlope = ((int)Slopes << 8) >> 20;
            KeyFrame value = new KeyFrame(
                Frame,
                Value,
                InSlope * KeyFrameQuantizationHelper.FP_1_6_5,
                OutSlope * KeyFrameQuantizationHelper.FP_1_6_5);


            long position = this.BaseStream.Position;
            outputFile.WriteLine(String.Format("{0} | HERMITE48: {1}", position, value));
            if (!classRef.Equals(""))
            {
                outputFile.WriteLine(String.Format("{0} = {1}", classRef, value));
            }

            outputFile.Flush();
            return value;
        }


        public KeyFrame ReadHermite64(ref StreamWriter outputFile, String classRef = "")
        {
            uint FrameVal = this.ReadUInt32(ref outputFile, "LogReader.ReadHermite64(...) | FrameVal");
            short InSlope = this.ReadInt16(ref outputFile, "LogReader.ReadHermite64(...) | InSlope");
            short OutSlope = this.ReadInt16(ref outputFile, "LogReader.ReadHermite64(...) | OutSlope");

            uint Frame = (FrameVal >> 0) & 0xfff;
            uint Value = (FrameVal >> 12) & 0xfffff;
            KeyFrame value = new KeyFrame(
                Frame,
                Value,
                InSlope * KeyFrameQuantizationHelper.FP_1_7_8,
                OutSlope * KeyFrameQuantizationHelper.FP_1_7_8);


            long position = this.BaseStream.Position;
            outputFile.WriteLine(String.Format("{0} | HERMITE64: {1}", position, value));
            if (!classRef.Equals(""))
            {
                outputFile.WriteLine(String.Format("{0} = {1}", classRef, value));
            }

            outputFile.Flush();
            return value;
        }


        public KeyFrame ReadHermite128(ref StreamWriter outputFile, String classRef = "")
        {
            long position = this.BaseStream.Position;
            KeyFrame value = new KeyFrame(
                this.ReadSingle(),
                this.ReadSingle(),
                this.ReadSingle(),
                this.ReadSingle());

            outputFile.WriteLine(String.Format("{0} | HERMITE128: {1}", position, value));
            if (!classRef.Equals(""))
            {
                outputFile.WriteLine(String.Format("{0} = {1}", classRef, value));
            }

            outputFile.Flush();
            return value;
        }
    }
}
