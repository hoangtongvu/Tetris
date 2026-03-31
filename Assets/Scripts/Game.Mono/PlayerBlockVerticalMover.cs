using Cysharp.Threading.Tasks;
using Game.Common;
using System;
using UnityEngine;

namespace Game.Mono;

[Serializable]
public class PlayerBlockVerticalMover : MicroBehaviour
{
    [SerializeField] private BoardConfigHolder boardConfig;
    [SerializeField] private CurrentBlockRef currentBlock;
    [SerializeField] private float moveCooldownSeconds = 0.1f;

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
            BlockPositionCheckingHelpers.CheckCollision(boardConfig.Value, BoardCellArrayHolder.Instance.Value, tempPos, blockData.CellOffsets);

        if (!canMove) return;

        blockData.CenterPosition = tempPos;
    }
}