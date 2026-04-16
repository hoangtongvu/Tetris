using DSPool;
using UnityEngine;

namespace Game.UI.Common.Pooling;

public class UICtrlPool : ComponentPool<BaseUICtrl>
{
    public uint GlobalID;
    public Transform DefaultHolderTransform { get; set; }

    protected override BaseUICtrl InstantiateElement()
    {
        var baseUICtrl = Object.Instantiate(this.Prefab, this.DefaultHolderTransform, false).GetComponent<BaseUICtrl>();

        this.GlobalID++;
        baseUICtrl.UIRuntimeId.Type = baseUICtrl.GetUIType();
        baseUICtrl.UIRuntimeId.Version = this.GlobalID;

        return baseUICtrl;
    }

    protected override void OnRent(BaseUICtrl element)
    {
        base.OnRent(element);
        element.OnRent();
    }

    protected override void OnReturn(BaseUICtrl element)
    {
        base.OnReturn(element);
        element.OnReturn();
        element.transform
            .SetParent(this.DefaultHolderTransform);
    }
}