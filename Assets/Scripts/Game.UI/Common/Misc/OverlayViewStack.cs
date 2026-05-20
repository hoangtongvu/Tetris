using Game.UI.Common;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Game.UI;

[UxmlElement]
public partial class OverlayViewStack : VisualElement
{
    public Stack<BaseUITKCtrl> Stack = new();
    public event Action<BaseUITKCtrl> StackPushedEvent;

    public OverlayViewStack() : base()
    {
        this.style.position = Position.Absolute;
        this.style.left = 0;
        this.style.right = 0;
        this.style.top = 0;
        this.style.bottom = 0;
    }

    public void Push(BaseUITKCtrl viewCtrl)
    {
        var view = viewCtrl.UIDocument.rootVisualElement;

        view.style.position = Position.Absolute;
        view.style.left = 0;
        view.style.right = 0;
        view.style.top = 0;
        view.style.bottom = 0;

        this.Add(view);
        this.Stack.Push(viewCtrl);
        this.StackPushedEvent?.Invoke(viewCtrl);
    }

    public void Pop()
    {
        if (this.Stack.Count == 0)
            return;

        this.RemoveAt(this.Stack.Count - 1);

        var viewCtrl = this.Stack.Pop();
        viewCtrl.ReturnSelfToPool();
    }
}