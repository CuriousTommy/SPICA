using SPICA.Serialization;
using SPICA.Serialization.Attributes;
using System.IO;

namespace SPICA.Formats.CtrH3D.Animation
{
    public class H3DAnimVector2D : ICustomSerialization
    {
        [Ignore] private H3DFloatKeyFrameGroup[] Vector;

        public H3DFloatKeyFrameGroup X => Vector[0];
        public H3DFloatKeyFrameGroup Y => Vector[1];

        public H3DAnimVector2D()
        {
            Vector = new H3DFloatKeyFrameGroup[]
            {
                new H3DFloatKeyFrameGroup(),
                new H3DFloatKeyFrameGroup()
            };
        }

        void ICustomSerialization.Deserialize(ref StreamWriter OutputFile, BinaryDeserializer Deserializer)
        {
            H3DAnimVector.SetVector(ref OutputFile, Deserializer, Vector);
        }

        bool ICustomSerialization.Serialize(BinarySerializer Serializer)
        {
            H3DAnimVector.WriteVector(Serializer, Vector);

            return true;
        }
    }
}
