using Game.Common;
using Game.Domain;
using System;
using System.Collections.Generic;

namespace Game.Mono.Lines;

[Serializable]
public class CompletedLinesCheckAndClearer : MicroBehaviour
{
    private BlockLockedEvent blockLockedEvent;
    private BoardConfig boardConfig;
    private BoardCellArray boardCellArray;

    public override void InjectDependencies()
    {
        this.InjectSingle(out this.blockLockedEvent);
        this.InjectSingle(out this.boardConfig);
        this.InjectSingle(out this.boardCellArray);
    }

    public override void Update()
    {
        if (!this.blockLockedEvent.Value)
            return;

        var completedLineIndexes = this.GetCompletedLineIndexes(this.boardCellArray);

        if (completedLineIndexes.Count == 0)
            return;

        this.ClearCompletedLines(this.boardCellArray, completedLineIndexes);
    }

    private List<int> GetCompletedLineIndexes(BoardCellArray board)
    {
        var completedLineIndexes = new List<int>();

        for (int y = 0; y < this.boardConfig.Height; y++)
        {
            bool isLineCompleted = true;

            for (int x = 0; x < this.boardConfig.Width; x++)
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
        int highestVerticalIndex = this.boardConfig.Height - 1;
        int length = completedLineIndexes.Count;

        for (int i = length - 1; i >= 0; i--)
        {
            var lineIndex = completedLineIndexes[i];

            for (int y = lineIndex; y < highestVerticalIndex; y++)
            {
                for (int x = 0; x < this.boardConfig.Width; x++)
                {
                    board.Value[x][y] = board.Value[x][y + 1];
                }
            }

            highestVerticalIndex--;
        }
    }
}