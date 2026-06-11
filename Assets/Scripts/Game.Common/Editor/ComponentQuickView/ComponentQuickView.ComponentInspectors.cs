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
            var inspectorStates = _cqv._componentInspectorStates;

            if (!target) return;

            var components = target.GetComponents<Component>();

            foreach (var c in components)
            {
                if (inspectorStates.ContainsKey(c) && !inspectorStates[c].IsVisible) continue;

                var element = CreateComponentElement(c);
                this.Add(element);
            }
        }

        private VisualElement CreateComponentElement(Component component)
        {
            var container = new VisualElement();
            container.AddToClassList("component-element");

            // ===== Inspector =====
            if (!_cqv._componentInspectorStates.TryGetValue(component, out var componentInspectorState))
            {
                componentInspectorState = new();
                _cqv._componentInspectorStates[component] = componentInspectorState;
            }

            componentInspectorState.Editor = Editor.CreateEditor(component);

            var titleBar = new VisualElement();
            titleBar.AddToClassList("title-bar");

            var nativeComponentHeader = new IMGUIContainer(() =>
            {
                EditorGUILayout.InspectorTitlebar(
                    componentInspectorState.IsExpanded,
                    componentInspectorState.Editor);
            });

            nativeComponentHeader.AddToClassList("native-component-header");

            var subComponentHeader = new VisualElement();
            subComponentHeader.AddToClassList("sub-component-header");

            subComponentHeader.Add(CreateHideButton(component));

            var inspector = new InspectorElement(componentInspectorState.Editor);

            inspector.style.marginTop = 4;
            inspector.style.marginBottom = 4;
            inspector.style.display = componentInspectorState.IsExpanded
                ? DisplayStyle.Flex
                : DisplayStyle.None;

            nativeComponentHeader.RegisterCallback<MouseDownEvent>(evt =>
            {
                if (evt.button != 0) return; // Left mouse button

                componentInspectorState.IsExpanded = !componentInspectorState.IsExpanded;

                inspector.style.display = componentInspectorState.IsExpanded
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
                _cqv._componentInspectorStates[component].IsVisible = false;
                _cqv.RepaintWindow();
            };

            return hideBtn;
        }
    }
}