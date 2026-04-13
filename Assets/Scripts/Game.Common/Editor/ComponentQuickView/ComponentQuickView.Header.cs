using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

public partial class ComponentQuickView : EditorWindow
{
    private Header _header;

    private class Header : Toolbar
    {
        private readonly ComponentQuickView _cqv;
        private int _headerHeight = 25;

        public Header(ComponentQuickView componentQuickView) : base()
        {
            _cqv = componentQuickView;
            CreateHeader();
        }

        private void CreateHeader()
        {
            var styleSheet = USSUtils.LoadStyleSheet("styles/component-header.uss");
            this.styleSheets.Add(styleSheet);
            this.AddToClassList("header");

            // Lock button (icon)
            var lockButton = new ToolbarButton();
            lockButton.clicked += () =>
            {
                bool wasLocked = _cqv._isLocked;
                _cqv._isLocked = !_cqv._isLocked;

                if (wasLocked && !_cqv._isLocked)
                {
                    _cqv.RefreshSelection();
                }

                UpdateLockUI(lockButton);
            };

            lockButton.style.width = _headerHeight;
            this.Add(lockButton);

            UpdateLockUI(lockButton);
        }

        private void UpdateLockUI(ToolbarButton lockButton)
        {
            var icon = EditorGUIUtility.IconContent(
                _cqv._isLocked ? "LockIcon-On" : "LockIcon"
            );

            lockButton.iconImage = icon.image as Texture2D;
        }
    }
}