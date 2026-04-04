using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ComponentQuickView : EditorWindow
{
    private GameObject _target;

    private Dictionary<Component, bool> _visibility = new Dictionary<Component, bool>();
    private Dictionary<Component, Editor> _editors = new Dictionary<Component, Editor>();

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

        _target = Selection.activeGameObject;
        _visibility.Clear();

        if (_target == null) return;

        var components = _target.GetComponents<Component>();

        foreach (var c in components)
        {
            if (c == null) continue;

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
            float buttonHeight = 20f;

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
            GUILayout.Space(2);
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        GUILayout.Space(4); // ✅ critical: separate from inspector below
    }

    // ---------------- INSPECTORS ----------------

    private void DrawComponentInspectors()
    {
        var components = _target.GetComponents<Component>();

        _scroll = EditorGUILayout.BeginScrollView(_scroll);

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

        EditorGUILayout.EndScrollView();
    }

    private void DrawSingleComponent(Component component, Editor editor)
    {
        EditorGUILayout.BeginVertical("box");

        // Title
        EditorGUILayout.BeginHorizontal();

        Texture icon = AssetPreview.GetMiniThumbnail(component);

        GUILayout.Label(icon, GUILayout.Width(20), GUILayout.Height(20));
        GUILayout.Label(component.GetType().Name, EditorStyles.boldLabel);

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Hide", GUILayout.Width(50)))
        {
            _visibility[component] = false;
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(3);

        // Inspector
        editor.OnInspectorGUI();

        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(4);
    }
}