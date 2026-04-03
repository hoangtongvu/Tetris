using Game.Common;
using Game.Domain.PubSub.Messengers;
using System;
using UnityEngine;
using ZBase.Foundation.PubSub;

namespace Game.Mono;

public record struct RedrawCurrentBlockMessage : IMessage;

[Serializable]
public class RedrawCurrentBlockMessagePublisher : MicroBehaviour
{
    [SerializeField] private CurrentBlockTransformedEventHolder currentBlockTransformedEvent;

    public override void LoadComponents()
    {
        this.FindFirstObjectByType(out this.currentBlockTransformedEvent);
    }

    public override void Update()
    {
        if (this.currentBlockTransformedEvent.Value.Value)
        {
            this.currentBlockTransformedEvent.Value.Value = false;
            GameplayMessenger.MessagePublisher.Publish(new RedrawCurrentBlockMessage());
        }
    }
}