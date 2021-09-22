using SPICA.Serialization;
using SPICA.Serialization.Attributes;
using System.IO;

namespace SPICA.Formats.CtrGfx.Animation
{
    public class GfxAnimRGBA : ICustomSerialization
    {
        [Ignore] private GfxFloatKeyFrameGroup[] Vector;

        public GfxFloatKeyFrameGroup R => Vector[0];
        public GfxFloatKeyFrameGroup G => Vector[1];
        public GfxFloatKeyFrameGroup B => Vector[2];
        public GfxFloatKeyFrameGroup A => Vector[3];

        public GfxAnimRGBA()
        {
            Vector = new GfxFloatKeyFrameGroup[]
            {
                new GfxFloatKeyFrameGroup(),
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
