using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public partial class ComponentQuickView : EditorWindow
{
    private static Button CreateShowContextMenuButton(Component component)
    {
        var button = new Button();
        button.text = "⋮";
        button.style.height = 20;
        button.style.width = 20;
        button.style.marginLeft = 0;
        button.style.marginRight = 0;
        button.style.marginTop = 0;
        button.style.marginBottom = 0;

        button.clicked += () =>
        {
            var mouseRect = new Rect(Event.current.mousePosition, Vector2.zero);
            EditorContextMenuUtil.Show(mouseRect, component);
        };

        return button;
    }
}