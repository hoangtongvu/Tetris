using Reflex.Attributes;
using UnityEngine;

namespace Game.Mono
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

            // Define 4 corners (clockwise or counter-clockwise)
            Vector3[] corners = new Vector3[4]
            {
                new Vector3(0, 0, 0),
                new Vector3(width, 0, 0),
                new Vector3(width, height, 0),
                new Vector3(0, height, 0)
            };

            lineRenderer.SetPositions(corners);
        }
    }
}