using Game.Domain;
using Reflex.Core;
using UnityEngine;

namespace Game.Mono
{
    public class CurrentBlockRef : MonoBehaviour, IInstaller
    {
        public BlockData Value;

        public void InstallBindings(ContainerBuilder builder)
        {
            builder.RegisterValue(this);
        }
    }
}