using Game.Domain.GameCommands;
using Game.UI.Common.Pooling;
using System;

namespace Game.UI.BootScreen;

[Serializable]
public class ReturnBootScreenCommand : IGameCommand
{
    public void Execute()
    {
        SharedUICtrlPoolMap.Return(BootScreen_Ctrl.Instance);
    }
}