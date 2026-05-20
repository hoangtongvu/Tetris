using Game.Common;
using Game.Domain;
using System;
using UnityEngine;

namespace Game.Mono;

[Serializable]
public class PlayerBlockHardDropper : MicroBehaviour
{
    private CanLockCurrentBlockTag canLockCurrentBlockTag;
    private BoardConfig boardConfig;
    private BoardCellArray boardCellArray;
    private CurrentBlockRef currentBlock;
    private CurrentBlockTransformedEvent currentBlockTransformedEvent;

    public override void InjectDependencies()
    {
        this.InjectSingle(out this.canLockCurrentBlockTag);
        this.InjectSingle(out this.boardConfig);
        this.InjectSingle(out this.boardCellArray);
        this.InjectSingle(out this.currentBlock);
        this.InjectSingle(out this.currentBlockTransformedEvent);
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var blockData = this.currentBlock.Value;
            var tempPos = blockData.CenterPosition;

            do
            {
                tempPos.y--;
            } while (
                BlockPositionCheckingHelpers.CheckBottomBorder(tempPos, blockData.CellOffsets) &&
                BlockPositionCheckingHelpers.CheckCollision(boardConfig, this.boardCellArray, tempPos, blockData.CellOffsets)
            );

            tempPos.y++;
            blockData.CenterPosition = tempPos;
            this.canLockCurrentBlockTag.Value = true;
            this.currentBlockTransformedEvent.Value = true;
        }
    }
}