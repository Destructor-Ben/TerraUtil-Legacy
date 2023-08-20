using TerraUtil.UI;

namespace TerraUtil.Utilities;
/// <summary>
/// A static class containing many helper and utility fields and methods
/// </summary>
public static partial class Util
{
    internal static Mod Mod { get; private set; }

    /// <summary>
    /// Call this in a mods constructor to initialize TerraUtil
    /// </summary>
    /// <param name="mod"></param>
    public static void Load(Mod mod)
    {
        Mod = mod;

        TileId = typeof(Tile).GetField("TileId");
    }

    /// <summary>
    /// Call this in Mod.Load to further initialize TerraUtil
    /// </summary>
    public static void LoadContent()
    {
        Mod.AddContent<UISystem>();
    }

    /// <summary>
    /// Call this in Mod.Unload to uninitialize TerraUtil
    /// </summary>
    public static void Unload()
    {
        Mod = null;
    }

    #region Miscellaneous Utilities

    /// <summary>
    /// Applies the specified items equip effects to the player
    /// </summary>
    /// <param name="player"></param>
    /// <param name="itemType"></param>
    /// <param name="hideVisual"></param>
    public static void CopyVanillaEquipEffects(this Player player, int itemType, bool hideVisual)
    {
        var item = new Item();
        item.SetDefaults(itemType);
        player.ApplyEquipFunctional(item, hideVisual);
    }

    /// <summary>
    /// Gets the element if it's in the dictionary, otherwise returns the default value.
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="dict"></param>
    /// <param name="key"></param>
    /// <param name="defaultVal"></param>
    /// <returns></returns>
    public static T2 TryGetOrGiven<T1, T2>(this Dictionary<T1, T2> dict, T1 key, T2 defaultVal)
    {
        return dict.ContainsKey(key) ? dict[key] : defaultVal;
    }

    /// <summary>
    /// Returns if an info display is active and not hidden
    /// </summary>
    /// <param name="display"></param>
    /// <returns></returns>
    public static bool InfoDisplayActive(InfoDisplay display)
    {
        return display.Active() && !Main.LocalPlayer.hideInfo[display.Type];
    }

    #endregion
}