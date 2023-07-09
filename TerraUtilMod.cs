using TerraUtil.Networking;
using TerraUtil.Utilities;

namespace TerraUtil;
/// <summary>
/// Functions as a mod class but integrates TerraUtil loading into it.
/// </summary>
public class TerraUtilMod : Mod
{
    // Have to use constructor, because Load isn't called before all other content
    public TerraUtilMod()
    {
        Util.Load(this);
    }

    public override void Load()
    {
        Util.LoadContent();
    }

    public override void Unload()
    {
        Util.Unload();
    }

    public override void HandlePacket(BinaryReader reader, int whoAmI)
    {
        Packet.HandlePacket(reader, whoAmI);
    }
}