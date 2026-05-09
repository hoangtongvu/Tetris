using Game.Domain.PubSub.Messengers;
using Game.Domain.GameStates;
using Game.Domain.GameStates.Messages;
using System;
using UnityEngine.UIElements;
using ZBase.Foundation.PubSub;

namespace Game.UI.MainMenu;

[Serializable]
public class StartGameButton
{
    public Button Button;

    public void Bind(VisualElement root)
    {
        this.Button = root.Q<Button>(name: "start-game__button");
        this.Button.clicked += () =>
        {
            GameplayMessenger.MessagePublisher
                .Publish(new ChangeGameStateMessage(GameState.SelectGameMode));
        };
    }
}