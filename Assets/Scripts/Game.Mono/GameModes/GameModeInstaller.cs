using Game.Common;
using Game.ScriptableObjects.GameModes;
using Reflex.Core;
using UnityEngine;

namespace Game.Mono.GameModes
{
    public class GameModeInstaller : SaiMonoBehaviour, IInstaller
    {
        [SerializeField] private GameModeProfilesSO gameModeProfiles;

        public void InstallBindings(ContainerBuilder builder)
        {
            builder.RegisterValue(this.gameModeProfiles);
        }
    }
}