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
        [SerializeField] private Color color;
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

            this.currentBlockPresenter = BlockMeshBuilder.CreateBlock(
                this.boardConfig.Value,
                this.currentBlock.Value.CenterPosition,
                this.currentBlock.Value.CellOffsets,
                this.material,
                transform);

            this.currentBlockPresenter.GetComponent<MeshRenderer>().material.color = this.color;
        }
    }
}