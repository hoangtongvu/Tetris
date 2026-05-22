using Game.Common;
using Game.Domain;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Mono.Block;

[Serializable]
public class BlockSpawner : MicroBehaviour
{
    private BlockLockedEvent blockLockedEvent;
    private BoardConfig boardConfig;
    private CurrentBlockRef currentBlock;
    private CurrentBlockTransformedEvent currentBlockTransformedEvent;
    [SerializeField] private int batchIndexCount = 4;
    private Queue<int> batchIndexes;

    public IEnumerable<int> BatchIndexes => this.batchIndexes;

    public override void Awake()
    {
        this.batchIndexes = new(this.batchIndexCount);
    }

    public override void InjectDependencies()
    {
        this.InjectSingle(out this.blockLockedEvent);
        this.InjectSingle(out this.boardConfig);
        this.InjectSingle(out this.currentBlock);
        this.InjectSingle(out this.currentBlockTransformedEvent);
    }

    public override void Update()
    {
        if (!this.blockLockedEvent.Value)
            return;

        this.SpawnNewBlock();
    }

    private void SpawnNewBlock()
    {
        this.TryFillIndexQueue();
        this.currentBlockTransformedEvent.Value = true;
        int blockIndex = this.batchIndexes.Dequeue();

        this.currentBlock.Value = new BlockData
        {
            CenterPosition = new(this.boardConfig.Width / 2, this.boardConfig.Height + 1),
            CellOffsets = BlockOffsets.Value[blockIndex],
            Color = UnityEngine.Random.ColorHSV(),
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