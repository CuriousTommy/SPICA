using SPICA.Serialization;
using SPICA.Serialization.Attributes;
using System.IO;

namespace SPICA.Formats.CtrGfx.Animation
{
    public class GfxAnimFloat : ICustomSerialization
    {
        [Ignore] private GfxFloatKeyFrameGroup _Value;

        public GfxFloatKeyFrameGroup Value => _Value;

        public GfxAnimFloat()
        {
            _Value = new GfxFloatKeyFrameGroup();
        }

        void ICustomSerialization.Deserialize(ref StreamWriter outputFile, BinaryDeserializer Deserializer)
        {
            GfxAnimVector.SetVector(ref outputFile, Deserializer, _Value);
        }

        bool ICustomSerialization.Serialize(BinarySerializer Serializer)
        {
            GfxAnimVector.WriteVector(Serializer, _Value);

            return true;
        }
    }
}
