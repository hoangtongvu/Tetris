using UnityEditor;
using UnityEngine;

public partial class ComponentQuickView : EditorWindow
{
    private LockInspectorButton _lockInspectorButton;

    public class LockInspectorButton : EditorWindowTitleButton
    {
        public LockInspectorButton(ComponentQuickView componentQuickView) : base(componentQuickView)
        {
        }

        protected override void DrawButton(Rect rect)
        {
            var icon = EditorGUIUtility.IconContent(
                _cqv._isLocked ? "LockIcon-On" : "LockIcon"
            );

            if (GUI.Button(rect, GUIContent.none, EditorStyles.iconButton))
            {
                bool wasLocked = _cqv._isLocked;
                _cqv._isLocked = !_cqv._isLocked;

                if (wasLocked && !_cqv._isLocked)
                    _cqv.RefreshSelection();
            }

            const float iconSize = 11f;

            var iconRect = new Rect(
                rect.x + (rect.width - iconSize) * 0.5f,
                rect.y + (rect.height - iconSize) * 0.5f,
                iconSize,
                iconSize
            );

            GUI.DrawTexture(iconRect, (Texture2D)icon.image, ScaleMode.ScaleToFit);
        }
    }
}