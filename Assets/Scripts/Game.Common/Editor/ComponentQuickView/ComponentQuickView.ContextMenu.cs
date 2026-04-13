using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public partial class ComponentQuickView : EditorWindow
{
    private static Button CreateShowContextMenuButton(Component component)
    {
        var button = new Button();
        button.text = "⋮";
        button.AddToClassList("util-button");

        button.clicked += () =>
        {
            var mouseRect = new Rect(Event.current.mousePosition, Vector2.zero);
            EditorContextMenuUtil.Show(mouseRect, component);
        };

        return button;
    }
}