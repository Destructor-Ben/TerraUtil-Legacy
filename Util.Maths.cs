namespace TerraUtil;
public static partial class Util
{
    /// <summary>
    /// Multiply pixels per tick by this scalar to get miles per hour
    /// </summary>
    public static float PPTToMPH => 216000f / 42240f;

    /// <summary>
    /// A Vector2 representing the size of the screen
    /// </summary>
    public static Vector2 ScreenSize => Main.ScreenSize.ToVector2();

    /// <summary>
    /// A Vector2 representing the position of the screen
    /// </summary>
    public static Vector2 ScreenPos => Main.screenPosition;

    /// <summary>
    /// A Vector2 representing the centre of the screen
    /// </summary>
    public static Vector2 ScreenCenter => Screen.Center.ToVector2();

    /// <summary>
    /// A rectangle representing the screens position and size
    /// </summary>
    public static Rectangle Screen => new Rectangle((int)ScreenPos.X, (int)ScreenPos.Y, (int)ScreenSize.X, (int)ScreenSize.Y);

    /// <summary>
    /// The position of the mouse on the screen
    /// </summary>
    public static Vector2 MousePos => Main.MouseScreen;

    /// <summary>
    /// The position of the mouse in the world
    /// </summary>
    public static Vector2 MouseWorld => Main.MouseWorld;

    /// <summary>
    /// If the mouse was just clicked
    /// </summary>
    public static bool LeftClick => Main.mouseLeft && Main.mouseLeftRelease;

    /// <summary>
    /// Helper method for Vector2 instead of point
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static bool Contains(this Rectangle rect, Vector2 pos)
    {
        return rect.Contains(pos.ToPoint());
    }

    /// <summary>
    /// Rounds the given value
    /// </summary>
    /// <param name="value"></param>
    /// <param name="nearest"></param>
    /// <returns></returns>
    public static float RoundToNearest(float value, float nearest = 1f)
    {
        return MathF.Round(value / nearest) * nearest;
    }
}