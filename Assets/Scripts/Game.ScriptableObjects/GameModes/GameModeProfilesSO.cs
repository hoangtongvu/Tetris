using AYellowpaper.SerializedCollections;
using Game.Domain.GameModes;
using UnityEngine;

namespace Game.ScriptableObjects.GameModes
{
    [CreateAssetMenu(fileName = "GameModeProfilesSO", menuName = "SO/GameModeProfilesSO")]
    public class GameModeProfilesSO : ScriptableObject
    {
        public SerializedDictionary<GameMode, GameModeData> Value;
    }
}