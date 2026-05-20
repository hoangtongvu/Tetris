using Game.Domain.GameCommands;
using Reflex.Attributes;
using System;

namespace Game.UI.BootScreen;

[Serializable]
[SourceGeneratorInjectable]
public partial class PopViewFromBootScreenCommand : IGameCommand
{
    public void Execute()
    {
        // TODO: These commands should know about the caller (GameScreenStateMachine)
        var bootScreenCtrl = (BootScreen_Ctrl)GameScreenStateMachine.Instance.CurrentScreenCtrl;

        bootScreenCtrl.OverlayViewStack.Pop();
    }
}