using Terraria.UI;
using TerraUtil.Utilities;

namespace TerraUtil.UI;
internal class UISystem : ModSystem
{
    public static List<Interface> Interfaces;

    public override void SetStaticDefaults()
    {
        if (Util.IsHeadless)
            return;

        foreach (var ui in Interfaces)
        {
            ui.UserInterface = new UserInterface();
            ui.UserInterface.SetState(ui);
            ui.Activate();
        }
    }

    public override void Unload()
    {
        Interfaces = null;
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        foreach (var ui in Interfaces)
        {
            if (!ui.Visible)
                return;

            int index = ui.GetLayerInsertIndex(layers);
            if (index == -1)
                return;

            layers.Insert(index, new LegacyGameInterfaceLayer(
                Util.Mod.Name + ": " + ui.Name,
                delegate
                {
                    ui.UserInterface?.Draw(Main.spriteBatch, null);
                    return true;
                },
                ui.ScaleType
            ));
        }
    }

    public override void UpdateUI(GameTime gameTime)
    {
        foreach (var ui in Interfaces)
        {
            if (!ui.Visible)
                return;

            ui.UserInterface?.Update(gameTime);
        }
    }
}
