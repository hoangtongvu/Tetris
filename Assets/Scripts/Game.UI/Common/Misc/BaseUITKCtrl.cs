using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI.Common;

[RequireComponent(typeof(UIDocument))]
public abstract class BaseUITKCtrl : BaseUICtrl
{
    [Header(nameof(BaseUITKCtrl))]
    [SerializeField] protected UIDocument uiDocument;

    public UIDocument UIDocument => uiDocument;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponentInCtrl(out this.uiDocument);
    }

    protected virtual void Awake()
    {
        this.LoadUIElements();
    }

    protected virtual void LoadUIElements() { }
}