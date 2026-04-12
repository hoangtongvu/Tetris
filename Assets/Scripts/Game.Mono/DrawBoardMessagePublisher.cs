using Game.Common;
using Game.Domain.PubSub.Messengers;
using System;
using UnityEngine;
using ZBase.Foundation.PubSub;

namespace Game.Mono;

public record struct DrawBoardMessage : IMessage;

[Serializable]
public class DrawBoardMessagePublisher : MicroBehaviour
{
    [SerializeField] private BlockLockedEventHolder blockLockedEvent;

    public override void LoadComponents()
    {
        this.FindAnyObjectByType(out this.blockLockedEvent);
    }

    public override void Start()
    {
        this.SendDrawMessage();
    }

    public override void Update()
    {
        bool canDraw = this.blockLockedEvent.Value.Value;
        if (!canDraw) return;

        this.SendDrawMessage();
    }

    private void SendDrawMessage()
    {
        GameplayMessenger.MessagePublisher.Publish(new DrawBoardMessage());

    }
}