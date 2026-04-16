using Game.Domain.GameCommands;
using Game.UI.Common;
using Game.UI.Common.Pooling;
using System;

namespace Game.UI.BootScreen;

[Serializable]
public class PushViewToBootScreenCommand : IGameCommand
{
    public UIType ViewUIType;

    public void Execute()
    {
        var newView = SharedUICtrlPoolMap.Rent(this.ViewUIType) as BaseUITKCtrl;
        BootScreen_Ctrl.Instance.OverlayViewStack.Push(newView);
    }
}