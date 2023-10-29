using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace TerraUtil.UI.Elements;
// TODO: documentation
// TODO: allow safe overrides for CreateUI and SafeUpdate
public abstract class UIWindow : Interface
{
    public override int GetLayerInsertIndex(List<GameInterfaceLayer> layers)
    {
        return layers.FindIndex(l => l.Name == "Vanilla: Mouse Text");
    }

    protected abstract LocalizedText WindowTitle { get; }
    protected virtual Vector2 Size { get; set; } = new Vector2(900, 600);
    protected virtual Vector2 InitialPosition { get; set; } = new Vector2(0.5f, 0.5f);

    public UIPanel Panel { get; private set; }
    public UIElement Content { get; private set; }
    private UIElement TitleBar;

    public event Action OnClose;

    private Vector2 dragOffset;
    private bool dragging;

    protected override void CreateUI()
    {
        // Background
        Panel = new UIPanel
        {
            Width = { Pixels = Size.X },
            Height = { Pixels = Size.Y },
            HAlign = InitialPosition.X,
            VAlign = InitialPosition.Y,
            BackgroundColor = UICommon.MainPanelBackground,
        };
        Append(Panel);

        // Title bar
        TitleBar = new UIElement
        {
            Width = { Percent = 1f },
            Height = { Pixels = 30 },
        };
        Panel.Append(TitleBar);

        // Content
        Content = new UIElement
        {
            Width = { Percent = 1f },
            Height = { Pixels = -20, Percent = 1f },
            Top = { Pixels = 30 },
            PaddingTop = 12,
        };
        Panel.Append(Content);

        // Close button
        // TODO: replace texture requesting when new asset system comes around
        var closeButton = new UIImageButton(ModContent.Request<Texture2D>("Terraria/Images/UI/SearchCancel", AssetRequestMode.ImmediateLoad))
        {
            HAlign = 1f,
        };
        closeButton.OnLeftClick += CloseWindow;
        TitleBar.Append(closeButton);

        // Title
        var title = new UIText(WindowTitle, 0.5f, large: true);
        TitleBar.Append(title);

        // Divider
        var divider = new UIDivider
        {
            VAlign = 1f,
        };
        TitleBar.Append(divider);
    }

    public override void SafeUpdate(GameTime gameTime)
    {
        // Stop weapons from being able to be used while the window is being hovered over
        if (Panel.IsMouseHovering)
            Main.LocalPlayer.mouseInterface = true;

        // Dragging
        var dimensions = GetDimensions();

        if (!Main.mouseLeft)
            dragging = false;

        if ((Main.mouseLeft && TitleBar.ContainsPoint(Main.MouseScreen)) || dragging)
        {
            dragging = true;

            if (dragOffset == Vector2.Zero)
                dragOffset = Main.MouseScreen - dimensions.Position();

            var newPos = Main.MouseScreen - dragOffset;
            Left.Set(newPos.X, 0f);
            Top.Set(newPos.Y, 0f);
            HAlign = 0f;
            VAlign = 0f;
        }
        else
        {
            dragOffset = Vector2.Zero;
        }
    }

    private void CloseWindow(UIMouseEvent evt, UIElement listeningElement)
    {
        SoundEngine.PlaySound(SoundID.MenuClose);
        Visible = false;
        OnClose?.Invoke();
    }
}
