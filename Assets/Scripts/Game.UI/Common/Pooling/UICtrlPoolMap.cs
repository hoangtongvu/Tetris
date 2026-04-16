using DSPool;
using System.Collections.Generic;

namespace Game.UI.Common.Pooling;

[DSPoolSharedInstance]
public class UICtrlPoolMap : PoolMap<UIType, UICtrlPool, BaseUICtrl>
{
    private readonly Dictionary<UIRuntimeId, BaseUICtrl> rentedUIs = new();

    protected override void OnRent(UIType poolKey, UICtrlPool retrievedPool, BaseUICtrl poolElement)
    {
        base.OnRent(poolKey, retrievedPool, poolElement);
        rentedUIs.Add(poolElement.UIRuntimeId, poolElement);
    }

    protected override void OnReturn(UIType poolKey, UICtrlPool retrievedPool, BaseUICtrl poolElement)
    {
        base.OnReturn(poolKey, retrievedPool, poolElement);
        rentedUIs.Remove(poolElement.UIRuntimeId);
    }

    public void Return(UIRuntimeId uiRuntimeId)
    {
        var toReturnElement = this.rentedUIs[uiRuntimeId];
        this.Return(toReturnElement);
    }
}