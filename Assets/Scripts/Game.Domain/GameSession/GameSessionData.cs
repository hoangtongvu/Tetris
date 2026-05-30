using Game.Domain.GameModes;
using System;

namespace Game.Domain.GameSession;

[Serializable]
public class GameSessionData
{
    public GameMode GameMode;
    public int PlacedBlockCount;
    public int CompletedLineCount;
    public SessionTimer SessionTimer;
    public float BlocksPerSecond;
}