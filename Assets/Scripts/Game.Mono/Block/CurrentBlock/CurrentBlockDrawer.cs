using Game.Common;
using Game.Domain.PubSub.Messengers;
using Reflex.Attributes;
using UnityEngine;
using ZBase.Foundation.PubSub;

namespace Game.Mono.Block
{
    [SourceGeneratorInjectable]
    public partial class CurrentBlockDrawer : SaiMonoBehaviour
    {
        [Inject] private BoardConfig boardConfig;
        [Inject] private CurrentBlockRef currentBlock;
        [SerializeField] private GameObject currentBlockPresenter;
        [SerializeField] private Material material;
        private ISubscription redrawCurrentBlockMessageSubscription;

        private void OnEnable()
        {
            this.redrawCurrentBlockMessageSubscription = GameplayMessenger.MessageSubscriber
                .Subscribe<DrawCurrentBlockMessage>(_ => this.DrawCurrentBlock());
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
                this.boardConfig.CellWorldSize,
                this.boardConfig.BoardWorldOffset,
                this.currentBlock.Value.CenterPosition,
                this.currentBlock.Value.CellOffsets,
                colors,
                this.material);

            this.currentBlockPresenter.transform.SetParent(transform);
        }
    }
}