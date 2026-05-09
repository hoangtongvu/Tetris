using Game.Domain.GameCommands;
using Game.UI.Common;
using Game.UI.Common.Pooling;
using System;

namespace Game.UI.BootScreen;

[Serializable]
public class TryRentBootScreenCommand : IGameCommand
{
    public void Execute()
    {
        if (BootScreen_Ctrl.Instance)
            return;

        BootScreen_Ctrl.Instance = SharedUICtrlPoolMap.Rent(UIType.BootScreen) as BootScreen_Ctrl;
    }
}