using Game.Domain;
using Reflex.Core;
using UnityEngine;

namespace Game.Mono
{
    public class BlockLockedEventHolder : MonoBehaviour, IInstaller
    {
        public BlockLockedEvent Value = new() { Value = true };

        public void InstallBindings(ContainerBuilder builder)
        {
            builder.RegisterValue(this.Value);
        }
    }
}