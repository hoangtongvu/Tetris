using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Mono
{
    [RequireComponent(typeof(CurrentBlockRef))]
    public class PlayerBlockVerticalMover : MonoBehaviour
    {
        [SerializeField] private BoardConfigHolder boardConfig;
        [SerializeField] private CurrentBlockRef currentBlock;
        [SerializeField] private float moveCooldownSeconds = 0.1f;

        private void Start()
        {
            this.GetTask().Forget();
        }

        private async UniTaskVoid GetTask()
        {
            while (true)
            {
                await UniTask.WaitForSeconds(this.moveCooldownSeconds);

                if (currentBlock.Value == null)
                    continue;

                if (Input.GetKey(KeyCode.DownArrow))
                {
                    this.MoveBlockY(-1);
                }
            }
        }

        private void MoveBlockY(sbyte input)
        {
            var blockData = this.currentBlock.Value;

            var tempPos = blockData.CenterPosition;
            tempPos.y += input;

            bool canMove =
                BlockPositionCheckingHelpers.CheckBottomBorder(tempPos, blockData.CellOffsets);

            if (!canMove) return;

            blockData.CenterPosition = tempPos;
        }
    }
}
