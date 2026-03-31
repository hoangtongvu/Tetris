using System;

namespace Game.Mono;

[Serializable]
public struct BoardConfig
{
    public byte Width;
    public byte Height;
    public float CellWorldSize;
}