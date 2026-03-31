using System.Collections.Generic;
using UnityEngine;

namespace Game.Common
{
    public class MicroBehavioursExecutor : MonoBehaviour
    {
        [SerializeReference, SubclassSelector]
        private List<MicroBehaviour> microBehaviours;

        private void Awake()
        {
            foreach (var microBehaviour in this.microBehaviours)
            {
                microBehaviour.Awake();
            }
        }

        private void Start()
        {
            foreach (var microBehaviour in this.microBehaviours)
            {
                microBehaviour.Start();
            }
        }

        private void FixedUpdate()
        {
            foreach (var microBehaviour in this.microBehaviours)
            {
                microBehaviour.FixedUpdate();
            }
        }

        private void Update()
        {
            foreach (var microBehaviour in this.microBehaviours)
            {
                microBehaviour.Update();
            }
        }

        private void OnEnable()
        {
            foreach (var microBehaviour in this.microBehaviours)
            {
                microBehaviour.OnEnable();
            }
        }

        private void OnDisable()
        {
            foreach (var microBehaviour in this.microBehaviours)
            {
                microBehaviour.OnDisable();
            }
        }

        private void OnDestroy()
        {
            foreach (var microBehaviour in this.microBehaviours)
            {
                microBehaviour.OnDestroy();
            }
        }
    }
}