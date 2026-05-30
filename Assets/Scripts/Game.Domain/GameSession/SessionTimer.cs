using System;
using Unity.Mathematics;

namespace Game.Domain.GameSession;

[Serializable]
public struct SessionTimer
{
    public float TimeMilliseconds;

    public void Tick(float deltaTimeSeconds)
    {
        this.TimeMilliseconds += deltaTimeSeconds * 1000;
    }

    public void GetFormatedTime(out int minutes, out int seconds, out int milliseconds)
    {
        int flooredMilliseconds = (int)math.floor(this.TimeMilliseconds);

        minutes = flooredMilliseconds / 60000;
        seconds = (flooredMilliseconds % 60000) / 1000;
        milliseconds = flooredMilliseconds - minutes * 60000 - seconds * 1000;
    }
}