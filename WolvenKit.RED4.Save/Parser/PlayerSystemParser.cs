using WolvenKit.RED4.Types;
using WolvenKit.Core.Extensions;
using WolvenKit.RED4.Save.IO;

namespace WolvenKit.RED4.Save
{
    public class PlayerSystem : INodeData
    {
        public ulong EntityId { get; set; }
        public TweakDBID Character { get; set; }
    }


    public class PlayerSystemParser : INodeParser
    {
        public static string NodeName => Constants.NodeNames.PLAYER_SYSTEM;

        public void Read(BinaryReader reader, NodeEntry node)
        {
            var data = new PlayerSystem();
            data.EntityId = reader.ReadUInt64();
            data.Character = reader.ReadUInt64();

            node.Value = data;
        }

        public void Write(NodeWriter writer, NodeEntry node)
        {
            var data = (PlayerSystem)node.Value;

            writer.Write(data.EntityId);
            writer.Write((ulong)data.Character);
        }
    }

}
