using Game.Common;
using Game.Domain.PubSub.Messengers;
using UnityEngine;
using ZBase.Foundation.PubSub;

namespace Game.Mono
{
    public class CurrentBlockDrawer : SaiMonoBehaviour
    {
        [SerializeField] private BoardConfigHolder boardConfig;
        [SerializeField] private CurrentBlockRef currentBlock;
        [SerializeField] private GameObject currentBlockPresenter;
        [SerializeField] private Material material;
        private ISubscription redrawCurrentBlockMessageSubscription;

        protected override void LoadComponents()
        {
            this.FindFirstObjectByType(out this.boardConfig);
            this.FindFirstObjectByType(out this.currentBlock);
        }

        private void OnEnable()
        {
            this.redrawCurrentBlockMessageSubscription = GameplayMessenger.MessageSubscriber
                .Subscribe<RedrawCurrentBlockMessage>(_ => this.DrawCurrentBlock());
        }

        private void OnDisable()
        {
            this.redrawCurrentBlockMessageSubscription.Dispose();
        }

        private void DrawCurrentBlock()
        {
            if (this.currentBlockPresenter)
                Destroy(this.currentBlockPresenter);

            Color[] colors = new Color[]
            {
                this.currentBlock.Value.Color,
            };

            this.currentBlockPresenter = CellsMeshBuilder.CreateCellsPresenterGO(
                this.boardConfig.Value.CellWorldSize,
                this.currentBlock.Value.CenterPosition,
                this.currentBlock.Value.CellOffsets,
                colors,
                this.material);

            this.currentBlockPresenter.transform.SetParent(transform);
        }
    }
}