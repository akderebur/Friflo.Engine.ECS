using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Friflo.Engine.ECS;

[ComponentKey("trs")]
[StructLayout(LayoutKind.Sequential)]
public struct TRS : IComponent
{
    private Vector3 _position = Vector3.Zero;
    private Quaternion _rotation = Quaternion.Identity;
    private Vector3 _scale = Vector3.One;

    public TRS()
    {
        SetDirty();
    }

    public Vector3 Position
    {
        get => _position;
        set
        {
            _position = value;
            IsDirty = true;
        }
    }
    
    public Quaternion Rotation
    {
        get => _rotation;
        set
        {
            _rotation = value;
            IsDirty = true;
            IsRecalc = true;
        }
    }
    
    public Vector3 Scale
    {
        get => _scale;
        set
        {
            _scale = value;
            IsDirty = true;
            IsRecalc = true;
        }
    }

    public void SetDirty()
    {
        IsDirty = true;
        IsRecalc = true;
    }

    public void ResetDirty()
    {
        IsDirty = false;
        IsRecalc = false;
    }

    public bool IsDirty { get; set; }
    public bool IsRecalc { get; set; }
    
    [ExcludeFromCodeCoverage] public override   int     GetHashCode()       => throw new NotImplementedException("to avoid boxing");
    [ExcludeFromCodeCoverage] public override   bool    Equals(object obj)  => throw new NotImplementedException("to avoid boxing");
}