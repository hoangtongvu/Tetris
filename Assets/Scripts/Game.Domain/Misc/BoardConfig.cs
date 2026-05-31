using System;
using Unity.Mathematics;

namespace Game.Mono;

[Serializable]
public class BoardConfig
{
    public byte Width;
    public byte Height;
    public float CellWorldSize;// TODO: Change to CellWorldWidth

    public float2 BoardWorldOffset
    {
        get
        {
            return new(- this.CellWorldSize * this.Width * 0.5f, - this.CellWorldSize * Height * 0.5f);
        }
    }
}