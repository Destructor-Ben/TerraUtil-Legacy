namespace TerraUtil.Utilities;
public static partial class Util
{
    /// <summary>
    /// Requests an asset in the Assets folder
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="loadAsync"></param>
    /// <returns></returns>
    public static Asset<T> GetAsset<T>(string name, bool loadAsync = true) where T : class
    {
        return ModContent.Request<T>($"{Mod.Name}/Assets/{name}", loadAsync ? AssetRequestMode.AsyncLoad : AssetRequestMode.ImmediateLoad);
    }

    /// <summary>
    /// Requests a texture in the Assets/Textures folder
    /// </summary>
    /// <param name="name"></param>
    /// <param name="loadAsync"></param>
    /// <returns></returns>
    public static Asset<Texture2D> GetTexture(string name, bool loadAsync = true)
    {
        return GetAsset<Texture2D>($"Textures/{name.Replace('.', '/')}", loadAsync);
    }
}