using Game.Common;
using Game.Domain.PubSub.Messengers;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;
using ZBase.Foundation.PubSub;

namespace Game.Mono
{
    public class BoardDrawer : SaiMonoBehaviour
    {
        [SerializeField] private BoardConfigHolder boardConfig;
        [SerializeField] private GameObject currentBoardPresenter;
        [SerializeField] private Material material;
        private ISubscription drawBoardMessageSubscription;

        protected override void LoadComponents()
        {
            this.FindAnyObjectByType(out this.boardConfig);
        }

        private void OnEnable()
        {
            this.drawBoardMessageSubscription = GameplayMessenger.MessageSubscriber
                .Subscribe<DrawBoardMessage>(_ => this.DrawActiveCells());
        }

        private void OnDisable()
        {
            this.drawBoardMessageSubscription.Dispose();
        }

        private void DrawActiveCells()
        {
            if (this.currentBoardPresenter)
                Destroy(this.currentBoardPresenter);

            var activeCellPositions = new List<int2>();
            var activeCellColors = new List<Color>();
            var board = BoardCellArrayHolder.Instance.Value.Value;

            for (int x = 0; x < this.boardConfig.Value.Width; x++)
            {
                for (int y = 0; y < this.boardConfig.Value.Height; y++)
                {
                    var cell = board[x][y];
                    if (!cell.IsValid) continue;

                    activeCellPositions.Add(new(x, y));
                    activeCellColors.Add(cell.Color);
                }
            }

            this.currentBoardPresenter = CellsMeshBuilder.CreateCellsPresenterGO(
                this.boardConfig.Value.CellWorldSize,
                int2.zero,
                CollectionsMarshal.AsSpan(activeCellPositions),
                CollectionsMarshal.AsSpan(activeCellColors),
                this.material,
                "BoardPresenter");

            this.currentBoardPresenter.transform.SetParent(transform);
        }
    }
}