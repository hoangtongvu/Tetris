using Game.Domain.GameCommands;
using Game.Domain.GameStates;
using Game.Domain.GameStates.Messages;
using Game.Domain.PubSub.Messengers;
using System;
using ZBase.Foundation.PubSub;

namespace Game.Mono.GameStates;

[Serializable]
public class ChangeGameStateCommand : IGameCommand
{
    public GameState GameState;

    public void Execute()
    {
        GameplayMessenger.MessagePublisher
            .Publish(new ChangeGameStateMessage(this.GameState));
    }
}