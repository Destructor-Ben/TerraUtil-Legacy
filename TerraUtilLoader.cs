namespace TerraUtil;
/// <summary>
/// A template <see cref="ModSystem"/> for loading content.
/// </summary>
/// <typeparam name="T">The type of the content being handled by this loader.</typeparam>
public abstract class TerraUtilLoader<T> : ModSystem where T : IModType
{
    /// <summary>
    /// The instance of this loader.
    /// </summary>
    public static TerraUtilLoader<T> Instance => ModContent.GetInstance<TerraUtilLoader<T>>();

    /// <inheritdoc cref="ModType.Mod"/>
    public static new Mod Mod => Util.Mod;

    internal static List<T> Content = new();

    /// <summary>
    /// Registers the given content.
    /// </summary>
    /// <param name="content">The content to register.</param>
    public virtual void AddContent(T content)
    {
        Content ??= new();
        Content.Add(content);
        ModTypeLookup<T>.Register(content);
    }

    public override void Unload()
    {
        Content?.Clear();
        Content = null;
    }
}
