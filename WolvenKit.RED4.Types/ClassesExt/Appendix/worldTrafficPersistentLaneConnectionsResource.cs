using System.IO;
using WolvenKit.RED4.IO;

namespace WolvenKit.RED4.Types
{
    public partial class worldTrafficPersistentLaneConnectionsResource : IRedAppendix
    {
        [RED("buffer")]
        [REDProperty(IsIgnored = true)]
        public WByteArray Buffer
        {
            get => GetPropertyValue<WByteArray>();
            set => SetPropertyValue<WByteArray>(value);
        }

        public void Read(Red4Reader reader, uint size)
        {
            Buffer = reader.BaseReader.ReadBytes((int)size);
        }

        public void Write(Red4Writer writer) => writer.BaseWriter.Write(Buffer);
    }
}
