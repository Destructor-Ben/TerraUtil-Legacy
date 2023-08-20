namespace TerraUtil.Edits;
public abstract class Detour : TerraUtilModType
{
    protected sealed override void Register() { }

    public sealed override void SetupContent()
    {
        Apply();
        SetStaticDefaults();
    }

    public abstract void Apply();
}