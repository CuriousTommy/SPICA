using System.IO;

namespace SPICA.Serialization
{
    interface ICustomSerialization
    {
        void Deserialize(ref StreamWriter OutputFile, BinaryDeserializer Deserializer);
        bool Serialize(BinarySerializer Serializer);
    }
}
