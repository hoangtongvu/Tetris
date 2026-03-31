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
    [SerializeField] private float autoDropIntervalSeconds = 1f;
    private float autoDropTimer = 0;

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

        blockData.CenterPosition = tempPos;
    }
}