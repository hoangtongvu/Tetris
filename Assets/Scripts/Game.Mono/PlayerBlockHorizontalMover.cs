using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Game.Mono;

[Serializable]
public class PlayerBlockHorizontalMover : MicroBehaviour
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
            BlockPositionCheckingHelpers.CheckHorizontalBorders(this.boardConfig.Value, tempPos, blockData.CellOffsets) &&
            BlockPositionCheckingHelpers.CheckCollision(boardConfig.Value, BoardCellArrayHolder.Instance.Value, tempPos, blockData.CellOffsets);

        if (!canMove) return;

        blockData.CenterPosition = tempPos;
    }
}