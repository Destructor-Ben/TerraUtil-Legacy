using TerraUtil.Utilities;

namespace TerraUtil.Networking;
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

    #region Public Stuff

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

    // TODO: automatic serialization
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

    #endregion

    #region Sending Methods

    private ModPacket GetPacket(NetID sendType)
    {
        var packet = Mod.GetPacket();
        packet.Write(Type);
        Serialize(packet);
        packet.Write((byte)sendType);
        return packet;
    }

    private void Send(ModPacket packet, int toWho = -1, int ignoreWho = -1)
    {
        if (Util.IsSingleplayer)
            return;

        packet.Send(toWho, ignoreWho);
    }

    /// <summary>
    /// Sends this packet to the server
    /// </summary>
    /// <param name="handleIfTarget"></param>
    public void SendToServer()
    {
        if (Util.IsServer && HandleIfTarget)
        {
            // Server handles the packet itself
            HandleFromTarget();
        }
        else if (Util.IsClient)
        {
            // Send to server
            var packet = GetPacket(NetID.SendToServer);
            Send(packet);
        }
    }

    /// <summary>
    /// Sends this packet to a specific client
    /// </summary>
    /// <param name="toWho"></param>
    /// <param name="handleIfTarget"></param>
    public void SendToClient(int toWho)
    {
        if (Util.IsServer)
        {
            // Server sends the packet to the client
            var packet = GetPacket(NetID.SendToClient);
            Send(packet, toWho);
        }
        else if (Util.IsClient && toWho == Main.myPlayer && HandleIfTarget)
        {
            // This is the target machine
            HandleFromTarget();
        }
        else if (Util.IsClient)
        {
            // Send packet to server so it can forward it
            var packet = GetPacket(NetID.SendToClient);
            packet.Write(toWho);
            Send(packet);
        }
    }

    /// <summary>
    /// Sends this packet to the specified clients
    /// </summary>
    /// <param name="handleIfTarget"></param>
    /// <param name="toWho"></param>
    public void SendToClients(params int[] toWho)
    {
        if (Util.IsServer)
        {
            // Server sends packets to the clients
            foreach (int client in toWho)
            {
                var packet = GetPacket(NetID.SendToClients);
                Send(packet, client);
            }
        }
        else if (Util.IsClient && toWho.Contains(Main.myPlayer) && HandleIfTarget)
        {
            // If this is the target machine then handle it (not sending it is handled by the server)
            HandleFromTarget();
        }
        else if (Util.IsClient)
        {
            // Send packet to server so it can forward it
            var packet = GetPacket(NetID.SendToClients);
            packet.Write(toWho.Length);
            foreach (int client in toWho)
            {
                packet.Write(client);
            }
            Send(packet);
        }
    }

    /// <summary>
    /// Sends this packet to all clients
    /// </summary>
    /// <param name="handleIfTarget"></param>
    public void SendToAllClients()
    {
        if (Util.IsServer)
        {
            // Server sends packets to all the clients
            var packet = GetPacket(NetID.SendToAllClients);
            packet.Send();
        }
        else if (Util.IsClient)
        {
            // Handling if we want to
            if (HandleIfTarget)
            {
                HandleFromTarget();
                return;
            }

            // Send packet to server so it can forward it to everyone
            var packet = GetPacket(NetID.SendToAllClients);
            Send(packet);
        }
    }

    /// <summary>
    /// Sends this packet to all of the clients and the server
    /// </summary>
    /// <param name="handleIfTarget"></param>
    public void SendToAll()
    {
        if (Util.IsServer)
        {
            // Handling if we want to
            if (HandleIfTarget)
                HandleFromTarget();

            // Server sends packets to all the clients
            var packet = GetPacket(NetID.SendToAll);
            packet.Send();
        }
        else if (Util.IsClient)
        {
            // Handling if we want to
            if (HandleIfTarget)
                HandleFromTarget();

            // Send packet to server so it can forward it to everyone
            var packet = GetPacket(NetID.SendToAll);
            Send(packet);
        }
    }

    #endregion

    #region Handling

    /// <summary>
    /// Call this in Mod.HandlePacket to enable custom packet handling
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="sender"></param>
    public static void HandlePacket(BinaryReader reader, int sender)
    {
        int? fromWho = Util.ClientID(sender);
        int id = reader.ReadInt32();

        var packet = packets[id];
        packet.Deserialize(reader);

        // Handling for different send types
        var sendType = (NetID)reader.ReadByte();
        switch (sendType)
        {
            case NetID.SendToServer:
                packet.HandleSendToServer(fromWho);
                break;
            case NetID.SendToClient:
                packet.HandleSendToClient(reader, fromWho);
                break;
            case NetID.SendToClients:
                packet.HandleSendToClients(reader, fromWho);
                break;
            case NetID.SendToAllClients:
                packet.HandleSendToAllClients(fromWho);
                break;
            case NetID.SendToAll:
                packet.HandleSendToAll(fromWho);
                break;
            default:
                Mod.Logger.Warn("Unknown packet type: " + sendType.ToString());
                break;
        }
    }

    private void HandleSendToServer(int? fromWho)
    {
        if (Util.IsServer)
            Handle(fromWho);
    }

    private void HandleSendToClient(BinaryReader reader, int? fromWho)
    {
        if (Util.IsServer)
        {
            // Forward the packet
            int toWho = reader.ReadInt32();
            var packet = GetPacket(NetID.SendToClient);
            Send(packet, toWho);
        }
        else if (Util.IsClient)
        {
            // Handle the packet
            Handle(fromWho);
        }
    }

    private void HandleSendToClients(BinaryReader reader, int? fromWho)
    {
        if (Util.IsServer)
        {
            // Forward the packets
            int numClients = reader.ReadInt32();
            for (int i = 0; i < numClients; i++)
            {
                int toWho = reader.ReadInt32();
                if (Util.ClientID(toWho) == fromWho)
                    continue;

                // Sending new packet
                var packet = GetPacket(NetID.SendToClients);
                packet.Send(toWho);
            }
        }
        else if (Util.IsClient)
        {
            // Handle the packet
            Handle(fromWho);
        }
    }

    private void HandleSendToAllClients(int? fromWho)
    {
        if (Util.IsServer)
        {
            // Send a packet to everyone
            var packet = GetPacket(NetID.SendToAllClients);
            packet.Send(ignoreClient: fromWho ?? -1);
        }
        else if (Util.IsClient)
        {
            // Handle the packet
            Handle(fromWho);
        }
    }

    private void HandleSendToAll(int? fromWho)
    {
        if (Util.IsServer)
        {
            // Handle the packet
            Handle(fromWho);

            // Send a packet to everyone
            var packet = GetPacket(NetID.SendToAllClients);
            packet.Send(ignoreClient: fromWho ?? -1);
        }
        else if (Util.IsClient)
        {
            // Handle the packet
            Handle(fromWho);
        }
    }

    private void HandleFromTarget()
    {
        Handle(Util.ClientID(Main.myPlayer));
    }

    #endregion
}
