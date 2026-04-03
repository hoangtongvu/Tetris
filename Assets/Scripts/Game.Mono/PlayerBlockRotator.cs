using Game.Common;
using System;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Mono;

[Serializable]
public class PlayerBlockRotator : MicroBehaviour
{
    [SerializeField] private BoardConfigHolder boardConfig;
    [SerializeField] private CurrentBlockRef currentBlock;
    [SerializeField] private CurrentBlockTransformedEventHolder currentBlockTransformedEvent;

    public override void LoadComponents()
    {
        this.FindFirstObjectByType(out this.boardConfig);
        this.FindFirstObjectByType(out this.currentBlock);
        this.FindFirstObjectByType(out this.currentBlockTransformedEvent);
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.RotateBlock();
        }
    }

    private void RotateBlock()
    {
        var blockData = this.currentBlock.Value;

        var tempOffsets = blockData.CellOffsets.ToArray();
        int length = tempOffsets.Length;

        for (int i = 0; i < length; i++)
        {
            var offset = tempOffsets[i];
            tempOffsets[i] = Rotate90CW(offset);
        }

        bool canMove =
            BlockPositionCheckingHelpers.CheckHorizontalBorders(boardConfig.Value, blockData.CenterPosition, tempOffsets) &&
            BlockPositionCheckingHelpers.CheckBottomBorder(blockData.CenterPosition, tempOffsets) &&
            BlockPositionCheckingHelpers.CheckCollision(boardConfig.Value, BoardCellArrayHolder.Instance.Value, blockData.CenterPosition, tempOffsets);

        if (!canMove) return;

        this.currentBlockTransformedEvent.Value.Value = true;
        blockData.CellOffsets = tempOffsets;
    }

    private static int2 Rotate90CW(int2 v)
    {
        return new int2(v.y, -v.x);
    }
}