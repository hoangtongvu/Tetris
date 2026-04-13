using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public partial class ComponentQuickView : EditorWindow
{
    private ComponentInspectors _componentInspectors;

    private class ComponentInspectors : ScrollView
    {
        private readonly ComponentQuickView _cqv;

        public ComponentInspectors(ComponentQuickView componentQuickView) : base()
        {
            _cqv = componentQuickView;
            this.verticalScrollerVisibility = ScrollerVisibility.AlwaysVisible;
        }

        public void Refresh()
        {
            this.Clear();

            var target = _cqv._target;
            var visibility = _cqv._visibility;

            if (!target) return;

            var components = target.GetComponents<Component>();

            foreach (var c in components)
            {
                if (c == null) continue;
                if (visibility.ContainsKey(c) && !visibility[c]) continue;

                visibility[c] = true;
                var element = CreateComponentElement(c);
                this.Add(element);
            }
        }

        private VisualElement CreateComponentElement(Component component)
        {
            var container = new VisualElement();
            container.style.marginBottom = 0;
            container.style.borderBottomWidth = 1;
            container.style.borderTopWidth = 1;
            container.style.borderLeftWidth = 1;
            container.style.borderRightWidth = 1;
            container.style.borderBottomColor = Color.black;

            // ===== Title Bar =====
            var title = new VisualElement();
            title.style.flexDirection = FlexDirection.Row;
            title.style.alignItems = Align.Center;
            title.style.backgroundColor = new Color(0.25f, 0.25f, 0.25f);
            title.style.height = 20;
            title.style.paddingLeft = 4;
            title.style.paddingRight = 4;

            // Icon
            var icon = new Image();
            icon.image = AssetPreview.GetMiniThumbnail(component);
            icon.style.width = 15;
            icon.style.height = 15;

            // Name
            var label = new Label(component.GetType().Name);
            label.style.unityFontStyleAndWeight = FontStyle.Bold;
            label.style.marginLeft = 4;
            label.style.flexGrow = 1;

            title.Add(icon);
            title.Add(label);
            title.Add(CreateHideButton(component));
            title.Add(CreateShowContextMenuButton(component));

            container.Add(title);

            // ===== Inspector =====
            if (!_cqv._editors.TryGetValue(component, out Editor value) || value == null)
            {
                value = Editor.CreateEditor(component);
                _cqv._editors[component] = value;
            }

            var inspector = new InspectorElement(value);
            inspector.style.marginTop = 4;
            inspector.style.marginBottom = 4;

            container.Add(inspector);

            // ===== Context Menu =====
            title.RegisterCallback<MouseDownEvent>(evt =>
            {
                if (evt.button != 1) return; // Right mouse button

                var mouseRect = new Rect(Event.current.mousePosition, Vector2.zero);
                EditorContextMenuUtil.Show(mouseRect, component);

                evt.StopPropagation();
            });

            return container;
        }

        private Button CreateHideButton(Component component)
        {
            var hideBtn = new Button();
            hideBtn.text = "👁";
            hideBtn.style.height = 20;
            hideBtn.style.width = 20;
            hideBtn.style.marginLeft = 0;
            hideBtn.style.marginRight = 0;
            hideBtn.style.marginTop = 0;
            hideBtn.style.marginBottom = 0;

            hideBtn.clicked += () =>
            {
                _cqv._visibility[component] = false;
                _cqv.RepaintWindow();
            };

            return hideBtn;
        }
    }
}