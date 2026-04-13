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

            var styleSheet = USSUtils.LoadStyleSheet("styles/component-inspectors.uss");
            this.styleSheets.Add(styleSheet);
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
            container.AddToClassList("component-element");

            // ===== Title Bar =====
            var title = new VisualElement();
            title.AddToClassList("title-bar");

            // Icon
            var icon = new Image();
            icon.image = AssetPreview.GetMiniThumbnail(component);
            icon.AddToClassList("component-icon");

            // Name
            var label = new Label(component.GetType().Name);
            label.AddToClassList("component-name-label");

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
            hideBtn.AddToClassList("util-button");

            hideBtn.clicked += () =>
            {
                _cqv._visibility[component] = false;
                _cqv.RepaintWindow();
            };

            return hideBtn;
        }
    }
}