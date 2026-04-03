using System;

namespace Game.Domain;

[Serializable]
public struct CurrentBlockTransformedEvent
{
    public bool Value;
}