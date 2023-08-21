using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace TerraUtil.Utilities;
public static partial class Util
{
    /// <summary>
    /// A <see cref="Vector2"/> representing the size of the screen.
    /// </summary>
    public static Vector2 ScreenSize => Main.ScreenSize.ToVector2();

    /// <summary>
    /// A <see cref="Vector2"/> representing the position of the screen.
    /// </summary>
    public static Vector2 ScreenPos => Main.screenPosition;

    /// <summary>
    /// A <see cref="Rectangle"/> representing the screen's position and size.
    /// </summary>
    public static Rectangle Screen => new((int)ScreenPos.X, (int)ScreenPos.Y, (int)ScreenSize.X, (int)ScreenSize.Y);

    /// <summary>
    /// A <see cref="Vector2"/> representing the centre of the screen.
    /// </summary>
    public static Vector2 ScreenCenter => ScreenSize / 2f;

    /// <summary>
    /// A <see cref="Vector2"/> representing the centre of the screen in world coordinates.
    /// </summary>
    public static Vector2 ScreenWorldCenter => Screen.Center.ToVector2();

    /// <summary>
    /// The position of the mouse on the screen.
    /// </summary>
    public static Vector2 MousePos => Main.MouseScreen;

    /// <summary>
    /// The position of the mouse in the world.
    /// </summary>
    public static Vector2 MouseWorld => Main.MouseWorld;

    /// <summary>
    /// If the mouse was just left clicked.
    /// </summary>
    public static bool LeftClick => Main.mouseLeft && Main.mouseLeftRelease;

    /// <summary>
    /// If the mouse was just right clicked.
    /// </summary>
    public static bool RightClick => Main.mouseRight && Main.mouseRightRelease;

    /// <summary>
    /// If the mouse was just middle clicked.
    /// </summary>
    public static bool MiddleClick => Main.mouseMiddle && Main.mouseMiddleRelease;

    /// <summary>
    /// If the mouse was just X1 clicked.
    /// </summary>
    public static bool X1Click => Main.mouseXButton1 && Main.mouseXButton1Release;

    /// <summary>
    /// If the mouse was just X2 clicked.
    /// </summary>
    public static bool X2Click => Main.mouseXButton2 && Main.mouseXButton2Release;

    /// <summary>
    /// Sets the mouse text.
    /// </summary>
    /// <param name="text">The text to display as a tooltip.</param>
    /// <param name="tooltip">Whether a background should be shown for the tooltip.</param>
    public static void MouseText(string text, bool tooltip = false)
    {
        if (tooltip)
            UICommon.TooltipMouseText(text);
        else
            Main.instance.MouseText(text);
    }

    /// <summary>
    /// Resets all mouse text for this frame.
    /// </summary>
    public static void ResetMouseText()
    {
        Main.LocalPlayer.cursorItemIconEnabled = false;
        Main.LocalPlayer.cursorItemIconID = 0;
        Main.LocalPlayer.cursorItemIconText = string.Empty;
        Main.LocalPlayer.cursorItemIconPush = 0;
        Main.signHover = -1;
        Main.mouseText = false;
    }

    // TODO: Remove these when UIButton is added to tML
    /// <summary>
    /// Makes the specified element display the specified mouse text when hovered over.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="element"></param>
    /// <param name="text"></param>
    /// <param name="tooltip"></param>
    /// <returns></returns>
    public static T WithHoverText<T>(this T element, string text, bool tooltip = false) where T : UIElement
    {
        element.OnUpdate += delegate (UIElement affectedElement)
        {
            if (element.IsMouseHovering)
                MouseText(text, tooltip);
        };

        return element;
    }

    /// <summary>
    /// Makes the specified UIPanel change it's colours when hovered over.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="element"></param>
    /// <param name="panelCol"></param>
    /// <param name="panelHoverCol"></param>
    /// <param name="borderCol"></param>
    /// <param name="borderHoverCol"></param>
    /// <param name="useAlternateColours"></param>
    /// <param name="panelAltCol"></param>
    /// <param name="panelHoverAltCol"></param>
    /// <param name="borderAltCol"></param>
    /// <param name="borderHoverAltCol"></param>
    /// <returns></returns>
    public static T WithHoverColours<T>(
        this T element,
        Color panelCol = default,
        Color panelHoverCol = default,
        Color borderCol = default,
        Color borderHoverCol = default,
        Func<bool> useAlternateColours = default,
        Color panelAltCol = default,
        Color panelHoverAltCol = default,
        Color borderAltCol = default,
        Color borderHoverAltCol = default
        ) where T : UIPanel
    {
        // Set the normal and hover colours to the ones that are used by UICommon.WithFadedMouseOver
        if (panelCol == default)
            panelCol = UICommon.DefaultUIBlueMouseOver;
        if (panelHoverCol == default)
            panelHoverCol = UICommon.DefaultUIBlue;
        if (borderCol == default)
            borderCol = UICommon.DefaultUIBorder;
        if (borderHoverCol == default)
            borderHoverCol = UICommon.DefaultUIBorderMouseOver;

        // If useAlternateColours hasn't been specified, then set it to always return false
        if (useAlternateColours == default)
            useAlternateColours = () => false;

        // Set the alternate colours to the normal ones if they aren't specified
        if (panelAltCol == default)
            panelAltCol = panelCol;
        if (panelHoverAltCol == default)
            panelHoverAltCol = panelHoverCol;
        if (borderAltCol == default)
            borderAltCol = borderCol;
        if (borderHoverAltCol == default)
            borderHoverAltCol = borderHoverCol;

        // Updating
        element.OnUpdate += delegate (UIElement affectedElement)
        {
            bool altCols = useAlternateColours();
            if (element.IsMouseHovering)
            {
                element.BackgroundColor = altCols ? panelAltCol : panelCol;
                element.BorderColor = altCols ? borderAltCol : borderCol;
            }
            else
            {
                element.BackgroundColor = altCols ? panelHoverAltCol : panelHoverCol;
                element.BorderColor = altCols ? borderHoverAltCol : borderHoverCol;
            }
        };

        return element;
    }

    /// <summary>
    /// Makes the specified element play sounds when clicked or hovered over.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="element"></param>
    /// <param name="hoverSound"></param>
    /// <param name="clickSound"></param>
    /// <returns></returns>
    public static T WithHoverSounds<T>(this T element, SoundStyle hoverSound, SoundStyle? clickSound = null) where T : UIElement
    {
        // Hover sound
        // TODO: stop children elements from triggering it
        element.OnMouseOver += delegate (UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(hoverSound);
        };

        // Click sound
        if (clickSound != null)
        {
            element.OnLeftClick += delegate (UIMouseEvent evt, UIElement listeningElement)
            {
                SoundEngine.PlaySound(clickSound.Value);
            };
        }

        return element;
    }
}
