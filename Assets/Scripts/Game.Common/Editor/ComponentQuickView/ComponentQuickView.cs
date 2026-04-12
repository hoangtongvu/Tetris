using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public partial class ComponentQuickView : EditorWindow
{
    private GameObject _target;

    private Dictionary<Component, bool> _visibility = new Dictionary<Component, bool>();
    private Dictionary<Component, Editor> _editors = new Dictionary<Component, Editor>();
    private Dictionary<int, Dictionary<int, bool>> _traversedGameObjs = new();

    private Vector2 _scroll;
    private bool _isLocked = false;

    [MenuItem("Tools/Component Quick View")]
    public static void Open()
    {
        GetWindow<ComponentQuickView>("Component View");
    }

    private void OnEnable()
    {
        RefreshSelection();
    }

    private void OnDisable()
    {
        CleanupEditors();
    }

    private void OnSelectionChange()
    {
        if (_isLocked) return;

        RefreshSelection();
        Repaint();
    }

    private void RefreshSelection()
    {
        CleanupEditors();

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

        if (_target == null) return;

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

            _editors[c] = Editor.CreateEditor(c);
        }
    }

    private void CleanupEditors()
    {
        foreach (var kvp in _editors)
        {
            if (kvp.Value != null)
                DestroyImmediate(kvp.Value);
        }

        _editors.Clear();
    }

    private void OnGUI()
    {
        DrawHeader();

        if (_target == null)
        {
            EditorGUILayout.HelpBox("Select a GameObject to inspect.", MessageType.Info);
            return;
        }

        DrawComponentToolbar();
        DrawComponentInspectors();
    }

    // ---------------- HEADER ----------------

    private void DrawHeader()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

        GUILayout.Label("Component Quick View", EditorStyles.boldLabel);

        GUILayout.FlexibleSpace();

        // 🔒 Lock toggle
        if (_isLocked && _target != null)
        {
            EditorGUILayout.HelpBox(
                $"🔒: {_target.name}",
                MessageType.None
            );
        }

        GUIContent lockIcon = EditorGUIUtility.IconContent(
            _isLocked ? "LockIcon-On" : "LockIcon"
        );

        if (GUILayout.Button(lockIcon, EditorStyles.toolbarButton, GUILayout.Width(30)))
        {
            bool wasLocked = _isLocked;
            _isLocked = !_isLocked;

            // 🔓 Just unlocked → immediately follow current selection
            if (wasLocked && !_isLocked)
            {
                RefreshSelection();
                Repaint();
            }
        }

        if (GUILayout.Button("Show All", EditorStyles.toolbarButton))
        {
            SetAllVisibility(true);
        }

        if (GUILayout.Button("Hide All", EditorStyles.toolbarButton))
        {
            SetAllVisibility(false);
        }

        EditorGUILayout.EndHorizontal();
    }

    private void SetAllVisibility(bool value)
    {
        var components = _target.GetComponents<Component>();

        foreach (var c in components)
        {
            if (c == null) continue;

            _visibility[c] = value;
        }

        Repaint();
    }

    // ---------------- TOOLBAR ----------------
    private void DrawComponentToolbar()
    {
        if (_target == null) return;

        var components = _target.GetComponents<Component>();

        float viewWidth = position.width - 20f; // padding safety
        float x = 0f;

        // ❗ Use normal container instead of toolbar
        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.BeginHorizontal();

        foreach (var c in components)
        {
            if (c == null) continue;

            bool visible = _visibility.ContainsKey(c) && _visibility[c];

            Texture icon = AssetPreview.GetMiniThumbnail(c);
            string name = c.GetType().Name;

            GUIStyle style = EditorStyles.miniButton;

            Vector2 size = style.CalcSize(new GUIContent(name));
            float buttonWidth = Mathf.Min(size.x + 15f, 500f); // clamp width
            float buttonHeight = 18f;

            // 🔁 Wrap line
            if (x + buttonWidth > viewWidth)
            {
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                x = 0f;
            }

            GUI.backgroundColor = visible
                ? new Color(0.75f, 0.9f, 1f)
                : Color.black;

            if (GUILayout.Button(
                new GUIContent(" " + name, icon, name),
                style,
                GUILayout.Width(buttonWidth),
                GUILayout.Height(buttonHeight)))
            {
                _visibility[c] = !visible;
            }

            GUI.backgroundColor = Color.white;

            x += buttonWidth + 2f;
            GUILayout.Space(0);
        }

        DrawAddComponentButton();

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        GUILayout.Space(4); // ✅ critical: separate from inspector below
    }

    // ---------------- INSPECTORS ----------------
    private void DrawComponentInspectors()
    {
        var components = _target.GetComponents<Component>();

        _scroll = EditorGUILayout.BeginScrollView(_scroll);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(1); // left padding

        EditorGUILayout.BeginVertical();
        GUILayout.Space(1); // top padding

        foreach (var c in components)
        {
            if (c == null) continue;
            if (!_visibility.ContainsKey(c) || !_visibility[c]) continue;

            if (!_editors.ContainsKey(c) || _editors[c] == null)
            {
                _editors[c] = Editor.CreateEditor(c);
            }

            DrawSingleComponent(c, _editors[c]);
        }

        EditorGUILayout.EndVertical();
        GUILayout.Space(1); // right padding
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();
    }

    private void DrawSingleComponent(Component component, Editor editor)
    {
        Rect rect = EditorGUILayout.BeginVertical();

        // Title
        const int titleHeight = 18;
        var titleRect = EditorGUILayout.BeginHorizontal();
        GUILayout.Space(4);

        // Draw lighter background ONLY for title
        if (Event.current.type == EventType.Repaint)
        {
            EditorGUI.DrawRect(titleRect, new Color(0.25f, 0.25f, 0.25f));
        }

        Texture icon = AssetPreview.GetMiniThumbnail(component);
        GUILayout.Label(icon, GUILayout.Width(titleHeight), GUILayout.Height(titleHeight));
        GUILayout.Label(
            component.GetType().Name,
            EditorStyles.boldLabel,
            GUILayout.Height(titleHeight) // match icon height
        );

        GUILayout.FlexibleSpace();

        // Hide button
        var hideButtonStyle = new GUIStyle(GUI.skin.button);
        hideButtonStyle.border = new RectOffset(0, 0, 0, 0);
        hideButtonStyle.margin = new RectOffset(0, 0, 0, 0);
        hideButtonStyle.padding = new RectOffset(0, 0, 0, 0);

        var eyeIcon = EditorGUIUtility.IconContent("animationvisibilitytoggleon");
        if (GUILayout.Button(eyeIcon, hideButtonStyle, GUILayout.Width(24), GUILayout.Height(20)))
        {
            _visibility[component] = false;
        }

        DrawContextMenu(titleRect, component, hideButtonStyle);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(3);

        // Inspector
        editor.OnInspectorGUI();

        EditorGUILayout.Space(4);

        EditorGUILayout.EndVertical();

        // Draw border AFTER layout
        if (Event.current.type == EventType.Repaint)
        {
            Handles.DrawSolidRectangleWithOutline(
                rect,
                Color.clear,
                new Color(0.3f, 0.3f, 0.3f)
            );
        }

        EditorGUILayout.Space(5);
    }

    // ---------------- ADD COMPONENT BUTTON ----------------
    private void DrawAddComponentButton()
    {
        EditorGUILayout.BeginHorizontal();

        var buttonRect = GUILayoutUtility.GetRect(
            GUIContent.none,
            GUI.skin.button,
            GUILayout.Width(20),
            GUILayout.Height(18)
        );

        if (GUI.Button(buttonRect, EditorGUIUtility.IconContent("d_CreateAddNew")))
        {
            // Use 230 width so the popup renders at the correct size,
            // regardless of the button's actual visual width
            var screenRect = new Rect(buttonRect.x, buttonRect.y, 230, buttonRect.height);

            var addComponentWindow = System.Type.GetType(
                "UnityEditor.AddComponent.AddComponentWindow, UnityEditor");

            if (addComponentWindow != null)
            {
                var method = addComponentWindow.GetMethod(
                    "Show",
                    BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

                method?.Invoke(null, new object[] { screenRect, new[] { _target } });
            }
        }

        EditorGUILayout.EndHorizontal();
    }
}