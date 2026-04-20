using Game.Domain;
using Reflex.Core;
using System;
using UnityEngine;

namespace Game.Mono
{
    [Serializable]
    public class CanLockCurrentBlockTagHolder : MonoBehaviour, IInstaller
    {
        public CanLockCurrentBlockTag Value;

        public void InstallBindings(ContainerBuilder builder)
        {
            builder.RegisterValue(this.Value);
        }
    }
}