using Game.Common;
using Game.Domain;
using System;

namespace Game.Mono.Block;

[Serializable]
public class BlockLocker : MicroBehaviour
{
    private CanLockCurrentBlockTag canLockCurrentBlockTag;
    private BoardCellArray boardCellArray;
    private BlockLockedEvent blockLockedEvent;
    private CurrentBlockRef currentBlock;

    public override void InjectDependencies()
    {
        this.InjectSingle(out this.canLockCurrentBlockTag);
        this.InjectSingle(out this.boardCellArray);
        this.InjectSingle(out this.blockLockedEvent);
        this.InjectSingle(out this.currentBlock);
    }

    public override void Update()
    {
        this.blockLockedEvent.Value = false;

        if (!this.canLockCurrentBlockTag.Value)
            return;

        this.canLockCurrentBlockTag.Value = false;
        this.LockCurrentBlock();
    }

    private void LockCurrentBlock()
    {
        this.blockLockedEvent.Value = true;
        var board = this.boardCellArray.Value;
        var centerPos = this.currentBlock.Value.CenterPosition;

        foreach (var cellOffset in this.currentBlock.Value.CellOffsets)
        {
            var cellPos = centerPos + cellOffset;
            board[cellPos.x][cellPos.y].IsValid = true;
            board[cellPos.x][cellPos.y].Color = this.currentBlock.Value.Color;
        }
    }
}