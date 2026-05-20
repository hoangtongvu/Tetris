using Cysharp.Threading.Tasks;
using Game.Common;
using Game.Domain;
using System;
using UnityEngine;

namespace Game.Mono;

[Serializable]
public class PlayerBlockHorizontalMover : MicroBehaviour
{
    private BoardConfig boardConfig;
    private BoardCellArray boardCellArray;
    private CurrentBlockRef currentBlock;
    private CurrentBlockTransformedEvent currentBlockTransformedEvent;
    [SerializeField] private float moveCooldownSeconds = 0.1f;

    public override void InjectDependencies()
    {
        this.InjectSingle(out this.boardConfig);
        this.InjectSingle(out this.boardCellArray);
        this.InjectSingle(out this.currentBlock);
        this.InjectSingle(out this.currentBlockTransformedEvent);
    }

    public override void Start()
    {
        this.GetTask().Forget();
    }

    private async UniTaskVoid GetTask()
    {
        while (true)
        {
            await UniTask.WaitForSeconds(this.moveCooldownSeconds);

            if (currentBlock.Value == null)
                continue;

            if (Input.GetKey(KeyCode.RightArrow))
            {
                this.MoveBlockX(1);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                this.MoveBlockX(-1);
            }
        }
    }

    private void MoveBlockX(sbyte input)
    {
        var blockData = this.currentBlock.Value;

        var tempPos = blockData.CenterPosition;
        tempPos.x += input;

        bool canMove =
            BlockPositionCheckingHelpers.CheckHorizontalBorders(this.boardConfig, tempPos, blockData.CellOffsets) &&
            BlockPositionCheckingHelpers.CheckCollision(boardConfig, this.boardCellArray, tempPos, blockData.CellOffsets);

        if (!canMove) return;

        this.currentBlockTransformedEvent.Value = true;
        blockData.CenterPosition = tempPos;
    }
}