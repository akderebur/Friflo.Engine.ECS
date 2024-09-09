using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Friflo.Engine.ECS;

[ComponentKey("trs")]
[StructLayout(LayoutKind.Sequential)]
public struct TRS : IComponent
{
    private Vector3 _position;
    private Quaternion _rotation;
    private Vector3 _scale;

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
            IsRotationDirty = true;
        }
    }
    
    public Vector3 Scale
    {
        get => _scale;
        set
        {
            _scale = value;
            IsDirty = true;
        }
    }

    public bool IsDirty { get; set; }
    public bool IsRotationDirty { get; set; }
    
    [ExcludeFromCodeCoverage] public override   int     GetHashCode()       => throw new NotImplementedException("to avoid boxing");
    [ExcludeFromCodeCoverage] public override   bool    Equals(object obj)  => throw new NotImplementedException("to avoid boxing");
}