using Game.Common;
using Game.Domain;
using Game.Domain.PubSub.Messengers;
using System;
using ZBase.Foundation.PubSub;

namespace Game.Mono;

public record struct DrawCurrentBlockMessage : IMessage;

[Serializable]
public class DrawCurrentBlockMessagePublisher : MicroBehaviour
{
    private CurrentBlockTransformedEvent currentBlockTransformedEvent;

    public override void InjectDependencies()
    {
        this.InjectSingle(out this.currentBlockTransformedEvent);
    }

    public override void Update()
    {
        if (this.currentBlockTransformedEvent.Value)
        {
            this.currentBlockTransformedEvent.Value = false;
            GameplayMessenger.MessagePublisher.Publish(new DrawCurrentBlockMessage());
        }
    }
}