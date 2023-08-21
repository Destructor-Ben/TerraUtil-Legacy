namespace TerraUtil.RecipeGroups;
internal class RecipeGroupSystem : TerraUtilLoader<ModRecipeGroup>// TODO: test
{
    public override void AddRecipeGroups()
    {
        foreach (var modGroup in Content)
        {
            var group = new RecipeGroup(() => Util.GetTextValue($"RecipeGroups.{modGroup.Name}"), modGroup.ValidItems.ToArray())
            {
                IconicItemId = modGroup.ItemIconID
            };

            RecipeGroup.RegisterGroup(Mod.Name + ":" + Name, group);
            modGroup.Group = group;
        }
    }
}
