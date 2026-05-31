using Reflex.Attributes;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Mono.Board
{
    [SourceGeneratorInjectable]
    [RequireComponent(typeof(LineRenderer))]
    public partial class BoardBorderDrawer : MonoBehaviour
    {
        [Inject] private BoardConfig config;
        [SerializeField] private LineRenderer lineRenderer;

        void Start()
        {
            this.DrawBorder();
        }

        private void DrawBorder()
        {
            lineRenderer.useWorldSpace = false;
            lineRenderer.loop = true; // closes the rectangle automatically
            lineRenderer.positionCount = 4;

            float width = config.Width * config.CellWorldSize;
            float height = config.Height * config.CellWorldSize;
            float2 offset = config.BoardWorldOffset;

            // Define 4 corners (clockwise or counter-clockwise)
            Vector3[] corners = new Vector3[4]
            {
                new Vector3(offset.x, offset.y, 0),
                new Vector3(width + offset.x, offset.y, 0),
                new Vector3(width + offset.x, height + offset.y, 0),
                new Vector3(offset.x, height + offset.y, 0)
            };

            lineRenderer.SetPositions(corners);
        }
    }
}