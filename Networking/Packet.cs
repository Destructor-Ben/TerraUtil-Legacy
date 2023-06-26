using TerraUtil.Utilities;

namespace TerraUtil.Networking;
// TODO: rework sending - Make network system class, and keep everything static there, and then make everything in the class instanced because otherwise it gets messy
// TODO: automatic serialization
public abstract class Packet : ModType
{
    public int Type { get; private set; }

    public static new Mod Mod => Util.Mod;

    private static Dictionary<int, Packet> packets;
    private static int idCounter;

    public sealed override void Load()
    {
        idCounter = 0;
        packets = new();
    }

    public sealed override void Unload()
    {
        packets = null;
    }

    protected sealed override void Register()
    {
        Type = idCounter;
        packets.Add(Type, this);
        idCounter++;
        ModTypeLookup<Packet>.Register(this);
    }

    public sealed override void SetupContent()
    {
        SetStaticDefaults();
    }

    public sealed override void SetStaticDefaults()
    {
    }

    /// <summary>
    /// If true and the current machine is the target of a packet, the packet will also be handled on that machine. Works in singleplayer.
    /// </summary>
    public virtual bool HandleIfTarget => true;

    /// <summary>
    /// Makes the packet do something when it has arrived
    /// </summary>
    /// <param name="fromWho"></param>
    public abstract void Handle(int? fromWho);

    /// <summary>
    /// Called when the packet is about to be serialized
    /// </summary>
    public virtual void OnSend(int? toWho) { }

    /// <summary>
    /// Serializes the packet data
    /// </summary>
    /// <param name="writer"></param>
    public abstract void Serialize(BinaryWriter writer);

    /// <summary>
    /// Deserializes the packet data
    /// </summary>
    /// <param name="reader"></param>
    public abstract void Deserialize(BinaryReader reader);

    private void Recieve(BinaryReader reader, int? fromWho)
    {
        Deserialize(reader);
        Handle(fromWho);
    }

    private ModPacket GetPacket(NetID id)
    {
        var packet = Mod.GetPacket();
        packet.Write(Type);
        return packet;
    }

    private void SendPacket(ModPacket packet, int toWho = -1, int ignoreWho = -1)
    {
        if (Util.IsSingleplayer)
            return;

        Serialize(packet);
        OnSend(Util.ClientID(toWho));
        packet.Send(toWho, ignoreWho);
    }

    private void HandleBecauseMachineIsTarget()
    {
        Handle(Util.ClientID(Main.myPlayer));
    }

    private void Handle_Inner(NetID id, BinaryReader reader, int? fromWho)
    {
        switch (id)
        {
            case NetID.SendToServer:
                if (Util.IsServer)
                    break;
                else if (Util.IsClient)
                    break;
                break;
            case NetID.SendToClient:
                if (Util.IsServer)
                    break;
                else if (Util.IsClient)
                    break;
                break;
            case NetID.SendToClients:
                if (Util.IsServer)
                    break;
                else if (Util.IsClient)
                    break;
                break;
            case NetID.SendToAllClients:
                if (Util.IsServer)
                    break;
                else if (Util.IsClient)
                    break;
                break;
            case NetID.SendToAll:
                if (Util.IsServer)
                    break;
                else if (Util.IsClient)
                    break;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Call this in Mod.HandlePacket to enable custom packet handling
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="sender"></param>
    public static void Handle(BinaryReader reader, int fromWho)
    {
        var id = (NetID)reader.ReadByte();
        var packet = packets[(int)id];
        packet.Handle_Inner(id, reader, Util.ClientID(fromWho));
    }

    /// <summary>
    /// Sends this packet to the server
    /// </summary>
    /// <param name="handleIfTarget"></param>
    public void SendToServer(bool handleIfTarget = true)
    {
        if (Util.IsServer && handleIfTarget)
        {
            HandleBecauseMachineIsTarget();
        }
        else if (Util.IsClient)
        {
            var packet = GetPacket();
            SendPacket(packet, -1, -1);
        }
    }

    /// <summary>
    /// Sends this packet to a specific client
    /// </summary>
    /// <param name="toWho"></param>
    /// <param name="handleIfTarget"></param>
    public void SendToClient(int toWho, bool handleIfTarget = true)
    {
        if (Util.IsClient)
        {
            // If we are the target then handle the packet
            if (toWho == Main.myPlayer && handleIfTarget)
                Handle(fromWho: Main.myPlayer);
            else // Otherwise, send it and tell the server who we want to send it to
                Send(toWho: Util.ServerID, extraData: toWho);
        }
        else if (Util.IsServer)
        {
            // Sending as the server just requires us to send it to the client
            Send(toWho: toWho, extraData: Util.ServerID);
        }
    }

    /// <summary>
    /// Sends this packet to the specified clients
    /// </summary>
    /// <param name="handleIfTarget"></param>
    /// <param name="toWho"></param>
    public void SendToClients(bool handleIfTarget = true, params int[] toWho)
    {
        // Can just loop over all of them, and then the rest of the code will handle it
        foreach (int client in toWho)
        {
            SendToClient(toWho: client, handleIfTarget);
        }
    }

    /// <summary>
    /// Sends this packet to all clients
    /// </summary>
    /// <param name="handleIfTarget"></param>
    public void SendToAllClients(bool handleIfTarget = true)
    {
        if (Util.IsClient)
        {
            Send(toWho: Util.ServerID, extraData: Util.SendToAllID);
        }
        else if (Util.IsServer)
        {
            Send(toWho: Util.SendToAll, extraData: Util.ServerID);
        }
    }

    /// <summary>
    /// Sends this packet to all of the clients and the server
    /// </summary>
    /// <param name="handleIfTarget"></param>
    public void SendToAll(bool handleIfTarget = true)
    {
        SendToServer(handleIfTarget);
        SendToAllClients(handleIfTarget);
    }
}
