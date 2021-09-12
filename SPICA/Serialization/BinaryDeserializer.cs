using SPICA.Math3D;
using SPICA.Misc;
using SPICA.Serialization.Attributes;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace SPICA.Serialization
{
    class BinaryDeserializer : BinarySerialization
    {
        public LogReader Reader;

        private Dictionary<long, object> Objects;
        private Dictionary<long, object> ListObjs;

        public BinaryDeserializer(Stream BaseStream, SerializationOptions Options) : base(BaseStream, Options)
        {
            Reader = new LogReader(BaseStream);

            Objects = new Dictionary<long, object>();
            ListObjs = new Dictionary<long, object>();
        }

        public T Deserialize<T>(ref StreamWriter OutputFile)
        {
            return (T)ReadValue(ref OutputFile, typeof(T));
        }

        private object ReadValue(ref StreamWriter outputFile, Type Type, bool IsRef = false)
        {
            if (Type.IsPrimitive || Type.IsEnum)
            {
                switch (Type.GetTypeCode(Type))
                {
                    case TypeCode.UInt64: return Reader.ReadUInt64(ref outputFile);
                    case TypeCode.UInt32: return Reader.ReadUInt32(ref outputFile);
                    case TypeCode.UInt16: return Reader.ReadUInt16(ref outputFile);
                    case TypeCode.Byte: return Reader.ReadByte(ref outputFile);
                    case TypeCode.Int64: return Reader.ReadInt64(ref outputFile);
                    case TypeCode.Int32: return Reader.ReadInt32(ref outputFile);
                    case TypeCode.Int16: return Reader.ReadInt16(ref outputFile);
                    case TypeCode.SByte: return Reader.ReadSByte(ref outputFile);
                    case TypeCode.Single: return Reader.ReadSingle(ref outputFile);
                    case TypeCode.Double: return Reader.ReadDouble(ref outputFile);
                    case TypeCode.Boolean: return Reader.ReadBoolUInt32(ref outputFile);

                    default: return null;
                }
            }
            else if (IsList(Type))
            {
                return ReadList(ref outputFile, Type);
            }
            else if (Type == typeof(string))
            {
                return Reader.ReadString(ref outputFile);
            }
            else if (Type == typeof(Vector2))
            {
                return Reader.ReadVector2(ref outputFile);
            }
            else if (Type == typeof(Vector3))
            {
                return Reader.ReadVector3(ref outputFile);
            }
            else if (Type == typeof(Vector4))
            {
                return Reader.ReadVector4(ref outputFile);
            }
            else if (Type == typeof(Quaternion))
            {
                return Reader.ReadQuaternion(ref outputFile);
            }
            else if (Type == typeof(Matrix3x3))
            {
                return Reader.ReadMatrix3x3(ref outputFile);
            }
            else if (Type == typeof(Matrix3x4))
            {
                return Reader.ReadMatrix3x4(ref outputFile);
            }
            else if (Type == typeof(Matrix4x4))
            {
                return Reader.ReadMatrix4x4(ref outputFile);
            }
            else
            {
                return ReadObject(ref outputFile, Type, IsRef);
            }
        }

        private IList ReadList(ref StreamWriter outputFile, Type Type)
        {
            return ReadList(ref outputFile, Type, false, Reader.ReadInt32());
        }

        private IList ReadList(ref StreamWriter outputFile, Type Type, FieldInfo Info)
        {
            return ReadList(
                ref outputFile,
                Type,
                Info.IsDefined(typeof(RangeAttribute)),
                Info.GetCustomAttribute<FixedLengthAttribute>()?.Length ?? Reader.ReadInt32(ref outputFile));
        }

        private IList ReadList(ref StreamWriter outputFile, Type Type, bool Range, int Length)
        {
            IList List;

            if (Type.IsArray)
            {
                Type = Type.GetElementType();
                List = Array.CreateInstance(Type, Length);
            }
            else
            {
                List = (IList)Activator.CreateInstance(Type);
                Type = Type.GetGenericArguments()[0];
            }

            BitReader BR = new BitReader(Reader);

            bool IsBool = Type == typeof(bool);
            bool Inline = Type.IsDefined(typeof(InlineAttribute));
            bool IsValue = Type.IsValueType || Type.IsEnum || Inline;

            for (int Index = 0; (Range ? BaseStream.Position : Index) < Length; Index++)
            {
                long Position = BaseStream.Position;

                object Value;

                if (IsBool)
                {
                    Value = BR.ReadBit(ref outputFile);
                }
                else if (IsValue)
                {
                    Value = ReadValue(ref outputFile, Type);
                }
                else
                {
                    Value = ReadReference(ref outputFile, Type);
                }

                /*
                 * This is not necessary to make deserialization work, but
                 * is needed because H3D uses range lists for the meshes,
                 * and since meshes are actually classes treated as structs,
                 * we need to use the same reference for meshes on the different layer
                 * lists, otherwise it writes the same mesh more than once (and
                 * this should still work, but the file will be bigger for no
                 * good reason, and also is not what the original tool does).
                 */
                if (Type.IsClass && !IsList(Type))
                {
                    if (!ListObjs.TryGetValue(Position, out object Obj))
                    {
                        ListObjs.Add(Position, Value);
                    }
                    else if (Range)
                    {
                        Value = Obj;
                    }
                }

                if (List.IsFixedSize)
                {
                    List[Index] = Value;
                }
                else
                {
                    List.Add(Value);
                }
            }

            return List;
        }

        private string ReadString(ref StreamWriter outputFile)
        {
            StringBuilder SB = new StringBuilder();
            long position = outputFile.BaseStream.Position;

            for (char Chr; (Chr = Reader.ReadChar()) != '\0';)
            {
                SB.Append(Chr);
            }

            outputFile.WriteLine(String.Format("{0} | STRING: {1}", position, SB.ToString()));
            return SB.ToString();
        }

        private object ReadObject(ref StreamWriter outputFile, Type ObjectType, bool IsRef = false)
        {
            long Position = BaseStream.Position;
            outputFile.WriteLine(String.Format("Type: {1} | {0}", Position, ObjectType.FullName));

            if (ObjectType.IsDefined(typeof(TypeChoiceAttribute)))
            {
                uint TypeId = Reader.ReadUInt32(ref outputFile);

                Type Type = GetMatchingType(ObjectType, TypeId);

                if (Type != null)
                {
                    ObjectType = Type;
                }
                else
                {
                    Debug.WriteLine(string.Format(
                        "[SPICA|BinaryDeserializer] Unknown Type Id 0x{0:x8} at address {1:x8} and class {2}!",
                        TypeId,
                        Position,
                        ObjectType.FullName));
                }
            }

            object Value = Activator.CreateInstance(ObjectType);

            if (IsRef) Objects.Add(Position, Value);

            int FieldsCount = 0;

            foreach (FieldInfo Info in GetFieldsSorted(ObjectType))
            {
                FieldsCount++;

                if (!Info.GetCustomAttribute<IfVersionAttribute>()?.Compare(FileVersion) ?? false) continue;

                if (!(
                    Info.IsDefined(typeof(IgnoreAttribute)) ||
                    Info.IsDefined(typeof(CompilerGeneratedAttribute))))
                {
                    Type Type = Info.FieldType;

                    string TCName = Info.GetCustomAttribute<TypeChoiceNameAttribute>()?.FieldName;

                    if (TCName != null && Info.IsDefined(typeof(TypeChoiceAttribute)))
                    {
                        FieldInfo TCInfo = ObjectType.GetField(TCName);

                        uint TypeId = Convert.ToUInt32(TCInfo.GetValue(Value));

                        Type = GetMatchingType(Info, TypeId) ?? Type;
                    }

                    bool Inline;

                    Inline = Info.IsDefined(typeof(InlineAttribute));
                    Inline |= Type.IsDefined(typeof(InlineAttribute));

                    object FieldValue;

                    if (Type.IsValueType || Type.IsEnum || Inline)
                    {
                        FieldValue = IsList(Type)
                            ? ReadList(ref outputFile, Type, Info)
                            : ReadValue(ref outputFile, Type);

                        if (Type.IsPrimitive && Info.IsDefined(typeof(VersionAttribute)))
                        {
                            FileVersion = Convert.ToInt32(FieldValue);
                        }
                    }
                    else
                    {
                        FieldValue = ReadReference(ref outputFile, Type, Info);
                    }

                    if (FieldValue != null) Info.SetValue(Value, FieldValue);

                    Align(Info.GetCustomAttribute<PaddingAttribute>()?.Size ?? 1);
                }
            }

            if (FieldsCount == 0)
            {
                Debug.WriteLine($"[SPICA|BinaryDeserializer] Class {ObjectType.FullName} has no accessible fields!");
            }

            if (Value is ICustomSerialization) ((ICustomSerialization)Value).Deserialize(ref outputFile, this);

            return Value;
        }

        private Type GetMatchingType(MemberInfo Info, uint TypeId)
        {
            foreach (TypeChoiceAttribute Attr in Info.GetCustomAttributes<TypeChoiceAttribute>())
            {
                if (Attr.TypeVal == TypeId)
                {
                    return Attr.Type;
                }
            }

            return null;
        }

        private object ReadReference(ref StreamWriter output_file, Type Type, FieldInfo Info = null)
        {
            uint Address;
            int Length;

            if (GetLengthPos(Info) == LengthPos.AfterPtr)
            {
                Address = ReadPointer();
                Length = ReadLength(Type, Info);
            }
            else
            {
                Length = ReadLength(Type, Info);
                Address = ReadPointer();
            }

            bool Range = Info?.IsDefined(typeof(RangeAttribute)) ?? false;
            bool Repeat = Info?.IsDefined(typeof(RepeatPointerAttribute)) ?? false;

            if (Repeat) BaseStream.Seek(4, SeekOrigin.Current);

            object Value = null;

            if (Address != 0 && (!IsList(Type) || (IsList(Type) && Length > 0)))
            {
                if (!Objects.TryGetValue(Address, out Value))
                {
                    long Position = BaseStream.Position;

                    BaseStream.Seek(Address, SeekOrigin.Begin);

                    Value = IsList(Type)
                        ? ReadList(ref output_file, Type, Range, Length)
                        : ReadValue(ref output_file, Type, true);

                    BaseStream.Seek(Position, SeekOrigin.Begin);
                }
            }

            return Value;
        }

        private int ReadLength(Type Type, FieldInfo Info = null)
        {
            if (IsList(Type))
            {
                if (Info?.IsDefined(typeof(FixedLengthAttribute)) ?? false)
                {
                    return Info.GetCustomAttribute<FixedLengthAttribute>().Length;
                }
                else if (GetLengthSize(Info) == LengthSize.Short)
                {
                    return Reader.ReadUInt16();
                }
                else
                {
                    return Reader.ReadInt32();
                }
            }

            return 0;
        }

        public uint ReadPointer()
        {
            uint Address = Reader.ReadUInt32();

            if (Options.PtrType == PointerType.SelfRelative && Address != 0)
            {
                Address += (uint)BaseStream.Position - 4;
            }

            return Address;
        }
    }
}
