using SaintsField;

namespace Game.Common.SerializableWrapper;

public sealed class SetEditorDirtyAttribute : OnValueChangedAttribute
{
    public SetEditorDirtyAttribute() : base(":SaintsEditorUtils.SetEditorDirty")
    {
    }
}