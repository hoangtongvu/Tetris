using Game.Common;
using Game.Domain;
using Game.Domain.PubSub.Messengers;
using Reflex.Attributes;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;
using ZBase.Foundation.PubSub;

namespace Game.Mono
{
    [SourceGeneratorInjectable]
    public partial class BoardDrawer : SaiMonoBehaviour
    {
        [Inject] private BoardConfig boardConfig;
        [Inject] private BoardCellArray boardCellArray;
        [SerializeField] private GameObject currentBoardPresenter;
        [SerializeField] private Material material;
        private ISubscription drawBoardMessageSubscription;

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
            var board = this.boardCellArray.Value;

            for (int x = 0; x < this.boardConfig.Width; x++)
            {
                for (int y = 0; y < this.boardConfig.Height; y++)
                {
                    var cell = board[x][y];
                    if (!cell.IsValid) continue;

                    activeCellPositions.Add(new(x, y));
                    activeCellColors.Add(cell.Color);
                }
            }

            this.currentBoardPresenter = CellsMeshBuilder.CreateCellsPresenterGO(
                this.boardConfig.CellWorldSize,
                int2.zero,
                CollectionsMarshal.AsSpan(activeCellPositions),
                CollectionsMarshal.AsSpan(activeCellColors),
                this.material,
                "BoardPresenter");

            this.currentBoardPresenter.transform.SetParent(transform);
        }
    }
}