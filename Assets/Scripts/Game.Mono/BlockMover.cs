using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Game.Mono
{
    public class BlockMover : MonoBehaviour
    {
        [SerializeField] private float moveIntervalSeconds = 1f;

        private void Start()
        {
            this.TaskRunner().Forget();
        }

        private async UniTaskVoid TaskRunner()
        {
            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(this.moveIntervalSeconds));
                this.Move();
            }
        }

        private void Move()
        {
            var tempPos = transform.position;
            tempPos.y -= BoardConstants.cellWorldSize;

            transform.position = tempPos;
        }
    }
}
