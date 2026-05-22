using Reflex.Core;
using UnityEngine;

namespace Game.Mono
{
    public class BoardConfigHolder : MonoBehaviour, IInstaller
    {
        public BoardConfig Value;

        public void InstallBindings(ContainerBuilder builder)
        {
            builder.RegisterValue(this.Value);
        }
    }
}