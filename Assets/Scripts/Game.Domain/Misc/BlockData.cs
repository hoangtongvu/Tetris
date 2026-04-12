using System;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Domain;

[Serializable]
public class BlockData
{
    public int2 CenterPosition;
    public int2[] CellOffsets;
    public Color Color;
}