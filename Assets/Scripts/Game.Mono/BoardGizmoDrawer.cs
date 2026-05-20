using Game.Domain;
using Reflex.Attributes;
using UnityEngine;

namespace Game.Mono
{
    [SourceGeneratorInjectable]
    public partial class BoardGizmoDrawer : MonoBehaviour
    {
        [Inject] private BoardConfig config;
        [Inject] private BoardCellArray board;

        [SerializeField] private Color validColor = Color.yellow;
        [SerializeField] private Vector3 origin; // bottom-left corner of the board

        private void OnDrawGizmos()
        {
            if (board == null) return;

            float size = config.CellWorldSize;

            for (int x = 0; x < board.Value.Length; x++)
            {
                var column = board.Value[x];
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