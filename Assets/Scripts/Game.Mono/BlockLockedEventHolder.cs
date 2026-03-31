using System;
using UnityEngine;

namespace Game.Mono
{
    [Serializable]
    public class BlockLockedEventHolder : MonoBehaviour
    {
        public BlockLockedEvent Value = new() { Value = true };
    }
}