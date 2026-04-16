using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI.Common;

[UxmlElement]
public partial class MyButtonUI_Element : VisualElement
{
    [UxmlAttribute]
    public string myStringValue { get; set; }

    [UxmlAttribute]
    public int myIntValue { get; set; }

    [UxmlAttribute]
    public GameObject myGameObjectValue { get; set; }

    public MyButtonUI_Element()
    {
        this.Add(new Label("Hello"));

        this.RegisterCallback<ClickEvent>(evt =>
        {
            Debug.Log("Custom element clicked");
        });
    }
}