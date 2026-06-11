using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public partial class ComponentQuickView : EditorWindow
{
    private GameObject _target;

    private Dictionary<Component, bool> _visibility = new();
    private Dictionary<Component, ComponentInspectorData> _editors = new();
    private Dictionary<int, Dictionary<int, bool>> _traversedGameObjs = new();

    private bool _isLocked = false;

    [MenuItem("Tools/Component Quick View")]
    public static void Open()
    {
        GetWindow<ComponentQuickView>("Component View");
    }

    private void OnEnable()
    {
        EditorApplication.hierarchyChanged += RepaintWindow;
    }

    private void OnDisable()
    {
        EditorApplication.hierarchyChanged -= RepaintWindow;
    }

    public void CreateGUI()
    {
        rootVisualElement.style.flexDirection = FlexDirection.Column;

        _unityHeader = new(this);
        rootVisualElement.Add(_unityHeader);

        _header = new(this);
        rootVisualElement.Add(_header);

        _componentToolBar = new(this);
        rootVisualElement.Add(_componentToolBar);

        _componentInspectors = new(this);
        rootVisualElement.Add(_componentInspectors);
    }

    private void OnSelectionChange()
    {
        if (_isLocked) return;

        var targetObject = Selection.activeObject;
        rootVisualElement.style.opacity = targetObject ? 1 : 0;

        RefreshSelection();
    }

    private void RefreshSelection()
    {
        // Save visibility before cleaning up
        if (_target != null)
        {
            var temp = new Dictionary<int, bool>();
            foreach (var kVPair in _visibility)
                temp.Add(kVPair.Key.GetEntityId(), kVPair.Value);

            _traversedGameObjs[_target.GetEntityId()] = temp;
        }

        _target = Selection.activeGameObject;
        _visibility.Clear();

        if (_target == null)
        {
            RepaintWindow();
            return;
        }

        // Retrieve saved visibility
        bool canRetrieveSavedVisibility = _traversedGameObjs.TryGetValue(_target.GetEntityId(), out var savedVisibility);

        var components = _target.GetComponents<Component>();

        foreach (var c in components)
        {
            if (c == null) continue;

            if (canRetrieveSavedVisibility)
                _visibility[c] = !savedVisibility.TryGetValue(c.GetEntityId(), out bool v) || v;
            else
                _visibility[c] = true;
        }

        RepaintWindow();
    }

    private void SetAllVisibility(bool value)
    {
        var components = _target.GetComponents<Component>();

        foreach (var c in components)
        {
            if (c == null) continue;

            _visibility[c] = value;
        }

        RepaintWindow();
    }

    private void RepaintWindow()
    {
        _componentToolBar.Refresh();
        _componentInspectors.Refresh();
    }
}