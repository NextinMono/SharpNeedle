namespace SharpNeedle.Framework.BIXF;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, Size = 4)]
public struct Union
{
    [FieldOffset(0)] public byte Byte;
    [FieldOffset(0)] public bool Boolean;
    [FieldOffset(0)] public int Integer;
    [FieldOffset(0)] public uint UnsignedInteger;
    [FieldOffset(0)] public float Float;

    public Union(bool value) : this()
    {
        Boolean = value;
    }
    public Union(byte value) : this()
    {
        Byte = value;
    }

    public Union(int value) : this()
    {
        Integer = value;
    }

    public Union(uint value) : this()
    {
        UnsignedInteger = value;
    }

    public Union(float value) : this()
    {
        Float = value;
    }

    public void Set(bool value)
    {
        Boolean = value;
    }
    public void Set(byte value)
    {
        Byte = value;
    }
    public void Set(int value)
    {
        Integer = value;
    }

    public void Set(uint value)
    {
        UnsignedInteger = value;
    }

    public void Set(float value)
    {
        Float = value;
    }

    public static implicit operator Union(bool value)
    {
        return new(value);
    }

    public static implicit operator Union(int value)
    {
        return new(value);
    }

    public static implicit operator Union(uint value)
    {
        return new(value);
    }

    public static implicit operator Union(float value)
    {
        return new(value);
    }

    public static implicit operator bool(Union value)
    {
        return value.Boolean;
    }
    public static implicit operator byte(Union value)
    {
        return value.Byte;
    }

    public static implicit operator int(Union value)
    {
        return value.Integer;
    }

    public static implicit operator uint(Union value)
    {
        return value.UnsignedInteger;
    }

    public static implicit operator float(Union value)
    {
        return value.Float;
    }
}
