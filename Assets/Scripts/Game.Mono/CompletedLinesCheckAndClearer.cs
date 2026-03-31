using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Mono;

[Serializable]
public class CompletedLinesCheckAndClearer : MicroBehaviour
{
    [SerializeField] private BlockLockedEventHolder blockLockedEvent;
    [SerializeField] private BoardConfigHolder boardConfig;

    public override void Update()
    {
        if (!this.blockLockedEvent.Value.Value)
            return;

        var board = BoardCellArrayHolder.Instance.Value;
        var completedLineIndexes = this.GetCompletedLineIndexes(board);

        if (completedLineIndexes.Count == 0)
            return;

        this.ClearCompletedLines(board, completedLineIndexes);
    }

    private List<int> GetCompletedLineIndexes(BoardCellArray board)
    {
        var completedLineIndexes = new List<int>();

        for (int y = 0; y < this.boardConfig.Value.Height; y++)
        {
            bool isLineCompleted = true;

            for (int x = 0; x < this.boardConfig.Value.Width; x++)
            {
                var cell = board.Value[x][y];

                if (!cell.IsValid)
                {
                    isLineCompleted = false;
                    break;
                }
            }

            if (isLineCompleted)
                completedLineIndexes.Add(y);
        }

        return completedLineIndexes;
    }

    private void ClearCompletedLines(BoardCellArray board, List<int> completedLineIndexes)
    {
        int highestVerticalIndex = this.boardConfig.Value.Height - 1;
        int length = completedLineIndexes.Count;

        for (int i = length - 1; i >= 0; i--)
        {
            var lineIndex = completedLineIndexes[i];

            for (int y = lineIndex; y < highestVerticalIndex; y++)
            {
                for (int x = 0; x < this.boardConfig.Value.Width; x++)
                {
                    board.Value[x][y] = board.Value[x][y + 1];
                }
            }

            highestVerticalIndex--;
        }
    }
}