using Cysharp.Threading.Tasks;
using Game.Common;
using Game.Domain;
using System;
using UnityEngine;

namespace Game.Mono;

[Serializable]
public class PlayerBlockVerticalMover : MicroBehaviour
{
    private BoardConfig boardConfig;
    private CurrentBlockRef currentBlock;
    private CurrentBlockTransformedEvent currentBlockTransformedEvent;
    [SerializeField] private float moveCooldownSeconds = 0.1f;

    public override void InjectDependencies()
    {
        this.InjectSingle(out this.boardConfig);
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

            if (Input.GetKey(KeyCode.DownArrow))
            {
                this.MoveBlockY(-1);
            }
        }
    }

    private void MoveBlockY(sbyte input)
    {
        var blockData = this.currentBlock.Value;

        var tempPos = blockData.CenterPosition;
        tempPos.y += input;

        bool canMove =
            BlockPositionCheckingHelpers.CheckBottomBorder(tempPos, blockData.CellOffsets) &&
            BlockPositionCheckingHelpers.CheckCollision(boardConfig, BoardCellArrayHolder.Instance.Value, tempPos, blockData.CellOffsets);

        if (!canMove) return;

        this.currentBlockTransformedEvent.Value = true;
        blockData.CenterPosition = tempPos;
    }
}