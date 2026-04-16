using Game.Domain.GameCommands;
using Game.UI.Common;
using Game.UI.Common.Pooling;
using System;

namespace Game.UI.BootScreen;

[Serializable]
public class RentBootScreenCommand : IGameCommand
{
    public void Execute()
    {
        BootScreen_Ctrl.Instance = SharedUICtrlPoolMap.Rent(UIType.BootScreen) as BootScreen_Ctrl;
    }
}