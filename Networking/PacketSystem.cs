using TerraUtil.Utilities;

namespace TerraUtil.Networking;
public static class PacketSystem
{
    public static void HandlePacket(BinaryReader reader, int sender)
    {
        int? fromWho = Util.ClientID(sender);

    }
}
