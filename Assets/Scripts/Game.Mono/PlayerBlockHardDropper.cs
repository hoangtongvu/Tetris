using System;
using UnityEngine;

namespace Game.Mono;

[Serializable]
public class PlayerBlockHardDropper : MicroBehaviour
{
    [SerializeField] private CanLockCurrentBlockTagHolder canLockCurrentBlockTag;
    [SerializeField] private BoardConfigHolder boardConfig;
    [SerializeField] private CurrentBlockRef currentBlock;

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
                BlockPositionCheckingHelpers.CheckCollision(boardConfig.Value, BoardCellArrayHolder.Instance.Value, tempPos, blockData.CellOffsets)
            );

            tempPos.y++;
            blockData.CenterPosition = tempPos;
            this.canLockCurrentBlockTag.Value.Value = true;
        }
    }
}