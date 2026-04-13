using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public partial class ComponentQuickView : EditorWindow
{
    private ComponentToolbar _componentToolBar;

    private class ComponentToolbar : VisualElement
    {
        private class ToolBarButton : Button
        {
            public ToolBarButton() : base()
            {
                this.style.height = 22;

                this.style.paddingLeft = 0;
                this.style.paddingRight = 0;
                this.style.paddingTop = 0;
                this.style.paddingBottom = 0;

                this.style.marginLeft = 1;
                this.style.marginRight = 1;
                this.style.marginTop = 1;
                this.style.marginBottom = 1;
            }
        }

        private readonly ComponentQuickView _cqv;
        private static readonly Color buttonBackgroundColorOn = new(0.25f, 0.37f, 0.48f);
        private static readonly Color buttonBackgroundColorOff = Color.gray3;
        private bool _toggledAllVisible = true;

        public ComponentToolbar(ComponentQuickView componentQuickView) : base()
        {
            _cqv = componentQuickView;
            CreateComponentToolbar();
        }

        private void CreateComponentToolbar()
        {
            this.style.flexShrink = 0;
            this.style.flexGrow = 0;

            this.style.backgroundColor = Color.gray3;

            this.style.borderTopWidth = 1;
            this.style.borderBottomWidth = 1;
            this.style.borderLeftWidth = 1;
            this.style.borderRightWidth = 1;

            this.style.paddingLeft = 4;
            this.style.paddingRight = 4;
            this.style.paddingTop = 4;
            this.style.paddingBottom = 4;

            this.style.flexDirection = FlexDirection.Row;
            this.style.flexWrap = Wrap.Wrap;
        }

        public void Refresh()
        {
            this.Clear();

            var target = _cqv._target;
            if (!target) return;

            this.Add(CreateToggleAllButton());

            var components = target.GetComponents<Component>();
            var visibility = _cqv._visibility;

            foreach (var c in components)
            {
                if (c == null) continue;

                bool visible;

                if (visibility.TryGetValue(c, out bool value))
                    visible = value;
                else
                    visible = true;

                var button = CreateSingleComponentButton(c, visible);
                button.style.paddingLeft = 3;
                button.style.paddingRight = 3;

                button.style.backgroundColor = visible
                    ? buttonBackgroundColorOn
                    : buttonBackgroundColorOff;

                this.Add(button);
            }

            this.Add(CreateAddComponentButton());
        }

        private ToolBarButton CreateToggleAllButton()
        {
            var button = new ToolBarButton();
            button.style.width = button.style.height;

            var iconSize = new Length(15, LengthUnit.Pixel);
            UpdateToggleAllButtonUI();

            button.clicked += () =>
            {
                _toggledAllVisible = !_toggledAllVisible;
                _cqv.SetAllVisibility(_toggledAllVisible);
                UpdateToggleAllButtonUI();
            };

            void UpdateToggleAllButtonUI()
            {
                // Update icon
                var iconName = _toggledAllVisible ? "d_scenevis_visible_hover" : "d_scenevis_hidden";
                button.style.backgroundImage = UnityEditor.EditorGUIUtility.IconContent(iconName).image as Texture2D;
                button.style.backgroundSize = new BackgroundSize(iconSize, iconSize);

                // Update color
                button.style.backgroundColor = _toggledAllVisible
                    ? buttonBackgroundColorOn
                    : buttonBackgroundColorOff;
            }

            return button;
        }

        private ToolBarButton CreateSingleComponentButton(Component c, bool visible)
        {
            var button = new ToolBarButton();

            var content = new VisualElement();
            content.style.flexDirection = FlexDirection.Row;
            content.style.alignItems = Align.Center;
            content.style.height = 20;
            content.style.maxWidth = 500;

            content.style.paddingLeft = 0;
            content.style.paddingRight = 0;
            content.style.paddingTop = 0;
            content.style.paddingBottom = 0;

            content.style.marginLeft = 0;
            content.style.marginRight = 0;
            content.style.marginTop = 0;
            content.style.marginBottom = 0;

            // Icon
            var icon = new Image();
            icon.image = AssetPreview.GetMiniThumbnail(c);
            icon.style.width = 15;
            icon.style.height = 15;
            icon.style.marginRight = 3;

            // Label
            var label = new Label(c.GetType().Name);

            // Assemble
            content.Add(icon);
            content.Add(label);
            button.Add(content);

            button.clicked += () =>
            {
                _cqv._visibility[c] = !visible;
                _cqv.RepaintWindow();
            };

            return button;
        }

        private ToolBarButton CreateAddComponentButton()
        {
            var button = new ToolBarButton();
            button.style.width = button.style.height;

            button.clicked += () =>
            {
                // Use 230 width so the popup renders at the correct size,
                // regardless of the button's actual visual width
                var screenRect = new Rect(Event.current.mousePosition, new(230, 0));

                var addComponentWindow = System.Type.GetType(
                    "UnityEditor.AddComponent.AddComponentWindow, UnityEditor");

                if (addComponentWindow != null)
                {
                    var method = addComponentWindow.GetMethod(
                        "Show",
                        BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

                    method?.Invoke(null, new object[] { screenRect, new[] { _cqv._target } });
                }
            };

            var icon = EditorGUIUtility.IconContent("d_CreateAddNew").image as Texture2D;
            button.style.backgroundImage = icon;
            button.style.backgroundSize = new BackgroundSize(
                new Length(12, LengthUnit.Pixel),
                new Length(12, LengthUnit.Pixel)
            );

            return button;
        }
    }
}