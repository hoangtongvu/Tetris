using Game.Common;
using System;
using UnityEngine;

namespace Game.Mono;

[Serializable]
public class BlockLocker : MicroBehaviour
{
    [SerializeField] private CanLockCurrentBlockTagHolder canLockCurrentBlockTag;
    [SerializeField] private BlockLockedEventHolder blockLockedEvent;
    [SerializeField] private CurrentBlockRef currentBlock;

    public override void LoadComponents()
    {
        this.FindFirstObjectByType(out this.canLockCurrentBlockTag);
        this.FindFirstObjectByType(out this.blockLockedEvent);
        this.FindFirstObjectByType(out this.currentBlock);
    }

    public override void Update()
    {
        this.blockLockedEvent.Value.Value = false;

        if (!this.canLockCurrentBlockTag.Value.Value)
            return;

        this.canLockCurrentBlockTag.Value.Value = false;
        this.LockCurrentBlock();
    }

    private void LockCurrentBlock()
    {
        this.blockLockedEvent.Value.Value = true;
        var board = BoardCellArrayHolder.Instance.Value.Value;
        var centerPos = this.currentBlock.Value.CenterPosition;

        foreach (var cellOffset in this.currentBlock.Value.CellOffsets)
        {
            var cellPos = centerPos + cellOffset;
            board[cellPos.x][cellPos.y].IsValid = true;
            board[cellPos.x][cellPos.y].Color = this.currentBlock.Value.Color;
        }
    }
}