using Game.Domain.GameCommands;
using System;

namespace Game.UI.BootScreen;

[Serializable]
public class PopViewFromBootScreenCommand : IGameCommand
{
    public void Execute()
    {
        BootScreen_Ctrl.Instance.OverlayViewStack.Pop();
    }
}