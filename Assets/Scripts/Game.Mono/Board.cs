using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Mono
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private bool[,] grid = new bool[BoardConstants.BOARD_WIDTH, BoardConstants.BOARD_HEIGHT];

        public static readonly List<List<int2>> pieces = new()
        {
            new() { new(0,0), new(1,0), new(-1,0), new(0,1) }
        };

        private void Start()
        {
        }

        private void Update()
        {
        }
    }
}
