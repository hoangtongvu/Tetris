using UnityEditor;
using UnityEngine;

public partial class ComponentQuickView : EditorWindow
{
    private static void DrawContextMenu(Rect titleRect, Component component, GUIStyle buttonStyle)
    {
        var menuIcon = EditorGUIUtility.IconContent("_Menu");
        var mouseRect = new Rect(Event.current.mousePosition, Vector2.zero);
        var e = Event.current;

        if (
            e.type == EventType.MouseDown &&
            e.button == 1 &&
            titleRect.Contains(e.mousePosition))
        {
            EditorContextMenuUtil.Show(mouseRect, component);
            e.Use();
        }

        if (GUILayout.Button(menuIcon, buttonStyle, GUILayout.Width(24), GUILayout.Height(20)))
        {
            EditorContextMenuUtil.Show(mouseRect, component);
        }
    }
}