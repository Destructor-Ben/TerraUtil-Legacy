namespace TerraUtil;
public static partial class Util
{
    /// <summary>
    /// The ID of the server
    /// </summary>
    public const int ServerID = -1;

    /// <summary>
    /// The alternate ID of the server
    /// </summary>
    public const int ServerAltID = 255;

    /// <summary>
    /// The ID of a message when a client requests to the server to send a packet to everyone
    /// </summary>
    public const int SendToAllID = -2;

    /// <summary>
    /// The target in ModPacket.Send when sending from the server to all clients
    /// </summary>
    public const int SendToAll = -1;

    /// <summary>
    /// If the local player is in singleplayer
    /// </summary>
    public static bool IsSingleplayer => Main.netMode == NetmodeID.SinglePlayer;

    /// <summary>
    /// If the local player is a client or server
    /// </summary>
    public static bool IsMultiplayer => IsClient || IsServer;

    /// <summary>
    /// If the local player is a multiplayer client
    /// </summary>
    public static bool IsClient => Main.netMode == NetmodeID.MultiplayerClient;

    /// <summary>
    /// If the local player is a server
    /// </summary>
    public static bool IsServer => Main.netMode == NetmodeID.Server;
}
