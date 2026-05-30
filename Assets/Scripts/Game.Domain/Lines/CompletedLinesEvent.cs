using System;

namespace Game.Domain.Lines;

[Serializable]
public class CompletedLinesEvent
{
    public bool Value;
    public int Count;
}