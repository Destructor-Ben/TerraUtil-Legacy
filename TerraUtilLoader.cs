namespace TerraUtil;
/// <summary>
/// A template <see cref="ModSystem"/> for loading content.
/// </summary>
/// <typeparam name="T">The type of the content being handled by this loader.</typeparam>
public class TerraUtilLoader<T> : ModSystem where T : TerraUtilModType
{
    internal static List<T> Content = new();

    /// <summary>
    /// Registers the given content.
    /// </summary>
    /// <param name="content">The content to register.</param>
    public static void AddContent(T content)
    {
        Content ??= new();
        Content.Add(content);
    }

    public override void Unload()
    {
        Content = null;
    }
}
