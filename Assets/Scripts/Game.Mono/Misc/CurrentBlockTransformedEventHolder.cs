using Game.Domain;
using Reflex.Core;
using UnityEngine;

namespace Game.Mono
{
    public class CurrentBlockTransformedEventHolder : MonoBehaviour, IInstaller
    {
        public CurrentBlockTransformedEvent Value;

        public void InstallBindings(ContainerBuilder builder)
        {
            builder.RegisterValue(this.Value);
        }
    }
}