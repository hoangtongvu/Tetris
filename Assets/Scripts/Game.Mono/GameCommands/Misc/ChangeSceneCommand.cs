using Game.Domain.GameCommands;
using SaintsField;
using System;
using UnityEngine.SceneManagement;

namespace Game.Mono.GameCommands;

[Serializable]
public class ChangeSceneCommand : IGameCommand
{
    [Scene] public int SceneInt;

    public void Execute()
    {
        SceneManager.LoadScene(this.SceneInt);
    }
}