using Game.Common;
using Game.Domain;
using System;
using UnityEngine;

namespace Game.Mono;

[Serializable]
public class BlockAutoDropper : MicroBehaviour
{
    private CanLockCurrentBlockTag canLockCurrentBlockTag;
    private BoardConfig boardConfig;
    private CurrentBlockRef currentBlock;
    private CurrentBlockTransformedEvent currentBlockTransformedEvent;
    [SerializeField] private float autoDropIntervalSeconds = 1f;
    private float autoDropTimer = 0;

    public override void InjectDependencies()
    {
        this.InjectSingle(out this.canLockCurrentBlockTag);
        this.InjectSingle(out this.boardConfig);
        this.InjectSingle(out this.currentBlock);
        this.InjectSingle(out this.currentBlockTransformedEvent);
    }

    public override void Update()
    {
        this.autoDropTimer += Time.deltaTime;

        if (this.autoDropTimer < this.autoDropIntervalSeconds)
            return;

        this.autoDropTimer = 0f;
        this.DropCurrentBlock();
    }

    private void DropCurrentBlock()
    {
        var blockData = this.currentBlock.Value;

        var tempPos = blockData.CenterPosition;
        tempPos.y--;

        bool canMove =
            BlockPositionCheckingHelpers.CheckBottomBorder(tempPos, blockData.CellOffsets) &&
            BlockPositionCheckingHelpers.CheckCollision(boardConfig, BoardCellArrayHolder.Instance.Value, tempPos, blockData.CellOffsets);

        if (!canMove)
        {
            this.canLockCurrentBlockTag.Value = true;
            return;
        }

        this.currentBlockTransformedEvent.Value = true;
        blockData.CenterPosition = tempPos;
    }
}