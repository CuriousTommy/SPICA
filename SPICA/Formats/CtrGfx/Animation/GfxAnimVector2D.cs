using SPICA.Serialization;
using SPICA.Serialization.Attributes;
using System.IO;

namespace SPICA.Formats.CtrGfx.Animation
{
    public class GfxAnimVector2D : ICustomSerialization
    {
        [Ignore] private GfxFloatKeyFrameGroup[] Vector;

        public GfxFloatKeyFrameGroup X => Vector[0];
        public GfxFloatKeyFrameGroup Y => Vector[1];

        public GfxAnimVector2D()
        {
            Vector = new GfxFloatKeyFrameGroup[]
            {
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
