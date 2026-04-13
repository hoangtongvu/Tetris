using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public partial class ComponentQuickView : EditorWindow
{
    private UnityHeader _unityHeader;

    private class UnityHeader : IMGUIContainer
    {
        private readonly ComponentQuickView _cqv;
        private Editor _editor;

        public UnityHeader(ComponentQuickView componentQuickView) : base()
        {
            _cqv = componentQuickView;
            CreateHeader();
        }

        private void CreateHeader()
        {
            this.style.flexShrink = 0;
            this.style.flexGrow = 0;

            this.onGUIHandler += () =>
            {
                var target = _cqv._target;
                if (!target) return;

                if (_editor == null || _editor.target != target)
                {
                    if (_editor != null)
                        Object.DestroyImmediate(_editor);

                    _editor = Editor.CreateEditor(target);
                }

                EditorGUI.BeginChangeCheck();

                _editor.DrawHeader();

                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(target);
                    this.MarkDirtyRepaint();
                }
            };
        }
    }
}