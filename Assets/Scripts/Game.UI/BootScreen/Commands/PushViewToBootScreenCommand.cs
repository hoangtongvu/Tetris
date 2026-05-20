using Game.Domain.GameCommands;
using Game.UI.Common;
using Game.UI.Common.Pooling;
using System;

namespace Game.UI.BootScreen;

[Serializable]
public class PushViewToBootScreenCommand : IGameCommand
{
    public UIAssetId.SerializableWrapper UIAssetId;

    public void Execute()
    {
        var bootScreenCtrl = (BootScreen_Ctrl)GameScreenStateMachine.Instance.CurrentScreenCtrl;

        var newView = SharedUICtrlPoolMap.Rent(this.UIAssetId.Value.Type) as BaseUITKCtrl;
        newView.gameObject.SetActive(true);
        bootScreenCtrl.OverlayViewStack.Push(newView);
    }
}