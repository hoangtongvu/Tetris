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

            // Copy component values button
            this.Add(this.CreateCopyComponentValuesButton());

            // Paste component values button
            this.Add(this.CreatePasteComponentValuesButton());

            // Paste values or Add components button
            this.Add(this.CreatePasteValuesOrAddComponentsButton());

            // Create GameObj from copied components button
            this.Add(this.CreateCreateGameObjFromCopiedComponentsButton());
        }

        private ToolbarButton CreateCopyComponentValuesButton()
        {
            var btn = new ToolbarButton();
            btn.tooltip = "Copy selected Component values";
            var icon = EditorGUIUtility.IconContent("TreeEditor.Duplicate");
            btn.iconImage = icon.image as Texture2D;
            btn.style.width = _headerHeight;

            btn.clicked += () =>
            {
                var inspectorStates = _cqv._componentInspectorStates;
                var copiedComponents = _cqv._copiedComponents;

                copiedComponents.Clear();

                foreach (var kvPair in inspectorStates)
                {
                    if (!kvPair.Value.IsVisible) continue;

                    copiedComponents.Add(kvPair.Key);
                }
            };

            return btn;
        }

        private ToolbarButton CreatePasteComponentValuesButton()
        {
            var btn = new ToolbarButton();
            btn.tooltip = "Paste selected Component values";
            var icon = EditorGUIUtility.IconContent("d_UnityEditor.ConsoleWindow");
            btn.iconImage = icon.image as Texture2D;
            btn.style.width = _headerHeight;

            btn.clicked += () =>
            {
                var inspectorStates = _cqv._componentInspectorStates;
                var copiedComponents = _cqv._copiedComponents;

                foreach (var copiedComponent in copiedComponents)
                {
                    foreach (var kvPair in inspectorStates)
                    {
                        var destComponent = kvPair.Key;
                        if (copiedComponent.GetType() != destComponent.GetType()) continue;

                        EditorUtility.CopySerializedIfDifferent(copiedComponent, destComponent);
                        EditorUtility.SetDirty(destComponent);
                        break;
                    }
                }
            };

            return btn;
        }

        private ToolbarButton CreatePasteValuesOrAddComponentsButton()
        {
            var btn = new ToolbarButton();
            btn.tooltip = "Paste values or Add components";
            var icon = EditorGUIUtility.IconContent("d_Toolbar Plus");
            btn.iconImage = icon.image as Texture2D;
            btn.style.width = _headerHeight;

            btn.clicked += () =>
            {
                var inspectorStates = _cqv._componentInspectorStates;
                var copiedComponents = _cqv._copiedComponents;

                foreach (var copiedComponent in copiedComponents)
                {
                    bool hasDestComponent = false;

                    foreach (var kvPair in inspectorStates)
                    {
                        var destComponent = kvPair.Key;
                        if (copiedComponent.GetType() != destComponent.GetType()) continue;

                        EditorUtility.CopySerializedIfDifferent(copiedComponent, destComponent);
                        EditorUtility.SetDirty(destComponent);
                        hasDestComponent = true;
                        break;
                    }

                    if (!hasDestComponent)
                    {
                        var destComponent = _cqv._targetGO.AddComponent(copiedComponent.GetType());
                        EditorUtility.CopySerialized(copiedComponent, destComponent);
                        EditorUtility.SetDirty(destComponent);
                    }
                }
            };

            return btn;
        }

        private ToolbarButton CreateCreateGameObjFromCopiedComponentsButton()
        {
            var btn = new ToolbarButton();
            btn.tooltip = "Create GameObject from copied components";
            var icon = EditorGUIUtility.IconContent("GameObject Icon");
            btn.iconImage = icon.image as Texture2D;
            btn.style.width = _headerHeight;

            btn.clicked += () =>
            {
                var copiedComponents = _cqv._copiedComponents;
                var newGO = new GameObject();

                foreach (var copiedComponent in copiedComponents)
                {
                    if (!newGO.TryGetComponent(copiedComponent.GetType(), out var destComponent))
                        destComponent = newGO.AddComponent(copiedComponent.GetType());

                    EditorUtility.CopySerialized(copiedComponent, destComponent);
                }

                EditorUtility.SetDirty(newGO);
            };

            return btn;
        }
    }
}