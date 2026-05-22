using Game.Common;
using Game.Domain;
using System;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Mono.Player;

[Serializable]
public class PlayerBlockRotator : MicroBehaviour
{
    private BoardConfig boardConfig;
    private BoardCellArray boardCellArray;
    private CurrentBlockRef currentBlock;
    private CurrentBlockTransformedEvent currentBlockTransformedEvent;

    public override void InjectDependencies()
    {
        this.InjectSingle(out this.boardConfig);
        this.InjectSingle(out this.boardCellArray);
        this.InjectSingle(out this.currentBlock);
        this.InjectSingle(out this.currentBlockTransformedEvent);
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
            BlockPositionCheckingHelpers.CheckHorizontalBorders(boardConfig, blockData.CenterPosition, tempOffsets) &&
            BlockPositionCheckingHelpers.CheckBottomBorder(blockData.CenterPosition, tempOffsets) &&
            BlockPositionCheckingHelpers.CheckCollision(boardConfig, this.boardCellArray, blockData.CenterPosition, tempOffsets);

        if (!canMove) return;

        this.currentBlockTransformedEvent.Value = true;
        blockData.CellOffsets = tempOffsets;
    }

    private static int2 Rotate90CW(int2 v)
    {
        return new int2(v.y, -v.x);
    }
}