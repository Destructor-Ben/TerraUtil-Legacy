namespace TerraUtil.Utilities;
public static partial class Util
{
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

    /// <summary>
    /// Returns true if the sender ID is the server
    /// </summary>
    /// <param name="whoAmI"></param>
    /// <returns></returns>
    public static bool IsServerID(int whoAmI)
    {
        return whoAmI is 255 or -1;
    }

    /// <summary>
    /// Returns null if the ID is the server, otherwise it is the whoAmI of a client
    /// </summary>
    /// <param name="whoAmI"></param>
    /// <returns></returns>
    public static int? ClientID(int whoAmI)
    {
        return IsServerID(whoAmI) ? null : whoAmI;
    }
}
