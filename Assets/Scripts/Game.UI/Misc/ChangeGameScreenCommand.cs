using Game.Domain.GameCommands;
using Game.Domain.PubSub.Messengers;
using Game.UI.Common;
using System;
using ZBase.Foundation.PubSub;

namespace Game.UI;

[Serializable]
public class ChangeGameScreenCommand : IGameCommand
{
    public UIType ScreenType;

    public void Execute()
    {
        GameplayMessenger.MessagePublisher
            .Publish(new ChangeGameScreenMessage(this.ScreenType));
    }
}