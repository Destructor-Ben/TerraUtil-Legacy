namespace TerraUtil;
public static partial class Util
{
    internal static Mod Mod { get; private set; }

    // TODO: doesn't look like the copying works, dll doesn't appear in the lib folder

    /// <summary>
    /// Call this in Mod.Load to initialize TerraUtil
    /// </summary>
    /// <param name="mod"></param>
    public static void Load(Mod mod)
    {
        Mod = mod;
    }

    /// <summary>
    /// Call this in Mod.Unload to uninitialize TerraUtil
    /// </summary>
    public static void Unload()
    {
        Mod = null;
    }

    #region General Utilities

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
    /// Gets the element if it's in the dictionary, otherwise returns the default value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="dict"></param>
    /// <param name="defaultVal"></param>
    /// <returns></returns>
    public static T FromDictOrDefault<T>(int key, Dictionary<int, T> dict, T defaultVal)
    {
        return dict.ContainsKey(key) ? dict[key] : defaultVal;
    }

    #endregion
}