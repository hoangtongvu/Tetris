using UnityEngine;

namespace Game.Mono
{
    [RequireComponent(typeof(BoardConfigHolder))]
    public class BoardCellArrayInitializer : MonoBehaviour
    {
        public BoardConfigHolder config;

        private void Start()
        {
            var boardCellsArrayHolder = gameObject.AddComponent<BoardCellArrayHolder>();
            this.InitBoardCells(boardCellsArrayHolder);
            BoardCellArrayHolder.Instance = boardCellsArrayHolder;

            Destroy(this);
        }

        private void InitBoardCells(BoardCellArrayHolder boardCellsArrayHolder)
        {
            var cellArray = new CellData[this.config.Value.Width][];

            for (int i = 0; i < this.config.Value.Width; i++)
            {
                cellArray[i] = new CellData[this.config.Value.Height];
            }

            boardCellsArrayHolder.Value = new()
            {
                Value = cellArray,
            };
        }
    }
}