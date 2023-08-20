namespace TerraUtil.Utilities;
public static partial class Util
{
    /// <summary>
    /// Finds the index of the tooltip name
    /// </summary>
    /// <param name="tooltipName"></param>
    /// <param name="tooltips"></param>
    /// <returns></returns>
    public static int FindIndexOfTooltipName(this List<TooltipLine> tooltips, string tooltipName)
    {
        return tooltips.IndexOf(tooltips.Where(t => t.Name == tooltipName).FirstOrDefault());
    }

    /// <summary>
    /// Inserts the tooltips before or after the specified tooltip name
    /// </summary>
    /// <param name="tooltips"></param>
    /// <param name="name"></param>
    /// <param name="after"></param>
    /// <param name="tooltipsToInsert"></param>
    /// <returns></returns>
    public static void InsertTooltips(this List<TooltipLine> tooltips, string name, bool after, params TooltipLine[] tooltipsToInsert)
    {
        int index = tooltips.FindIndexOfTooltipName(name);
        if (index != -1)
        {
            tooltips.InsertRange(after ? index + 1 : index, tooltipsToInsert);
        }
    }

    /// <summary>
    /// Removes the list of tooltips
    /// </summary>
    /// <param name="tooltips"></param>
    /// <param name="tooltipNames"></param>
    public static void RemoveTooltips(this List<TooltipLine> tooltips, params string[] tooltipNames)
    {
        foreach (var tooltip in tooltips)
        {
            if (tooltipNames.Contains(tooltip.Name))
                tooltip.Hide();
        }
    }

    /// <summary>
    /// Returns a localized tooltipline
    /// </summary>
    /// <param name="name"></param>
    /// <param name="stringFormat"></param>
    /// <returns></returns>
    public static TooltipLine GetTooltipLine(string name, params object[] stringFormat)
    {
        return new TooltipLine(Mod, Mod.Name + ":" + name, GetTextValue("Tooltips." + name, stringFormat));
    }
}