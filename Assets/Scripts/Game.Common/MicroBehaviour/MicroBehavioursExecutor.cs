using Reflex.Core;
using Reflex.Injectors;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Common
{
    public class MicroBehavioursExecutor : SaiMonoBehaviour, IAttributeInjectionContract
    {
        [SerializeReference, SubclassSelector]
        private List<MicroBehaviour> microBehaviours;

        private void OnValidate()
        {
            foreach (var microBehaviour in this.microBehaviours)
            {
                microBehaviour.Init(this);
            }
        }

        public virtual void ReflexInject(Container container)
        {
            foreach (var microBehaviour in this.microBehaviours)
            {
                microBehaviour.AssignSceneContainer();
                microBehaviour.InjectDependencies();
            }
        }

        private void Awake()
        {
            foreach (var microBehaviour in this.microBehaviours)
            {
                microBehaviour.Awake();
            }
        }

        protected override void LoadComponents()
        {
            foreach (var microBehaviour in this.microBehaviours)
            {
                microBehaviour.LoadComponents();
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