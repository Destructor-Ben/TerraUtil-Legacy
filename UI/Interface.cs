using Terraria.UI;
using TerraUtil.Utilities;

namespace TerraUtil.UI;
/// <summary>
/// Provides an abstraction for UserInterface and UIState.<br/>
/// Shamelessly stolen from Starlight River.
/// </summary>
public abstract class Interface : UIState, ILoadable, IModType
{
    #region Setup

    public Mod Mod => Util.Mod;

    public virtual string Name => GetType().Name;

    public string FullName => (Mod?.Name ?? "Terraria") + "/" + Name;

    void ILoadable.Load(Mod mod)
    {
        UISystem.Interfaces ??= new();
        UISystem.Interfaces.Add(this);
        ModTypeLookup<Interface>.Register(this);
        Load();
    }

    void ILoadable.Unload()
    {
        Unload();
    }

    #endregion

    #region Implementation

    /// <summary>
    /// The internal UserInterface that this Interface uses
    /// </summary>
    public UserInterface UserInterface { get; internal set; }

    /// <summary>
    /// Whether the UserInterface should draw and update
    /// </summary>
    public virtual bool Visible { get; set; } = true;

    /// <summary>
    /// The scaling type of the UserInterface.<br/>
    /// By default is <see cref="InterfaceScaleType.UI"/>.
    /// </summary>
    public virtual InterfaceScaleType ScaleType => InterfaceScaleType.UI;

    /// <summary>
    /// Returns the index that the layer should be inserted in the list of game interface layers.<br/>
    /// Return -1 if you don't want to insert it.
    /// </summary>
    /// <param name="layers"></param>
    /// <returns></returns>
    public abstract int GetLayerInsertIndex(List<GameInterfaceLayer> layers);

    /// <summary>
    /// Called when the UserInterface loads.
    /// </summary>
    public virtual void Load() { }

    /// <summary>
    /// Called when the UserInterface unloads.
    /// </summary>
    public virtual void Unload() { }

    public sealed override void OnActivate()
    {
        RemoveAllChildren();
        ResetUI();
        CreateUI();
        SafeOnActivate();
        Recalculate();
    }

    /// <summary>
    /// Called when the UI is opened.
    /// </summary>
    public virtual void SafeOnActivate() { }

    /// <summary>
    /// Called when the UI should be initialized.
    /// </summary>
    protected virtual void CreateUI() { }

    /// <summary>
    /// Called when the UI is destroyed.
    /// </summary>
    protected virtual void ResetUI() { }

    #endregion

    #region Shared Code

    // TODO: add ways to not trigger parent OnMouseHover, but still set IsMouseHovering so hover effect apply to parent but don't double sound effects
    /// <summary>
    /// Prevents a child element from triggering <see cref="UIElement.OnMouseOver"/> and <see cref="UIElement.OnMouseOut"/> on it's parent.
    /// </summary>
    public virtual bool UsesNewMouseSystem { get; set; } = true;

