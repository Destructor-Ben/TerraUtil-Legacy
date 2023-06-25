namespace TerraUtil;
// TODO: test send methods, auto serialization, test server id as -1 or 255, figure out how instancing for ModTypes works - Need to set type in contructor, and also need to change Mod to be not static
public abstract class Packet : ModType
{
    #region Setup

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

    #endregion

    /// <summary>
    /// Sends this packet
    /// </summary>
    /// <param name="toWho"></param>
    /// <param name="ignoreWho"></param>
    /// <param name="extraData"></param>
    private void Send(int toWho, int extraData, int ignoreWho = -1)
    {
        if (Util.IsSingleplayer)
            return;

        // Create
        var packet = Mod.GetPacket();
        packet.Write(Type);

        // Write extra data
        packet.Write(extraData);

        // Serialize and send
        OnSend((toWho is Util.ServerID or Util.ServerAltID) ? null : toWho);
        Serialize(packet);
        packet.Send(toWho, ignoreWho);
    }

    /// <summary>
    /// Sends this packet to the server
    /// </summary>
    /// <param name="handleIfTarget"></param>
    public void SendToServer(bool handleIfTarget = true)
    {
        if (Util.IsServer && handleIfTarget)
        {
            Handle(fromWho: null);
        }
        else if (Util.IsClient)
        {
            Send(toWho: Util.ServerID, extraData: Util.ServerID);
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

    /// <summary>
    /// Call this in Mod.HandlePacket to enable custom packet handling
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="sender"></param>
    public static void Handle(BinaryReader reader, int sender)
    {
        int id = reader.ReadInt32();
        var packet = packets[id];

        if (Util.IsClient)
        {
            // If the sender isn't a player ID, then it was sent by the server
            sender = reader.ReadInt32();
            int? fromWho = (sender is Util.ServerID or Util.ServerAltID) ? null : sender;

            // So we handle the packet
            packet.Recieve(reader, fromWho);
        }
        else if (Util.IsServer)
        {
            // Get who the packet is meant to be sent to
            int toWho = reader.ReadInt32();
            if (toWho is Util.SendToAllID)
            {
                // Forward packet to all clients
                packet.Send(toWho: Util.SendToAll, extraData: sender, ignoreWho: sender);// TODO - test client 2 recieveing from client 1 in sendall - test that it is the correct extraData value
            }
            else if (toWho is Util.ServerID or Util.ServerAltID)
            {
                // Handle the packet as the server
                packet.Recieve(reader, sender);
            }
            else
            {
                // If it wasn't meant for the server, forward it
                packet.Deserialize(reader);
                packet.Send(toWho: toWho, extraData: sender);
            }
        }
    }

    /// <summary>
    /// Called when the packet is recieved<br/>Deserializes then handles the packet
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="fromWho"></param>
    private void Recieve(BinaryReader reader, int? fromWho)
    {
        Deserialize(reader);
        Handle(fromWho);
    }

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
}
