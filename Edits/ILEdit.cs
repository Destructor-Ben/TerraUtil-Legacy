using MonoMod.Cil;
using System.Reflection;

namespace TerraUtil.Edits;
/// <summary>
/// An abstraction of an ILEdit, packaged into a ModType.
/// </summary>
public abstract class ILEdit : TerraUtilModType
{
    protected sealed override void Register() { }

    public sealed override void SetupContent()
    {
        Setup();
        SetStaticDefaults();
    }

    public abstract void Setup();
    public abstract void Apply(ILCursor c);

    public void Edit(ILContext il)
    {
        try
        {
            var c = new ILCursor(il);
            Apply(c);
        }
        catch (Exception e)
        {
            throw new ILPatchFailureException(Mod, il, e);
        }
    }
}

/// <summary>
/// An abstraction of an ILEdit that uses reflection, packaged into a ModType.
/// </summary>
public abstract class ILEditReflection : ILEdit
{
    public abstract MethodInfo Method { get; }

    public override void Setup()
    {
        MonoModHooks.Modify(Method, Edit);
    }
}