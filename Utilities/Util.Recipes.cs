namespace TerraUtil.Utilities;
public static partial class Util
{
    /// <summary>
    /// Removes all of the recipes for an item type that aren't created by this mod
    /// </summary>
    /// <param name="type"></param>
    public static void RemoveRecipesForItem(int type)
    {
        foreach (var recipe in Main.recipe)
        {
            if (recipe.Mod != Mod && recipe.createItem.type == type)
                recipe.DisableRecipe();
        }
    }

    /// <summary>
    /// Registers a recipe group with the specified internal name, icon, and items
    /// </summary>
    /// <param name="internalName"></param>
    /// <param name="itemIcon"></param>
    /// <param name="itemIds"></param>
    /// <returns></returns>
    public static RecipeGroup RegisterRecipeGroup(string internalName, int itemIcon, params int[] itemIds)
    {
        var group = new RecipeGroup(() => GetTextValue($"RecipeGroups.{internalName}.Tooltip"), itemIds)
        {
            IconicItemId = itemIcon
        };

        RecipeGroup.RegisterGroup(Mod.Name + ":" + internalName, group);
        return group;
    }
}
