using UnityEngine;

namespace Game.Mono
{
    public class BoardGizmoDrawer : MonoBehaviour
    {
        [SerializeField] private BoardConfigHolder config;

        [SerializeField] private Color validColor = Color.yellow;
        [SerializeField] private Vector3 origin; // bottom-left corner of the board

        private void OnDrawGizmos()
        {
            var board = BoardCellArrayHolder.Instance;
            if (!board) return;

            float size = config.Value.CellWorldSize;

            for (int x = 0; x < board.Value.Value.Length; x++)
            {
                var column = board.Value.Value[x];
                if (column == null) continue;

                for (int y = 0; y < column.Length; y++)
                {
                    if (!column[y].IsValid)
                        continue;

                    Vector3 worldPos = origin + new Vector3(
                        x * size,
                        y * size,
                        0f
                    );

                    DrawCell(worldPos, size);
                }
            }
        }

        private void DrawCell(Vector3 bottomLeft, float size)
        {
            Vector3 center = bottomLeft + new Vector3(size, size, 0f) * 0.5f;

            Gizmos.color = validColor;
            Gizmos.DrawCube(center, new Vector3(size, size, 0f));

            // Optional: outline for clarity
            Gizmos.color = Color.black;
            Gizmos.DrawWireCube(center, new Vector3(size, size, 0f));
        }
    }
}