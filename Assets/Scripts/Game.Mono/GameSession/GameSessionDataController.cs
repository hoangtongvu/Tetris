using Game.Common;
using Game.Domain;
using Game.Domain.Lines;
using System;
using UnityEngine;

namespace Game.Mono.GameSession;

[Serializable]
public class GameSessionDataController : MicroBehaviour
{
    private BlockLockedEvent blockLockedEvent;
    private CompletedLinesEvent completedLinesEvent;

    public override void InjectDependencies()
    {
        this.InjectSingle(out this.blockLockedEvent);
        this.InjectSingle(out this.completedLinesEvent);
    }

    public override void Update()
    {
        var sessionData = GameSessionManager.Instance.SessionData;

        if (this.blockLockedEvent.Value)
        {
            sessionData.PlacedBlockCount++;
        }

        if (this.completedLinesEvent.Value)
        {
            sessionData.CompletedLineCount += this.completedLinesEvent.Count;
        }

        sessionData.SessionTimer.Tick(Time.deltaTime);
        sessionData.BlocksPerSecond = sessionData.PlacedBlockCount / sessionData.SessionTimer.TimeMilliseconds * 1000;
    }
}