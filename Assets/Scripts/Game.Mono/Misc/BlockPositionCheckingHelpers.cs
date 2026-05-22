using Game.Domain;
using Unity.Mathematics;

namespace Game.Mono;

public static class BlockPositionCheckingHelpers
{
    public static bool CheckHorizontalBorders(BoardConfig boardConfig, int2 blockPos, int2[] cellOffsets)
    {
        foreach (var cellOffset in cellOffsets)
        {
            var cellPos = blockPos + cellOffset;

            if (cellPos.x < 0)
                return false;

            if (cellPos.x >= boardConfig.Width)
                return false;
        }

        return true;
    }

    public static bool CheckBottomBorder(int2 blockPos, int2[] cellOffsets)
    {
        foreach (var cellOffset in cellOffsets)
        {
            var cellPos = blockPos + cellOffset;

            if (cellPos.y < 0)
                return false;
        }

        return true;
    }

    public static bool CheckCollision(BoardConfig boardConfig, BoardCellArray board, int2 blockPos, int2[] cellOffsets)
    {
        foreach (var cellOffset in cellOffsets)
        {
            var cellPos = blockPos + cellOffset;

            if (cellPos.y >= boardConfig.Height)
                return true;

            var cellData = board.Value[cellPos.x][cellPos.y];

            if (cellData.IsValid)
                return false;
        }

        return true;
    }
}