    /// <summary>
    /// Called when the UI is updated.
    /// </summary>
    /// <param name="gameTime"></param>
    public virtual void SafeUpdate(GameTime gameTime) { }
    public sealed override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        SafeUpdate(gameTime);
    }

    /// <summary>
    /// Called when the scroll wheel is used.
    /// </summary>
    /// <param name="evt"></param>
    public virtual void SafeScrollWheel(UIScrollWheelEvent evt) { }
    public sealed override void ScrollWheel(UIScrollWheelEvent evt)
    {
        base.ScrollWheel(evt);
        SafeScrollWheel(evt);
    }

    /// <summary>
    /// Called when the mouse starts hovering hovers over this element.
    /// </summary>
    /// <param name="evt"></param>
    public virtual void SafeMouseOver(UIMouseEvent evt) { }
    public sealed override void MouseOver(UIMouseEvent evt)
    {
        base.MouseOver(evt);
        SafeMouseOver(evt);
    }

    /// <summary>
    /// Called when the mouse stops hovering over this element.
    /// </summary>
    /// <param name="evt"></param>
    public virtual void SafeMouseOut(UIMouseEvent evt) { }
    public sealed override void MouseOut(UIMouseEvent evt)
    {
        base.MouseOut(evt);
        SafeMouseOut(evt);
    }

    // TODO: document these
    #region LMB

    /// <summary>
    /// Called when the left mouse button is released on this element.
    /// </summary>
    /// <param name="evt"></param>
    public virtual void SafeMouseUp(UIMouseEvent evt) { }
    public sealed override void LeftMouseUp(UIMouseEvent evt)
    {
        base.LeftMouseUp(evt);
        SafeMouseUp(evt);
    }

    /// <summary>
    /// Called when the left mouse button is pressed down on this element.
    /// </summary>
    /// <param name="evt"></param>
    public virtual void SafeMouseDown(UIMouseEvent evt) { }
    public sealed override void LeftMouseDown(UIMouseEvent evt)
    {
        base.LeftMouseDown(evt);
        SafeMouseDown(evt);
    }

    public virtual void SafeClick(UIMouseEvent evt) { }
    public sealed override void LeftClick(UIMouseEvent evt)
    {
        base.LeftClick(evt);
        SafeClick(evt);
    }

    public virtual void SafeDoubleClick(UIMouseEvent evt) { }
    public sealed override void LeftDoubleClick(UIMouseEvent evt)
    {
        base.LeftDoubleClick(evt);
        SafeDoubleClick(evt);
    }

    #endregion

    #region RMB

    public virtual void SafeRightMouseUp(UIMouseEvent evt) { }
    public sealed override void RightMouseUp(UIMouseEvent evt)
    {
        base.RightMouseUp(evt);
        SafeRightMouseUp(evt);
    }

    public virtual void SafeRightMouseDown(UIMouseEvent evt) { }
    public sealed override void RightMouseDown(UIMouseEvent evt)
    {
        base.RightMouseDown(evt);
        SafeRightMouseDown(evt);
    }

    public virtual void SafeRightClick(UIMouseEvent evt) { }
    public sealed override void RightClick(UIMouseEvent evt)
    {
        base.RightClick(evt);
        SafeRightClick(evt);
    }

    public virtual void SafeRightDoubleClick(UIMouseEvent evt) { }
    public sealed override void RightDoubleClick(UIMouseEvent evt)
    {
        base.RightDoubleClick(evt);
        SafeRightDoubleClick(evt);
    }

    #endregion

    #region MMB

    public virtual void SafeMiddleMouseUp(UIMouseEvent evt) { }
    public sealed override void MiddleMouseUp(UIMouseEvent evt)
    {
        base.MiddleMouseUp(evt);
        SafeMiddleMouseUp(evt);
    }

    public virtual void SafeMiddleMouseDown(UIMouseEvent evt) { }
    public sealed override void MiddleMouseDown(UIMouseEvent evt)
    {
        base.MiddleMouseDown(evt);
        SafeMiddleMouseDown(evt);
    }

    public virtual void SafeMiddleClick(UIMouseEvent evt) { }
    public sealed override void MiddleClick(UIMouseEvent evt)
    {
        base.MiddleClick(evt);
        SafeMiddleClick(evt);
    }

    public virtual void SafeMiddleDoubleClick(UIMouseEvent evt) { }
    public sealed override void MiddleDoubleClick(UIMouseEvent evt)
    {
        base.MiddleDoubleClick(evt);
        SafeMiddleDoubleClick(evt);
    }

    #endregion

    #region XButton1

    public virtual void SafeXButton1MouseUp(UIMouseEvent evt) { }
    public sealed override void XButton1MouseUp(UIMouseEvent evt)
    {
        base.XButton1MouseUp(evt);
        SafeXButton1MouseUp(evt);
    }

    public virtual void SafeXButton1MouseDown(UIMouseEvent evt) { }
    public sealed override void XButton1MouseDown(UIMouseEvent evt)
    {
        base.XButton1MouseDown(evt);
        SafeXButton1MouseDown(evt);
    }

    public virtual void SafeXButton1Click(UIMouseEvent evt) { }
    public sealed override void XButton1Click(UIMouseEvent evt)
    {
        base.XButton1Click(evt);
        SafeXButton1Click(evt);
    }

    public virtual void SafeXButton1DoubleClick(UIMouseEvent evt) { }
    public sealed override void XButton1DoubleClick(UIMouseEvent evt)
    {
        base.XButton1DoubleClick(evt);
        SafeXButton1DoubleClick(evt);
    }

    #endregion

    #region XButton2

    public virtual void SafeXButton2MouseUp(UIMouseEvent evt) { }
    public sealed override void XButton2MouseUp(UIMouseEvent evt)
    {
        base.XButton2MouseUp(evt);
        SafeXButton2MouseUp(evt);
    }

    public virtual void SafeXButton2MouseDown(UIMouseEvent evt) { }
    public sealed override void XButton2MouseDown(UIMouseEvent evt)
    {
        base.XButton2MouseDown(evt);
        SafeXButton2MouseDown(evt);
    }

    public virtual void SafeXButton2Click(UIMouseEvent evt) { }
    public sealed override void XButton2Click(UIMouseEvent evt)
    {
        base.XButton2Click(evt);
        SafeXButton2Click(evt);
    }

    public virtual void SafeXButton2DoubleClick(UIMouseEvent evt) { }
    public sealed override void XButton2DoubleClick(UIMouseEvent evt)
    {
        base.XButton2DoubleClick(evt);
        SafeXButton2DoubleClick(evt);
    }

    #endregion

    #endregion
}
