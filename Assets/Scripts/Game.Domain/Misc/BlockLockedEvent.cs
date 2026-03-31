using System;

namespace Game.Domain;

[Serializable]
public struct BlockLockedEvent
{
    public bool Value;
}