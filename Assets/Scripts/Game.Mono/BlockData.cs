using System;
using Unity.Mathematics;

namespace Game.Mono;

[Serializable]
public class BlockData
{
    public int2 CenterPosition;
    public int2[] CellOffsets;
}