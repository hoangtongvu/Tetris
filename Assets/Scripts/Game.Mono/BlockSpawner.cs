using Cysharp.Threading.Tasks;
using System;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Mono
{
    [RequireComponent(typeof(CurrentBlockRef))]
    public class BlockSpawner : MonoBehaviour
    {
        [SerializeField] private BoardConfigHolder boardConfigHolder;
        [SerializeField] private CurrentBlockRef currentBlock;
        [SerializeField] private float spawnIntervalSeconds = 1f;

        public static readonly int2[][] blockOffsets = new int2[][]
        {
            // T-piece
            new int2[] { new(0,0), new(1,0), new(-1,0), new(0,1) },
            // I-piece
            new int2[] { new(0,0), new(-1,0), new(1,0), new(2,0) },
            // O-piece
            new int2[] { new(0,0), new(1,0), new(0,1), new(1,1) },
            // S-piece
            new int2[] { new(0,0), new(1,0), new(0,1), new(-1,1) },
            // Z-piece
            new int2[] { new(0,0), new(-1,0), new(0,1), new(1,1) },
            // J-piece
            new int2[] { new(0,0), new(-1,0), new(1,0), new(-1,1) },
            // L-piece
            new int2[] { new(0,0), new(-1,0), new(1,0), new(1,1) },
        };

        private void Start()
        {
            this.TaskRunner().Forget();
        }

        private async UniTaskVoid TaskRunner()
        {
            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(this.spawnIntervalSeconds));

                int blockIndex = UnityEngine.Random.Range(0, blockOffsets.Length);

                this.currentBlock.Value = new BlockData
                {
                    CenterPosition = new(this.boardConfigHolder.Value.Width / 2, this.boardConfigHolder.Value.Height + 1),
                    CellOffsets = blockOffsets[blockIndex],
                };
            }
        }
    }
}
