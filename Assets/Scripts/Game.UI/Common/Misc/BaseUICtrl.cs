using DSPool;
using Game.Common;
using Game.UI.Common.Pooling;
using Game.UI.Common.UIRendering;
using SaintsField;
using UnityEngine;

namespace Game.UI.Common;

public abstract class BaseUICtrl : SaiMonoBehaviour, IPoolElement
{
    [Header(nameof(BaseUICtrl))]
    [ReadOnly] public UIRuntimeId UIRuntimeId;
    public UIRenderingConfig UIRenderingConfig;
    [ReadOnly] public UIState State = UIState.Visible;

    protected virtual void OnEnable() => this.State = UIState.Visible;

    protected virtual void OnDisable() => this.State = UIState.Hidden;

    public abstract UIType GetUIType();

    public virtual void TriggerHiding() => this.ReturnSelfToPool();

    public void ReturnSelfToPool() => SharedUICtrlPoolMap.Return(this);

    public abstract void OnRent();

    public abstract void OnReturn();
}