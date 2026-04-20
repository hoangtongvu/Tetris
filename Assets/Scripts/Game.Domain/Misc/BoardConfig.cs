using System;

namespace Game.Mono;

[Serializable]
public class BoardConfig
{
    public byte Width;
    public byte Height;
    public float CellWorldSize;
}