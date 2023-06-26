using TerraUtil.Networking;

namespace TerraUtil;
public class TerraUtilMod : Mod
{
    // Have to use constructor, because Load isn't called before all other content
    public TerraUtilMod()
    {
        Util.Load(this);
    }

    public override void Unload()
    {
        Util.Unload();
    }

    public override void HandlePacket(BinaryReader reader, int whoAmI)
    {
        PacketSystem.HandlePacket(reader, whoAmI);
    }
}