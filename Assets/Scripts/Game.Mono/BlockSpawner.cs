using Game.Common;
using Game.Domain;
using System;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Mono;

[Serializable]
public class BlockSpawner : MicroBehaviour
{
    [SerializeField] private BlockLockedEventHolder blockLockedEvent;
    [SerializeField] private BoardConfigHolder boardConfigHolder;
    [SerializeField] private CurrentBlockRef currentBlock;
    [SerializeField] private CurrentBlockTransformedEventHolder currentBlockTransformedEvent;

    public static readonly int2[][] blockOffsets = new int2[][]
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

    public override void LoadComponents()
    {
        this.FindFirstObjectByType(out this.blockLockedEvent);
        this.FindFirstObjectByType(out this.boardConfigHolder);
        this.FindFirstObjectByType(out this.currentBlock);
        this.FindFirstObjectByType(out this.currentBlockTransformedEvent);
    }

    public override void Update()
    {
        if (!this.blockLockedEvent.Value.Value)
            return;

        this.SpawnNewBlock();
    }

    private void SpawnNewBlock()
    {
        this.currentBlockTransformedEvent.Value.Value = true;
        int blockIndex = UnityEngine.Random.Range(0, blockOffsets.Length);

        this.currentBlock.Value = new BlockData
        {
            CenterPosition = new(this.boardConfigHolder.Value.Width / 2, this.boardConfigHolder.Value.Height + 1),
            CellOffsets = blockOffsets[blockIndex],
        };
    }
}