using Game.Domain.GameCommands;
using Game.Domain.PubSub.Messengers;
using System;
using ZBase.Foundation.PubSub;

namespace Game.UI.BootScreen;

public record struct ChangeToPreviousViewMessage() : IMessage;

[Serializable]
public class ChangeToPreviousViewCommand : IGameCommand
{
    public void Execute()
    {
        GameplayMessenger.MessagePublisher
            .Publish(new ChangeToPreviousViewMessage());
    }
}