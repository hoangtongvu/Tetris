using Game.Common;
using Game.Domain;
using Game.Domain.GameSession;
using Game.Domain.Lines;
using System;
using UnityEngine;

namespace Game.Mono.GameSession;

[Serializable]
public class GameSessionDataController : MicroBehaviour
{
    private GameSessionData sessionData;
    private BlockLockedEvent blockLockedEvent;
    private CompletedLinesEvent completedLinesEvent;

    public override void InjectDependencies()
    {
        this.InjectSingle(out this.sessionData);
        this.InjectSingle(out this.blockLockedEvent);
        this.InjectSingle(out this.completedLinesEvent);
    }

    public override void Update()
    {
        if (this.blockLockedEvent.Value)
        {
            this.sessionData.PlacedBlockCount++;
        }

        if (this.completedLinesEvent.Value)
        {
            this.sessionData.CompletedLineCount += this.completedLinesEvent.Count;
        }

        this.sessionData.SessionTimer.Tick(Time.deltaTime);
        this.sessionData.BlocksPerSecond = this.sessionData.PlacedBlockCount / this.sessionData.SessionTimer.TimeMilliseconds * 1000;
    }
}