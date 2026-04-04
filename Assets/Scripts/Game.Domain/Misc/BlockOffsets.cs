using Unity.Mathematics;

namespace Game.Domain;

public static class BlockOffsets
{
    public static readonly int2[][] Value = new int2[][]
    {
        // T-piece
        new int2[] { new(0,0), new(1,0), new(-1,0), new(0,1) },
        // I-piece
        new int2[] { new(0,0), new(-1,0), new(1,0), new(2,0) },
        // O-piece
        new int2[] { new(0,0), new(1,0), new(0,1), new(1,1) },
        // S-piece
        new int2[] { new(0,0), new(1,0), new(0,1), new(-1,1) },
        // Z-piece
        new int2[] { new(0,0), new(-1,0), new(0,1), new(1,1) },
        // J-piece
        new int2[] { new(0,0), new(-1,0), new(1,0), new(-1,1) },
        // L-piece
        new int2[] { new(0,0), new(-1,0), new(1,0), new(1,1) },
    };
}