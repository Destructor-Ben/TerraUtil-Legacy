namespace TerraUtil;
public abstract class Packet : ModType
{
    protected sealed override void Register()
    {

    }

    public sealed override void SetupContent()
    {
        SetStaticDefaults();
    }

    public void Send(int toWho = -1, int ignoreWho = -1)
    {
        var packet = Mod.GetPacket();
        // TODO: serialize
        packet.Send(toWho, ignoreWho);
    }
}
