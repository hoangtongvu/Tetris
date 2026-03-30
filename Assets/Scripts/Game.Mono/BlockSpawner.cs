using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Mono
{
    public struct CellData
    {
        // Coordination xy (Optional)
        // Block id
        // Presenter ref (Update on block move / block changed)
    }


    public class BlockSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject cellPresenterPrefab;
        [SerializeField] private GameObject blockPrefab;

        [SerializeField] private float spawnIntervalSeconds = 1f;

        public static readonly List<List<int2>> blocks = new()
        {
            // T-piece
            new() { new(0,0), new(1,0), new(-1,0), new(0,1) },
            // I-piece
            new() { new(0,0), new(-1,0), new(1,0), new(2,0) },
            // O-piece
            new() { new(0,0), new(1,0), new(0,1), new(1,1) },
            // S-piece
            new() { new(0,0), new(1,0), new(0,1), new(-1,1) },
            // Z-piece
            new() { new(0,0), new(-1,0), new(0,1), new(1,1) },
            // J-piece
            new() { new(0,0), new(-1,0), new(1,0), new(-1,1) },
            // L-piece
            new() { new(0,0), new(-1,0), new(1,0), new(1,1) },
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

                int blockIndex = UnityEngine.Random.Range(0, blocks.Count);
                this.SpawnBlock(blockIndex, 0, 0);
            }
        }

        private GameObject SpawnBlock(int blockIndex, float x, float y)
        {
            List<int2> block = blocks[blockIndex];

            var blockRoot = Instantiate(this.blockPrefab);
            blockRoot.name = $"Block_{blockIndex}";
            blockRoot.transform.position = new Vector3(x, y, 0f);

            foreach (int2 cell in block)
            {
                float worldX = x + cell.x * BoardConstants.cellWorldSize;
                float worldY = y + cell.y * BoardConstants.cellWorldSize;

                GameObject cellGO = Instantiate(this.cellPresenterPrefab, new Vector3(worldX, worldY, 0f), Quaternion.identity);
                cellGO.transform.SetParent(blockRoot.transform);
            }

            return blockRoot;
        }
    }
}
