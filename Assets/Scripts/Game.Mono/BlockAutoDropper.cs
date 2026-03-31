using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Game.Mono
{
    [RequireComponent(typeof(CurrentBlockRef))]
    public class BlockAutoDropper : MonoBehaviour
    {
        [SerializeField] private CurrentBlockRef currentBlock;
        [SerializeField] private float autoDropIntervalSeconds = 1f;

        private void Start()
        {
            this.TaskRunner().Forget();
        }

        private async UniTaskVoid TaskRunner()
        {
            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(this.autoDropIntervalSeconds));

                if (currentBlock.Value == null)
                    continue;

                this.DropCurrentBlock();
            }
        }

        private void DropCurrentBlock()
        {
            var blockData = this.currentBlock.Value;

            var tempPos = blockData.CenterPosition;
            tempPos.y--;

            bool canMove =
                BlockPositionCheckingHelpers.CheckBottomBorder(tempPos, blockData.CellOffsets);

            if (!canMove) return;

            blockData.CenterPosition = tempPos;
        }
    }
}
