using Game.Domain.Lines;
using Reflex.Core;
using UnityEngine;

namespace Game.Mono.Lines
{
    public class CompletedLinesEventHolder : MonoBehaviour, IInstaller
    {
        public CompletedLinesEvent Value = new() { Value = false };

        public void InstallBindings(ContainerBuilder builder)
        {
            builder.RegisterValue(this.Value);
        }
    }
}