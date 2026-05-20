using Game.Domain.GameCommands;
using Game.Domain.GameModes;
using System;

namespace Game.UI.BootScreen;

[Serializable]
public class ChangeGameModeCommand : IGameCommand
{
    public GameMode GameMode;

    public void Execute()
    {
        //BootScreen_Ctrl.Instance.BeforeGameplayData.GameMode = this.GameMode;
    }
}