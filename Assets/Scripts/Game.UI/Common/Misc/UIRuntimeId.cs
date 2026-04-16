using System;

namespace Game.UI.Common;

[Serializable]
public struct UIRuntimeId : IEquatable<UIRuntimeId>
{
    public UIType Type;
    public uint Version;

    public override bool Equals(object obj) => obj is UIRuntimeId other && this.Equals(other);

    public bool Equals(UIRuntimeId other) => this.Type == other.Type && this.Version == other.Version;

    public override string ToString() => $"{this.Type}-{this.Version}";

    public override int GetHashCode()
    {
        ulong kind = (ulong)this.Type;
        ulong combined = kind << 32 | this.Version;
        return combined.GetHashCode();
    }

    public static bool operator ==(UIRuntimeId left, UIRuntimeId right) => left.Equals(right);

    public static bool operator !=(UIRuntimeId left, UIRuntimeId right) => !(left == right);
}