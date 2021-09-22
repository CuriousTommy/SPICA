using System.IO;

namespace SPICA.Serialization
{
    interface ICustomSerialization
    {
        void Deserialize(ref StreamWriter outputFile, BinaryDeserializer Deserializer);
        bool Serialize(BinarySerializer Serializer);
    }
}
