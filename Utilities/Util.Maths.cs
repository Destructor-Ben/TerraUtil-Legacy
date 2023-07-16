namespace TerraUtil.Utilities;
public static partial class Util
{
    /// <summary>
    /// Multiply pixels per tick (velocity) by this scalar to get miles per hour.
    /// </summary>
    public const float PPTToMPH = 216000f / 42240f;

    /// <summary>
    /// Multiply pixels per tick per tick (acceleration) by this scalar to get miles per hour per second.
    /// </summary>
    public const float PPTPTToMPHPS = PPTToMPH * 60f;// TODO: this isn't correct? should be divide, but then it doesn't work

    /// <summary>
    /// Helper method for Vector2 instead of point.
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static bool Contains(this Rectangle rect, Vector2 pos)
    {
        return rect.Contains(pos.ToPoint());
    }

    /// <summary>
    /// Rounds the given value.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="nearest"></param>
    /// <returns></returns>
    public static float Round(float value, float nearest = 1f)
    {
        return value - value % nearest;
    }
}