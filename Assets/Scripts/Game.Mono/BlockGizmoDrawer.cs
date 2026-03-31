using Unity.Mathematics;
using UnityEngine;

namespace Game.Mono
{
    public class BlockGizmoDrawer : MonoBehaviour
    {
        public BoardConfigHolder boardConfig;
        public CurrentBlockRef currentBlock;

        private void OnDrawGizmos()
        {
            if (currentBlock == null || currentBlock.Value.CellOffsets == null)
                return;

            float size = boardConfig.Value.CellWorldSize;

            for (int i = 0; i < currentBlock.Value.CellOffsets.Length; i++)
            {
                if (i == 0)
                    Gizmos.color = Color.blue;
                else
                    Gizmos.color = Color.green;

                int2 offset = currentBlock.Value.CellOffsets[i];
                int2 cellPos = currentBlock.Value.CenterPosition + offset;

                Vector3 worldPos = GridToWorld(cellPos, size);
                worldPos.x += size / 2;
                worldPos.y += size / 2;

                var tempSize = Vector3.one * size;
                tempSize.z = 0f;
                Gizmos.DrawCube(worldPos, tempSize);
            }
        }

        private Vector3 GridToWorld(int2 gridPos, float size)
        {
            return new Vector3(
                gridPos.x * size,
                gridPos.y * size,
                0f
            );
        }
    }
}