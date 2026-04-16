#if UNITY_EDITOR
using UnityEditor;

namespace Game.Common.SerializableWrapper;

public static class SaintsEditorUtils
{
    public static void SetEditorDirty()
    {
        EditorUtility.SetDirty(Selection.activeObject);
    }
}
#endif