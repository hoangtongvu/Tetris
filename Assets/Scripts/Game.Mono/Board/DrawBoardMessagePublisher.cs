using Game.Common;
using Game.Domain;
using Game.Domain.PubSub.Messengers;
using System;
using ZBase.Foundation.PubSub;

namespace Game.Mono.Board;

public record struct DrawBoardMessage : IMessage;

[Serializable]
public class DrawBoardMessagePublisher : MicroBehaviour
{
    private BlockLockedEvent blockLockedEvent;

    public override void InjectDependencies()
    {
        this.InjectSingle(out this.blockLockedEvent);
    }

    public override void Start()
    {
        this.SendDrawMessage();
    }

    public override void Update()
    {
        bool canDraw = this.blockLockedEvent.Value;
        if (!canDraw) return;

        this.SendDrawMessage();
    }

    private void SendDrawMessage()
    {
        GameplayMessenger.MessagePublisher.Publish(new DrawBoardMessage());
    }
}