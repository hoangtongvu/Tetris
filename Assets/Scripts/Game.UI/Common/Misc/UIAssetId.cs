using Game.Common;
using Game.Common.SerializableWrapper;
using System;
using System.Runtime.InteropServices;

namespace Game.UI.Common;

[Serializable]
[StructLayout(LayoutKind.Explicit)]
public partial struct UIAssetId : IEquatable<UIAssetId>, ISerializableStruct, IAddressableKey
{
    [FieldOffset(0)] private readonly short _raw;
    [FieldOffset(0)] public UIType Type;
    [FieldOffset(1)] public byte VariantIndex;

    public bool Equals(UIAssetId other) => _raw == other._raw;

    public override bool Equals(object obj) => obj is UIAssetId other && _raw == other._raw;

    public override int GetHashCode() => _raw.GetHashCode();

    public static bool operator ==(UIAssetId left, UIAssetId right) => left._raw == right._raw;

    public static bool operator !=(UIAssetId left, UIAssetId right) => left._raw != right._raw;

    public override string ToString() => $"{this.Type}-{this.VariantIndex}";

    public string ToAddressableKey() => $"UI/{this.Type}-{this.VariantIndex}";

    public string ToSerializableKey() => $"{this.Type}-{this.VariantIndex}";

    public void AssignFromSerializableKey(string serializableKey)
    {
        var parts = serializableKey.Split('-');
        this.Type = Enum.Parse<UIType>(parts[0]);
        this.VariantIndex = byte.Parse(parts[1]);
    }
}