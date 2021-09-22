using SPICA.Serialization;
using SPICA.Serialization.Attributes;
using System.IO;

namespace SPICA.Formats.CtrGfx.Animation
{
    class GfxAnimVector3D : ICustomSerialization
    {
        [Ignore] private GfxFloatKeyFrameGroup[] Vector;

        public GfxFloatKeyFrameGroup X => Vector[0];
        public GfxFloatKeyFrameGroup Y => Vector[1];
        public GfxFloatKeyFrameGroup Z => Vector[2];

        public GfxAnimVector3D()
        {
            Vector = new GfxFloatKeyFrameGroup[]
            {
                new GfxFloatKeyFrameGroup(),
                new GfxFloatKeyFrameGroup(),
                new GfxFloatKeyFrameGroup()
            };
        }

        void ICustomSerialization.Deserialize(ref StreamWriter outputFile, BinaryDeserializer Deserializer)
        {
            GfxAnimVector.SetVector(ref outputFile, Deserializer, Vector);
        }

        bool ICustomSerialization.Serialize(BinarySerializer Serializer)
        {
            GfxAnimVector.WriteVector(Serializer, Vector);

            return true;
        }
    }
}
