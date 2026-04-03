using Game.Common;
using System;
using UnityEngine;

namespace Game.Mono;

[Serializable]
public class BlockAutoDropper : MicroBehaviour
{
    [SerializeField] private CanLockCurrentBlockTagHolder canLockCurrentBlockTag;
    [SerializeField] private BoardConfigHolder boardConfig;
    [SerializeField] private CurrentBlockRef currentBlock;
    [SerializeField] private CurrentBlockTransformedEventHolder currentBlockTransformedEvent;
    [SerializeField] private float autoDropIntervalSeconds = 1f;
    private float autoDropTimer = 0;

    public override void LoadComponents()
    {
        this.FindFirstObjectByType(out this.canLockCurrentBlockTag);
        this.FindFirstObjectByType(out this.boardConfig);
        this.FindFirstObjectByType(out this.currentBlock);
        this.FindFirstObjectByType(out this.currentBlockTransformedEvent);
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
            BlockPositionCheckingHelpers.CheckCollision(boardConfig.Value, BoardCellArrayHolder.Instance.Value, tempPos, blockData.CellOffsets);

        if (!canMove)
        {
            this.canLockCurrentBlockTag.Value.Value = true;
            return;
        }

        this.currentBlockTransformedEvent.Value.Value = true;
        blockData.CenterPosition = tempPos;
    }
}