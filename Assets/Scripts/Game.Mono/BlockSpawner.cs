using Game.Common;
using Game.Domain;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Mono;

[Serializable]
public class BlockSpawner : MicroBehaviour
{
    [SerializeField] private BlockLockedEventHolder blockLockedEvent;
    [SerializeField] private BoardConfigHolder boardConfigHolder;
    [SerializeField] private CurrentBlockRef currentBlock;
    [SerializeField] private CurrentBlockTransformedEventHolder currentBlockTransformedEvent;
    [SerializeField] private int batchIndexCount = 4;
    private Queue<int> batchIndexes;

    public IEnumerable<int> BatchIndexes => this.batchIndexes;

    public override void Awake()
    {
        this.batchIndexes = new(this.batchIndexCount);
    }

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
        this.TryFillIndexQueue();
        this.currentBlockTransformedEvent.Value.Value = true;
        int blockIndex = this.batchIndexes.Dequeue();

        this.currentBlock.Value = new BlockData
        {
            CenterPosition = new(this.boardConfigHolder.Value.Width / 2, this.boardConfigHolder.Value.Height + 1),
            CellOffsets = BlockOffsets.Value[blockIndex],
        };
    }

    private bool TryFillIndexQueue()
    {
        if (this.batchIndexes.Count >= this.batchIndexCount)
            return false;

        int count = this.batchIndexCount - this.batchIndexes.Count;

        for (int i = 0; i < count; i++)
        {
            int blockIndex = UnityEngine.Random.Range(0, BlockOffsets.Value.Length);
            this.batchIndexes.Enqueue(blockIndex);
        }

        return true;
    }
}