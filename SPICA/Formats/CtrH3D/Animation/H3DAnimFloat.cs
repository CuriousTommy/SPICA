using SPICA.Serialization;
using SPICA.Serialization.Attributes;
using System.IO;

namespace SPICA.Formats.CtrH3D.Animation
{
    public class H3DAnimFloat : ICustomSerialization
    {
        [Ignore] private H3DFloatKeyFrameGroup _Value;

        public H3DFloatKeyFrameGroup Value => _Value;

        public H3DAnimFloat()
        {
            _Value = new H3DFloatKeyFrameGroup();
        }

        void ICustomSerialization.Deserialize(ref StreamWriter outputFile, BinaryDeserializer Deserializer)
        {
            H3DAnimVector.SetVector(ref outputFile, Deserializer, ref _Value);
        }

        bool ICustomSerialization.Serialize(BinarySerializer Serializer)
        {
            H3DAnimVector.WriteVector(Serializer, _Value);

            return true;
        }
    }
}
