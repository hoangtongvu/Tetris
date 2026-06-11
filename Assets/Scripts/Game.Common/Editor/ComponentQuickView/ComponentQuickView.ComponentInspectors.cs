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

            // ===== Inspector =====
            if (!_cqv._editors.TryGetValue(component, out var componentInspectorData) || componentInspectorData.Editor == null)
            {
                componentInspectorData = new()
                {
                    Editor = Editor.CreateEditor(component),
                };
                _cqv._editors[component] = componentInspectorData;
            }

            var titleBar = new VisualElement();
            titleBar.AddToClassList("title-bar");

            var nativeComponentHeader = new IMGUIContainer(() =>
            {
                EditorGUILayout.InspectorTitlebar(
                    componentInspectorData.IsExpanded,
                    componentInspectorData.Editor);
            });

            nativeComponentHeader.AddToClassList("native-component-header");

            var subComponentHeader = new VisualElement();
            subComponentHeader.AddToClassList("sub-component-header");

            subComponentHeader.Add(CreateHideButton(component));

            var inspector = new InspectorElement(componentInspectorData.Editor);

            inspector.style.marginTop = 4;
            inspector.style.marginBottom = 4;

            nativeComponentHeader.RegisterCallback<MouseDownEvent>(evt =>
            {
                if (evt.button != 0) return; // Left mouse button

                componentInspectorData.IsExpanded = !componentInspectorData.IsExpanded;

                inspector.style.display = componentInspectorData.IsExpanded
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;

                evt.StopPropagation();
            });

            titleBar.Add(nativeComponentHeader);
            titleBar.Add(subComponentHeader);
            container.Add(titleBar);
            container.Add(inspector);

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