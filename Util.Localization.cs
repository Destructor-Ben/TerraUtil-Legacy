namespace TerraUtil;
public static partial class Util
{
    /// <summary>
    /// Returns a localized text
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static LocalizedText GetText(string key)
    {
        return Language.GetText($"Mods.{Mod.Name}.{key}");
    }

    /// <summary>
    /// Returns a localized text string
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string GetTextValue(string key)
    {
        return GetText(key).Value;
    }

    /// <summary>
    /// Returns a localized text string with the formatting
    /// </summary>
    /// <param name="key"></param>
    /// <param name="stringFormat"></param>
    /// <returns></returns>
    public static string GetTextValue(string key, params object[] stringFormat)
    {
        try
        {
            return GetText(key).Format(stringFormat);
        }
        catch (FormatException)
        {
            Mod.Logger.Warn($"Localization key \"{key}\" had invalid pluralization, make sure in the localization files it is \"{{^0\"}}, not \"{{0^\"}} ");
            return key;
        }
    }
}