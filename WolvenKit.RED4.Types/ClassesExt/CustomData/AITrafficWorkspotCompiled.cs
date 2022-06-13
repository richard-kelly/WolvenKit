using WolvenKit.RED4.IO;

namespace WolvenKit.RED4.Types
{
    public partial class AITrafficWorkspotCompiled : IRedCustomData
    {
        [RED("buffer")]
        [REDProperty(IsIgnored = true)]
        public WByteArray Buffer
        {
            get => GetPropertyValue<WByteArray>();
            set => SetPropertyValue<WByteArray>(value);
        }

        public void CustomRead(Red4Reader reader, uint size) => Buffer = reader.BaseReader.ReadBytes((int)size);

        public void CustomWrite(Red4Writer writer) => writer.BaseWriter.Write(Buffer);
    }
}
