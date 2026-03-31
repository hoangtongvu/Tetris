using Unity.Mathematics;
using UnityEngine;

namespace Game.Mono
{
    [RequireComponent(typeof(BoardConfigHolder))]
    public class CellDrawer : MonoBehaviour
    {
        public BoardConfigHolder config;
        public Material material;
        public Color color;

        private void Start()
        {
            var offsets = new int2[] { new(0, 0), new(1, 0), new(-1, 0), new(0, 1) };

            var go = BlockMeshBuilder.CreateBlock(
                this.config.Value,
                new int2(4, 18),
                offsets,
                this.material,
                transform);

            go.GetComponent<MeshRenderer>().material.color = this.color;
        }
    }
}